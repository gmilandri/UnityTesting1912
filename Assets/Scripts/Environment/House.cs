using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, ISpawnable {

	private WorldManager _worldManager;
	private const float _distancefromHouses = 5f;

	// Use this for initialization
	void Start()
	{
		_worldManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldManager>();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public GameObject ThisGameObject() => gameObject;

	public float MyMinimumDistance() => _distancefromHouses;

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

	public void InstantiateThis(float negativeMax, float positiveMax, List<ISpawnable> spawns)
	{
		var foundPosition = false;
		int breakLoop = 0;

		do
		{
			var housePos = new Vector3(Random.Range(negativeMax, positiveMax), 1.5f, Random.Range(negativeMax, positiveMax));

			var minDistance = float.MaxValue;
			var closestSpawnIndex = 0;

			if (spawns.Count > 1)
			{
				for (int i = 0; i < spawns.Count; i++)
				{
					var pos = spawns[i].ThisGameObject().transform.position;
					if (Vector3.Distance(pos, housePos) < minDistance)
					{
						minDistance = Vector3.Distance(pos, housePos);
						closestSpawnIndex = i;
					}
				}
			}
			if (spawns.Count == 0 || IsAtMinimumDistance(spawns[closestSpawnIndex]))
			{
				foundPosition = true;
				gameObject.transform.position = housePos;
				spawns.Add(this);
			}
			breakLoop++;
			if (breakLoop == 100)
			{
				Debug.LogError("No valid position found for a house.");
				break;
			}

		}
		while (!foundPosition);
	}
}
