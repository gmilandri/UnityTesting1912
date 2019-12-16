using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour, ICreature, ISpawnable {

	[HideInInspector]
	public IMovement HumanMovement { get; private set; }

	private WorldManager _worldManager;

	[SerializeField]
	private ISpawnable mySpawnDestination;

	public static int Count { get; private set; }

	private float _gatheringDistance = 2.5f;

	void Awake()
	{
		HumanMovement = GetComponent<WalkMovement>();
		_worldManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldManager>();
	}
	void OnEnable()
	{
		Count++;
	}

	void OnDisable()
	{
		Count--;
	}

	void Update()
	{
		if (mySpawnDestination == null)
		{
			var spawns = _worldManager.WorldObjects;
			var myPos = gameObject.transform.position;
			var minDistance = float.MaxValue;
			var closestSpawnIndex = 0;

			for (int i = 0; i < spawns.Count; i++)
			{
				if (spawns[i] is Tree && !spawns[i].HasBeenGathered())
				{
					var pos = spawns[i].ThisGameObject().transform.position;
					if (Vector3.Distance(pos, myPos) < minDistance)
					{
						minDistance = Vector3.Distance(pos, myPos);
						closestSpawnIndex = i;
					}
				}
			}

			mySpawnDestination = spawns[closestSpawnIndex];

			GetComponent<NavMeshAgent>().SetDestination(mySpawnDestination.ThisGameObject().transform.position);
		}
		else if (Vector3.Distance(mySpawnDestination.ThisGameObject().transform.position, gameObject.transform.position) > _gatheringDistance)
		{
			GetComponent<NavMeshAgent>().SetDestination(mySpawnDestination.ThisGameObject().transform.position);
		}
		else
		{
			Debug.Log("Chopping Down Tree...");
			mySpawnDestination.SetAside();
			StartCoroutine(mySpawnDestination.ThisGameObject().GetComponent<Tree>().ChopTree()); //CHANGE RESOURCE INTO IGATHERABLE AND GENERALIZE THIS
			mySpawnDestination = null;
			GetComponent<NavMeshAgent>().ResetPath();

		}

	}

	public bool HasBeenGathered() => true;

	public void SetAside() => gameObject.transform.position = new Vector3(10000f, 10000f, 10000f);

	public GameObject ThisGameObject() => gameObject;

	public void InstantiateThis(float positiveMax, List<ISpawnable> spawns)
	{
		var foundPosition = false;
		int breakLoop = 0;

		do
		{
			int randomX = UnityEngine.Random.Range(0, _worldManager.GetGridSize);
			int randomZ = UnityEngine.Random.Range(0, _worldManager.GetGridSize);

			if (_worldManager.gridCells[randomX, randomZ].IsEmpty)
			{
				foundPosition = true;

				if (!spawns.Contains(this))
				{
					spawns.Add(this);
				}

				gameObject.transform.position = new Vector3(_worldManager.gridCells[randomX, randomZ].gameObject.transform.position.x, 1f, _worldManager.gridCells[randomX, randomZ].gameObject.transform.position.z);
				//_worldManager.gridCells[randomX, randomZ].GridObject = this;
				GetComponent<NavMeshAgent>().enabled = true;
			}

			breakLoop++;
			if (breakLoop == 1000)
			{
				Debug.LogError("No valid position found for a human.");
				break;
			}

		}
		while (!foundPosition);
	}
}
