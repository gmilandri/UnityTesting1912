using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WorldManager : MonoBehaviour {

	private GameObject _ground;

	[SerializeField]
	private GameObject _cellPrefab;
	[SerializeField]
	private GameObject _housePrefab;
	[SerializeField]
	private GameObject _popPrefab;
	[SerializeField]
	private NavMeshSurface navMesh;

	public GameObject[] TreePrefabs;

	public const int StartingTrees = 50;
	public const int StartingHouses = 2;
	public const int StartingPops = 2;

	private const int _groundSize = 20;
	private const float _groundHeight = 0.1f;

	private EventManager _eventManager;

	public List<ISpawnable> WorldObjects { get; private set; }

	public GridCell[,] gridCells;

	public int timeScale = 15;


	void Start()
	{
		_eventManager = gameObject.GetComponent<EventManager>();
		_eventManager.OnTreeChopped.AddListener(_worldManager_TreeChopped);

		WorldObjects = new List<ISpawnable>();

		_ground = GameObject.FindGameObjectWithTag("Ground");

		GenerateWorld();

		//TEST
	}

	void Update()
	{
		Time.timeScale = timeScale;
	}

	public int GetGridSize => _groundSize;

	void _worldManager_TreeChopped()
	{
		Debug.Log("Planting new tree...");
		var tree = WorldObjects.First(t => t.HasBeenGathered());
		tree.InstantiateThis(WorldObjects);
	}

	private float _positiveMax => Math.Abs(_groundSize * _cellPrefab.transform.localScale.x);

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
		gridCells = new GridCell[_groundSize, _groundSize];

		for (int i = 0; i < _groundSize; i++)
		{
			for (int j = 0; j < _groundSize; j++)
			{
				GameObject newCell = Instantiate(_cellPrefab, _ground.transform);
				newCell.GetComponent<MeshRenderer>().material.color = Color.green;
				newCell.transform.position = new Vector3(i * 5f, 0f, j * 5f);
				gridCells[i, j] = newCell.GetComponent<GridCell>();
			}
		}
	}

	private void PlaceTrees ()
	{
		do
		{
			var randomTree = UnityEngine.Random.Range(0, 3);

			GameObject newTree = Instantiate(TreePrefabs[randomTree], gameObject.transform);

			newTree.GetComponent<MyTree>().InstantiateThis(WorldObjects);

		}
		while (MyTree.Count < StartingTrees);
	}

	private void PlaceHouses ()
	{
		do
		{
			GameObject newHouse = Instantiate(_housePrefab, gameObject.transform);

			newHouse.GetComponent<House>().InstantiateThis(WorldObjects);

		}
		while (House.Count < StartingHouses);
	}

	private void PlacePops()
	{
		do
		{
			GameObject newPop = Instantiate(_popPrefab, gameObject.transform);

			newPop.GetComponent<Human>().InstantiateThis(WorldObjects);

		}
		while (Human.Count < StartingPops);
	}

}
