using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

	private GameObject _ground;

	[SerializeField]
	private const int _groundSize = 100;
	[SerializeField]
	private GameObject _treePrefab;
	[SerializeField]
	private GameObject _housePrefab;

	public const int MaxTrees = 50;

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

	public List<ISpawnable> WorldObjects { get; private set; }

	void Start()
	{
		WorldObjects = new List<ISpawnable>();
		_ground = GameObject.FindGameObjectWithTag("Ground");
		_ground.transform.localScale = new Vector3(_groundSize, 0.1f, _groundSize);
		GenerateWorld();
	}

	private float _positiveMax => System.Math.Abs(_ground.transform.localScale.x) / 2f - 2f;

	private float _negativeMax => _positiveMax * (-1);


	private void GenerateWorld()
	{
		do
		{
			GameObject newTree = Instantiate(_treePrefab, gameObject.transform);

			newTree.GetComponent<Tree>().InstantiateThis(_negativeMax, _positiveMax, WorldObjects);

		}
		while (TreeCount() < MaxTrees);

		GameObject newHouse = Instantiate(_housePrefab, gameObject.transform);

		newHouse.GetComponent<House>().InstantiateThis(_negativeMax, _positiveMax, WorldObjects);
	}

}
