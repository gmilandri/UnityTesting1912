using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	private EventManager _eventManager;

	// Use this for initialization
	void Start () {
		_eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
		_eventManager.ColorChanged += _testScript_ColorChanged;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void _testScript_ColorChanged (object sender, EventArgs e)
	{
		GetComponent<Renderer>().material.color = Color.Lerp(gameObject.GetComponent<Renderer>().material.color, Color.red, 0.05f);
	}

}
