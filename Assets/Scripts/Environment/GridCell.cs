using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GridCell : MonoBehaviour {

	public ISpawnable GridObject;

	public bool IsEmpty => GridObject == null ? true : false;

	// Use this for initialization

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
