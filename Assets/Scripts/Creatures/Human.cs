using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour, ICreature, ISpawnable {

	private ISpawnable mySpawnDestination;
	public static int Count { get; private set; }
	private float _gatheringDistance = 3f;
	private Animator _animator;
	private NavMeshAgent _navMeshAgent;

	void Awake()
	{
		_animator = GetComponent<Animator>();
		_navMeshAgent = GetComponent<NavMeshAgent>();
		Count++;
	}

	void Start()
	{
		_animator.SetInteger("WeaponType_int", 0);
		_animator.SetBool("Static_b", false);		
	}

	void Update()
	{
		_animator.SetFloat("Speed_f", _navMeshAgent.velocity.magnitude / _navMeshAgent.speed);

		if (mySpawnDestination == null || mySpawnDestination.HasBeenGathered())
		{
			if (mySpawnDestination != null && mySpawnDestination.HasBeenGathered())
			{
				mySpawnDestination = null;
			}

			var spawns = WorldManager.Instance.WorldObjects;
			var myPos = gameObject.transform.position;
			var minDistance = float.MaxValue;
			var closestSpawnIndex = 0;

			for (int i = 0; i < spawns.Count; i++)
			{
				if (spawns[i] is MyTree && !spawns[i].HasBeenGathered())
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

			_navMeshAgent.SetDestination(mySpawnDestination.ThisGameObject().transform.position);
		}
		else if (Vector3.Distance(mySpawnDestination.ThisGameObject().transform.position, gameObject.transform.position) > _gatheringDistance)
		{
			_navMeshAgent.SetDestination(mySpawnDestination.ThisGameObject().transform.position);
		}
		else
		{
			mySpawnDestination.SetAside();
			StartCoroutine(mySpawnDestination.ThisGameObject().GetComponent<MyTree>().ChopTree()); //CHANGE RESOURCE INTO IGATHERABLE AND GENERALIZE THIS
		}

	}

	public bool HasBeenGathered() => false;

	public void SetAside() => gameObject.transform.position = new Vector3(10000f, 10000f, 10000f);

	public GameObject ThisGameObject() => gameObject;

	public void InstantiateThis(List<ISpawnable> spawns)
	{
		if (WorldManager.Instance.EmptyGridCells.Count != 0)
		{
			int randomIndex = UnityEngine.Random.Range(0, WorldManager.Instance.EmptyGridCells.Count);

			if (!spawns.Contains(this))
			{
				spawns.Add(this);
			}

			var myNewGridCell = WorldManager.Instance.EmptyGridCells[randomIndex];

			var pos = new Vector3(myNewGridCell.gameObject.transform.position.x - 2.5f, 0f, myNewGridCell.gameObject.transform.position.z - 2.5f);

			gameObject.transform.position = pos;

			_navMeshAgent.enabled = true;

		}
		else
		{
			Debug.LogError("No valid position found for a human.");
		}
	}
}