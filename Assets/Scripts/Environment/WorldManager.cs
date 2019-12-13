using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

	private GameObject _ground;

	[SerializeField]
	private GameObject _treePrefab;

	public const int TreeCount = 50;
	private const float _distanceBetweenTrees = 1.5f;

	private List<GameObject> _trees;

	void Start()
	{
		_trees = new List<GameObject>();
		_ground = GameObject.FindGameObjectWithTag("Ground");
		GenerateWorld();
	}

	private void GenerateWorld()
	{
		GameObject parent = GameObject.Find("Trees");
		var treePositions = new List<Vector3>();

		for (int i = 0; i < TreeCount; i++)
		{
			GameObject newTree = Instantiate(_treePrefab, parent.transform);
			_trees.Add(newTree);

			float positiveValue = System.Math.Abs(_ground.transform.localScale.x) / 2f;
			float negativeValue = positiveValue * (-1);

			var foundPosition = false;

			do
			{
				var treePos = new Vector3(Random.Range(negativeValue, positiveValue), 0f, Random.Range(negativeValue, positiveValue));

				var minDistance = float.MaxValue;

				if (treePositions.Count != 0)
				{
					foreach (var pos in treePositions)
					{
						if (Vector3.Distance(pos, treePos) < minDistance)
							minDistance = Vector3.Distance(pos, treePos);
					}
				}
				if (minDistance > _distanceBetweenTrees || treePositions.Count == 0)
				{
					foundPosition = true;
					treePositions.Add(treePos);
					newTree.transform.position = treePos;
				}
			}
			while (!foundPosition);

		}
	}

}
