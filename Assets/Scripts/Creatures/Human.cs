using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Human : MonoBehaviour, ICreature {

	private IResource myResourceDestination;
	public static int Count { get; private set; }
	private float _gatheringDistance = 3f;
	private Animator _animator;
	private NavMeshAgent _navMeshAgent;
	private HumanState _humanState;

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
		_humanState = HumanState.LookingForResource;
	}

	void Update()
	{
		_animator.SetFloat("Speed_f", _navMeshAgent.velocity.magnitude / _navMeshAgent.speed);

		switch (_humanState)
		{
			case HumanState.LookingForResource:
				LookForTree();
				break;
			case HumanState.GatheringResource:
				ApproachTree();
				break;
			case HumanState.Resting:
				break;
			default:
				Debug.LogError("I forgot to implement a new Human State");
				break;
		}
	}

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

	private void LookForTree()
	{
		var resources = WorldManager.Instance.Resources;
		var myPos = gameObject.transform.position;
		var minDistance = float.MaxValue;
		var closestSpawnIndex = 0;

		for (int i = 0; i < resources.Count; i++)
		{
			if (resources[i] is MyTree && !resources[i].HasBeenGathered())
			{
				var pos = resources[i].ThisGameObject().transform.position;
				if (Vector3.Distance(pos, myPos) < minDistance)
				{
					minDistance = Vector3.Distance(pos, myPos);
					closestSpawnIndex = i;
				}
			}
		}

		myResourceDestination = resources[closestSpawnIndex];

		_navMeshAgent.SetDestination(myResourceDestination.ThisGameObject().transform.position);

		Debug.Log("Found a tree at " + myResourceDestination.ThisGameObject().transform.position.ToString());

		_humanState = HumanState.GatheringResource;
	}

	public void ApproachTree()
	{
		if (myResourceDestination == null || myResourceDestination.HasBeenGathered())
		{
			_navMeshAgent.ResetPath();

			if (myResourceDestination != null && myResourceDestination.HasBeenGathered())
				myResourceDestination = null;

			StartCoroutine(DoSomeRest());
		}
		else if (Vector3.Distance(myResourceDestination.ThisGameObject().transform.position, gameObject.transform.position) < _gatheringDistance)
		{
			_navMeshAgent.ResetPath();
			_navMeshAgent.isStopped = true;

			myResourceDestination.SetAside();

			StartCoroutine(myResourceDestination.ThisGameObject().GetComponent<MyTree>().ChopTree());

			myResourceDestination = null;

			Debug.Log("I cut a tree! Starting rest...");

			StartCoroutine(DoSomeRest());
		}
	}

	IEnumerator DoSomeRest()
	{
		_humanState = HumanState.Resting;

		yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 5f));

		Debug.Log("Ending rest!");
		_navMeshAgent.isStopped = false;
		_humanState = HumanState.LookingForResource;
	}

	enum HumanState
	{
		LookingForResource,
		GatheringResource,
		Resting
	}
}