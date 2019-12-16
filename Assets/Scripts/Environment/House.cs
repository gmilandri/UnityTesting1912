using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, ISpawnable {

	private WorldManager _worldManager;
	public GridCell MyGridCell;

	public static int Count { get; private set; }

	// Use this for initialization
	void Awake()
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

	public void InstantiateThis(float positiveMax, List<ISpawnable> spawns)
	{
		var foundPosition = false;
		int breakLoop = 0;

		do
		{
			int randomX = UnityEngine.Random.Range(0, _worldManager.GetGridSize);
			int randomZ = UnityEngine.Random.Range(0, _worldManager.GetGridSize);

			if (_worldManager.gridCells[randomX, randomZ].IsEmpty)
			{
				foundPosition = true;

				if (!spawns.Contains(this))
				{
					spawns.Add(this);
				}

				gameObject.transform.position = _worldManager.gridCells[randomX, randomZ].gameObject.transform.position;

				_worldManager.gridCells[randomX, randomZ].GridObject = this;
				MyGridCell = _worldManager.gridCells[randomX, randomZ];

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
