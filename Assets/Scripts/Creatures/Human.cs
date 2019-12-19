using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Human : MonoBehaviour, ICreature {

	private IResource myResourceDestination;
	public static int Count { get; private set; }
	private float _gatheringDistance = 1f;
	private Animator _animator;
	private NavMeshAgent _navMeshAgent;
	public HumanState _humanState;

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

	//void OnAnimatorMove()
	//{
	//	transform.position = _navMeshAgent.nextPosition;
	//}

	public GameObject ThisGameObject() => gameObject;

	public void InstantiateThis()
	{
		Instantiator.Instance.WorldInstantiate(this);
	}

	private void LookForTree()
	{
		var resources = WorldManager.Instance.Resources;
		var myPos = gameObject.transform.position;
		var minDistance = float.MaxValue;
		var closestSpawnIndex = -1;

		for (int i = 0; i < resources.Count; i++)
		{
			if (!resources[i].HasBeenGathered())
			{
				var pos = resources[i].ThisGameObject().transform.position;
				if (Vector3.Distance(pos, myPos) < minDistance)
				{
					minDistance = Vector3.Distance(pos, myPos);
					closestSpawnIndex = i;
				}
			}
		}

		if (closestSpawnIndex >= 0)
		{

			myResourceDestination = resources[closestSpawnIndex];

			_navMeshAgent.SetDestination(myResourceDestination.ThisGameObject().transform.position);

			//Debug.Log("Found a tree at " + myResourceDestination.ThisGameObject().transform.position.ToString());

			_humanState = HumanState.GatheringResource;
		}
		else
		{
			Debug.Log("There was not a single gatherable tree.");
			DoSomeRest();
		}
	}

	public void ApproachTree()
	{
		if (myResourceDestination == null || myResourceDestination.HasBeenGathered())
		{
			_navMeshAgent.ResetPath();

			if (myResourceDestination != null && myResourceDestination.HasBeenGathered())
				myResourceDestination = null;

			_humanState = HumanState.LookingForResource;
		}
		else if (_navMeshAgent.hasPath && _navMeshAgent.remainingDistance < _gatheringDistance)
		{
			Debug.Log("A tree was cut down.");

			_navMeshAgent.ResetPath();

			_navMeshAgent.acceleration = 100;

			myResourceDestination.ThisGameObject().GetComponent<MyTree>().HasBeenChoppedDown = true;

			StartCoroutine(myResourceDestination.ThisGameObject().GetComponent<MyTree>().ChopTree());

			myResourceDestination = null;

			ResetHeadLook();

			StartCoroutine(DoSomeRest());
		}
		else if (_navMeshAgent.hasPath)
		{
			float turnAngle = Vector3.Cross(this.transform.position.normalized, _navMeshAgent.steeringTarget.normalized).y;

			LookAtTarget(turnAngle);
		}
	}

	IEnumerator DoSomeRest()
	{
		_humanState = HumanState.Resting;

		yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 5f));

		//Debug.Log("Ending rest!");

		_humanState = HumanState.LookingForResource;
	}

	void LookAtTarget(float angle)
	{
		//Debug.Log("Rotating head at " + angle.ToString());
		_animator.SetFloat("Head_Horizontal_f", angle);
	}

	void ResetHeadLook()
	{
		_animator.SetFloat("Head_Horizontal_f", 0f);
	}

	public enum HumanState
	{
		LookingForResource,
		GatheringResource,
		Resting
	}
}