using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour, ICreature, ISpawnable {

	[HideInInspector]
	public IMovement HumanMovement { get; private set; }

	private WorldManager _worldManager;
	[SerializeField]
	private const float _distancefromPops = 3f;

	[SerializeField]
	private ISpawnable mySpawnDestination;

	public static int Count { get; private set; }

	void Start()
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
		if (IsBeyondMinimumDistance(mySpawnDestination))
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

	public float MyMinimumDistance() => _distancefromPops;

	public bool IsBeyondMinimumDistance(ISpawnable other)
	{
		if (other == null)
			return true;

		var otherPos = other.ThisGameObject().transform.position;
		var myPos = gameObject.transform.position;
		var distance = Vector3.Distance(myPos, otherPos);
		float minDistance = MyMinimumDistance();
		if (other.MyMinimumDistance() > minDistance)
			minDistance = other.MyMinimumDistance();

		if (distance > minDistance)
			return true;
		return false;

	}

	public void InstantiateThis(float positiveMax, List<ISpawnable> spawns)
	{
		var foundPosition = false;
		int breakLoop = 0;

		do
		{
			var popPos = new Vector3(UnityEngine.Random.Range(0f, positiveMax), 1f, UnityEngine.Random.Range(0f, positiveMax));

			var minDistance = float.MaxValue;
			var closestSpawnIndex = 0;

			if (spawns.Count > 1)
			{
				for (int i = 0; i < spawns.Count; i++)
				{
					var pos = spawns[i].ThisGameObject().transform.position;
					if (Vector3.Distance(pos, popPos) < minDistance)
					{
						minDistance = Vector3.Distance(pos, popPos);
						closestSpawnIndex = i;
					}
				}
			}
			if (spawns.Count == 0 || IsBeyondMinimumDistance(spawns[closestSpawnIndex]))
			{
				foundPosition = true;
				gameObject.transform.position = popPos;
				GetComponent<NavMeshAgent>().enabled = true;
				spawns.Add(this);
			}
			breakLoop++;
			if (breakLoop == 1000)
			{
				Debug.LogError("No valid position found for a pop.");
				break;
			}

		}
		while (!foundPosition);
	}
}
