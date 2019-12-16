using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WorldManager : MonoBehaviour {

	private GameObject _ground;

	[SerializeField]
	private GameObject _treePrefab;
	[SerializeField]
	private GameObject _housePrefab;
	[SerializeField]
	private GameObject _popPrefab;
	[SerializeField]
	private NavMeshSurface navMesh;

	public const int StartingTrees = 75;
	public const int StartingHouses = 4;
	public const int StartingPops = 3;

	private const int _groundSize = 100;
	private const float _groundHeight = 0.1f;

	private EventManager _eventManager;

	public List<ISpawnable> WorldObjects { get; private set; }

	//public int TreeCount => (from s in WorldObjects where s is Tree select s).Count();


	void Start()
	{
		_eventManager = gameObject.GetComponent<EventManager>();
		_eventManager.TreeChopped += _worldManager_TreeChopped;

		WorldObjects = new List<ISpawnable>();

		_ground = GameObject.FindGameObjectWithTag("Ground");

		GenerateWorld();
	}

	void Update()
	{

	}

	void _worldManager_TreeChopped(object sender, EventArgs e)
	{
		Debug.Log("Planting new tree...");
		var tree = WorldObjects.First(t => t.HasBeenGathered());
		tree.InstantiateThis(_positiveMax, WorldObjects);
	}

	private float _positiveMax => Math.Abs(_ground.transform.localScale.x);

	private void GenerateWorld()
	{

		SetWorldPlane();

		PlaceTrees();

		PlaceHouses();

		navMesh.BuildNavMesh();

		PlacePops();
	}

	private void SetWorldPlane ()
	{
		_ground.transform.localScale = new Vector3(_groundSize, _groundHeight, _groundSize);
		_ground.transform.position = new Vector3(_groundSize / 2f, _groundHeight / -2f, _groundSize / 2f);
	}

	private void PlaceTrees ()
	{
		do
		{
			GameObject newTree = Instantiate(_treePrefab, gameObject.transform);

			newTree.GetComponent<Tree>().InstantiateThis(_positiveMax, WorldObjects);

		}
		while (Tree.Count < StartingTrees);
	}

	private void PlaceHouses ()
	{
		do
		{
			GameObject newHouse = Instantiate(_housePrefab, gameObject.transform);

			newHouse.GetComponent<House>().InstantiateThis(_positiveMax, WorldObjects);

		}
		while (House.Count < StartingHouses);
	}

	private void PlacePops()
	{
		do
		{
			GameObject newPop = Instantiate(_popPrefab, gameObject.transform);

			newPop.GetComponent<Human>().InstantiateThis(_positiveMax, WorldObjects);

		}
		while (Human.Count < StartingPops);
	}

}
