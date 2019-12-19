using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, IBuilding {

	public GridCell MyGridCell;
	public static int Count { get; private set; }

	public GameObject ThisGameObject() => gameObject;

	public void SetCell(GridCell cell) => MyGridCell = cell;

	public void InstantiateThis()
	{
		Instantiator.Instance.WorldInstantiate(this);
	}

	void Awake()
	{
		Count++;
	}
}
