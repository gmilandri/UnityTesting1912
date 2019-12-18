using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTree : MonoBehaviour, IResource {

	public GridCell MyGridCell;
	public static int Count { get; private set; }
	public bool HasBeenChoppedDown;

	public IEnumerator ChopTree()
	{
		WorldManager.Instance.OccupiedGridCells.Remove(MyGridCell);
		WorldManager.Instance.EmptyGridCells.Add(MyGridCell);
		MyGridCell.GridObject = null;
		MyGridCell = null;
		yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 10f));
		EventManager.Instance.OnTreeChopped.Invoke(this);
	}

	public bool HasBeenGathered() => HasBeenChoppedDown;

	public GameObject ThisGameObject() => gameObject;

	public void InstantiateThis (List<ISpawnable> spawns)
	{
		HasBeenChoppedDown = false;
		if (WorldManager.Instance.EmptyGridCells.Count != 0)
		{
			int randomIndex = UnityEngine.Random.Range(0, WorldManager.Instance.EmptyGridCells.Count);

			if (!spawns.Contains(this))
			{
				spawns.Add(this);
				WorldManager.Instance.Resources.Add(this);
			}

			var myNewGridCell = WorldManager.Instance.EmptyGridCells[randomIndex];

			var pos = new Vector3(myNewGridCell.gameObject.transform.position.x - 2.5f, 0f, myNewGridCell.gameObject.transform.position.z - 2.5f);

			gameObject.transform.position = pos;

			myNewGridCell.GridObject = this;

			MyGridCell = myNewGridCell;

			WorldManager.Instance.EmptyGridCells.Remove(myNewGridCell);
			WorldManager.Instance.OccupiedGridCells.Add(myNewGridCell);

		}
		else
		{
			Debug.LogError("No valid position found for a tree.");
		}
	}

	void Awake()
	{
		Count++;
	}
}
