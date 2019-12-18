﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WorldManager : Singleton<WorldManager> {

	private GameObject _ground;

	[SerializeField]
	private GameObject _cellPrefab;
	[SerializeField]
	private GameObject _housePrefab;
	[SerializeField]
	private GameObject _popPrefab;

	public GameObject[] TreePrefabs;
	public GameObject[] GenericObstacles;

	public int StartingTrees = 50;
	public int StartingHouses = 2;
	public int StartingPops = 2;
	public int StartingObstacles = 5;

	public int GroundSize = 20;

	public List<ISpawnable> WorldObjects { get; private set; }
	public List<IResource> Resources { get; private set; }

	public List<GridCell> EmptyGridCells;
	public List<GridCell> OccupiedGridCells;

	[Range(0f, 10f)]
	public float timeScale = 1f;

	void Awake()
	{
		WorldObjects = new List<ISpawnable>();
		Resources = new List<IResource>();
		EmptyGridCells = new List<GridCell>();
		OccupiedGridCells = new List<GridCell>();
		_ground = GameObject.FindGameObjectWithTag("Ground");
	}


	void Start()
	{
		EventManager.Instance.OnTreeChopped.AddListener(_worldManager_TreeChopped);

		GenerateWorld();
	}

	void Update()
	{
		Time.timeScale = timeScale;
	}

	public int GetGridSize => GroundSize;

	void _worldManager_TreeChopped(IResource resource)
	{
		resource.InstantiateThis(WorldObjects);
	}

	private void GenerateWorld()
	{

		SetWorldPlane();

		PlaceTrees();

		PlaceHouses();

		PlaceGenericObstacles();

		PlacePops();
	}

	private void SetWorldPlane ()
	{

		for (int i = 0; i < GroundSize; i++)
		{
			for (int j = 0; j < GroundSize; j++)
			{
				GameObject newCell = Instantiate(_cellPrefab, _ground.transform);
				newCell.transform.position = new Vector3(i * 5f, 0f, j * 5f);
				EmptyGridCells.Add(newCell.GetComponent<GridCell>());
			}
		}

		GameObject.Find("NavMesh").GetComponent<NavMeshSurface>().BuildNavMesh();
	}

	private void PlaceTrees ()
	{
		do
		{
			var randomTree = UnityEngine.Random.Range(0, TreePrefabs.Length);

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

	private void PlaceGenericObstacles()
	{
		do
		{
			var randomIndex = UnityEngine.Random.Range(0, GenericObstacles.Length);

			GameObject newObstacle = Instantiate(GenericObstacles[randomIndex], gameObject.transform);

			newObstacle.GetComponent<GenericObstacle>().InstantiateThis(WorldObjects);

		}
		while (GenericObstacle.Count < StartingObstacles);
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
