using System;
using UnityEngine;

public class EventManager : MonoBehaviour {

	public event EventHandler ColorChanged;

	protected virtual void OnColorChanged (EventArgs e) => ColorChanged?.Invoke(this, e);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space))
			OnColorChanged(EventArgs.Empty);
	}
}
