using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, ISpawnable {

	private WorldManager _worldManager;
	[SerializeField]
	private const float _distancefromHouses = 7f;

	public static int Count { get; private set; }

	// Use this for initialization
	void Start()
	{
		_worldManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldManager>();
	}
	void OnEnable()
	{
		Count++;
	}

	void OnDisable()
	{
		Count--;
	}

	// Update is called once per frame
	void Update () {
		
	}
	public bool HasBeenGathered() => true;

	public void SetAside() => gameObject.transform.position = new Vector3(10000f, 10000f, 10000f);

	public GameObject ThisGameObject() => gameObject;

	public float MyMinimumDistance() => _distancefromHouses;

	public bool IsBeyondMinimumDistance(ISpawnable other)
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

	public void InstantiateThis(float positiveMax, List<ISpawnable> spawns)
	{
		var foundPosition = false;
		int breakLoop = 0;

		do
		{
			var housePos = new Vector3(UnityEngine.Random.Range(0f, positiveMax), 1.5f, UnityEngine.Random.Range(0f, positiveMax));

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
			if (spawns.Count == 0 || IsBeyondMinimumDistance(spawns[closestSpawnIndex]))
			{
				foundPosition = true;
				gameObject.transform.position = housePos;
				spawns.Add(this);
			}
			breakLoop++;
			if (breakLoop == 1000)
			{
				Debug.LogError("No valid position found for a house.");
				break;
			}

		}
		while (!foundPosition);
	}
}
