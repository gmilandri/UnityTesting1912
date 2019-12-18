using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, IBuilding {

	public GridCell MyGridCell;
	public static int Count { get; private set; }

	public void SetAside() => gameObject.transform.position = new Vector3(10000f, 10000f, 10000f);

	public GameObject ThisGameObject() => gameObject;

	public void InstantiateThis(List<ISpawnable> spawns)
	{
		if (WorldManager.Instance.EmptyGridCells.Count != 0)
		{
			int randomIndex = UnityEngine.Random.Range(0, WorldManager.Instance.EmptyGridCells.Count);

			if (!spawns.Contains(this))
			{
				spawns.Add(this);
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
			Debug.LogError("No valid position found for a house.");
		}
	}

	void Awake()
	{
		Count++;
	}
}
