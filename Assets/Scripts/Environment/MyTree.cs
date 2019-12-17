using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTree : MonoBehaviour, ISpawnable {

	private WorldManager _worldManager;
	private EventManager _eventManager;
	public GridCell MyGridCell;
	public static int Count { get; private set; }

	public bool HasBeenChoppedDown;

	// Use this for initialization
	void Awake () {
		_worldManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldManager>();
		_eventManager = _worldManager.gameObject.GetComponent <EventManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable()
	{
		Count++;
	}

	void OnDisable()
	{
		Count--;
	}

	public IEnumerator ChopTree()
	{
		Debug.Log("Tree has been chopped... Waiting one second before inizializing it again.");
		MyGridCell.GridObject = null;
		MyGridCell = null;
		HasBeenChoppedDown = true;
		yield return new WaitForSeconds(UnityEngine.Random.Range(2, 10));
		Debug.Log("Planting Tree...");
		_eventManager.OnTreeChopped.Invoke();
	}

	public bool HasBeenGathered() => HasBeenChoppedDown;

	public void SetAside() => gameObject.transform.position = new Vector3(10000f, 10000f, 10000f);

	public GameObject ThisGameObject() => gameObject;

	public void InstantiateThis (List<ISpawnable> spawns)
	{
		var foundPosition = false;
		int breakLoop = 0;

		do
		{
			int randomX = UnityEngine.Random.Range(0, _worldManager.GetGridSize);
			int randomZ = UnityEngine.Random.Range(0, _worldManager.GetGridSize);

			if (_worldManager.gridCells[randomX,randomZ].IsEmpty)
			{
				foundPosition = true;

				if (!spawns.Contains(this))
				{
					spawns.Add(this);
				}

				var pos = new Vector3(_worldManager.gridCells[randomX, randomZ].gameObject.transform.position.x - 2.5f,
					0f,
					_worldManager.gridCells[randomX, randomZ].gameObject.transform.position.z - 2.5f);

				gameObject.transform.position = pos;

				_worldManager.gridCells[randomX, randomZ].GridObject = this;
				MyGridCell = _worldManager.gridCells[randomX, randomZ];

			}

			breakLoop++;
			if (breakLoop == 1000)
			{
				Debug.LogError("No valid position found for a tree.");
				break;
			}

		}
		while (!foundPosition);

		if (HasBeenChoppedDown)
			HasBeenChoppedDown = false;
	}
}
