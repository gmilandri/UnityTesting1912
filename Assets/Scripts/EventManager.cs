using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager> {

	public UnityEvent OnTreeChopped;

	void Start () {
		if (OnTreeChopped == null)
			OnTreeChopped = new UnityEvent();
	}
	
}
