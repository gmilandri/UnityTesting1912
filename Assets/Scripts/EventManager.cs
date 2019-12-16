using System;
using UnityEngine;

public class EventManager : MonoBehaviour {

	public event EventHandler TreeChopped;
	public virtual void OnTreeChopped(EventArgs e) => TreeChopped?.Invoke(this, e);

	void Start () {
	
	}
	
	void Update () {

	}
}
