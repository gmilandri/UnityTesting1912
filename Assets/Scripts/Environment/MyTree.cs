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

	public void SetCell(GridCell cell) => MyGridCell = cell;

	public void InstantiateThis()
	{
		HasBeenChoppedDown = false;
		Instantiator.Instance.WorldInstantiate(this);
	}

	void Awake()
	{
		Count++;
	}
}
