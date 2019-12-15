using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldManager : MonoBehaviour {

	private GameObject _ground;

	[SerializeField]
	private const int _groundSize = 100;
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

	public int TreeCount ()
	{
		var answer = 0;

		foreach (var spawnable in WorldObjects)
		{
			if (spawnable is Tree)
				answer++;
		}

		return answer;
	}

	public int HouseCount()
	{
		var answer = 0;

		foreach (var spawnable in WorldObjects)
		{
			if (spawnable is House)
				answer++;
		}

		return answer;
	}

	public int PopCount()
	{
		var answer = 0;

		foreach (var spawnable in WorldObjects)
		{
			if (spawnable is Human)
				answer++;
		}

		return answer;
	}

	public List<ISpawnable> WorldObjects { get; private set; }

	void Start()
	{
		WorldObjects = new List<ISpawnable>();
		_ground = GameObject.FindGameObjectWithTag("Ground");

		GenerateWorld();
	}

	private float _positiveMax => System.Math.Abs(_ground.transform.localScale.x) / 2f - 2f;

	private float _negativeMax => _positiveMax * (-1);


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
		_ground.transform.localScale = new Vector3(_groundSize, 0.1f, _groundSize);
	}

	private void PlaceTrees ()
	{
		do
		{
			GameObject newTree = Instantiate(_treePrefab, gameObject.transform);

			newTree.GetComponent<Tree>().InstantiateThis(_negativeMax, _positiveMax, WorldObjects);

		}
		while (TreeCount() < StartingTrees);
	}

	private void PlaceHouses ()
	{
		do
		{
			GameObject newHouse = Instantiate(_housePrefab, gameObject.transform);

			newHouse.GetComponent<House>().InstantiateThis(_negativeMax, _positiveMax, WorldObjects);

		}
		while (HouseCount() < StartingHouses);
	}

	private void PlacePops()
	{
		do
		{
			GameObject newPop = Instantiate(_popPrefab, gameObject.transform);

			newPop.GetComponent<Human>().InstantiateThis(_negativeMax, _positiveMax, WorldObjects);

		}
		while (PopCount() < StartingPops);
	}

}
