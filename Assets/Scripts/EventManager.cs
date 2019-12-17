using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {

	public UnityEvent OnTreeChopped;

	void Start () {
		if (OnTreeChopped == null)
			OnTreeChopped = new UnityEvent();
	}
	
}
