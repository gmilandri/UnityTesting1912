using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, ISpawnable {

	private WorldManager _worldManager;
	private const float _distanceBetweenTrees = 2f;

	// Use this for initialization
	void Start () {
		_worldManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject ThisGameObject() => gameObject;

	public float MyMinimumDistance() => _distanceBetweenTrees;

	public bool IsAtMinimumDistance(ISpawnable other)
	{
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

	public void InstantiateThis (float negativeMax, float positiveMax, List<ISpawnable> spawns)
	{
		var foundPosition = false;

		do
		{
			var treePos = new Vector3(Random.Range(negativeMax, positiveMax), 0f, Random.Range(negativeMax, positiveMax));

			var minDistance = float.MaxValue;
			var closestSpawnIndex = 0;

			if (spawns.Count > 1)
			{
				for (int i = 0; i < spawns.Count; i++)
				{
					var pos = spawns[i].ThisGameObject().transform.position;
					if (Vector3.Distance(pos, treePos) < minDistance)
					{
						minDistance = Vector3.Distance(pos, treePos);
						closestSpawnIndex = i;
					}
				}
			}
			if (spawns.Count == 0 || IsAtMinimumDistance(spawns[closestSpawnIndex]))
			{
				foundPosition = true;
				gameObject.transform.position = treePos;
			}

			spawns.Add(this);
		}
		while (!foundPosition);
	}
}
