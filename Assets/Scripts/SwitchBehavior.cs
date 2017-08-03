using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehavior : MonoBehaviour {

	public bool present;
	public GameObject plate;

	private bool previous;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer>().enabled = present;
		gameObject.GetComponent<Collider>().enabled = present;
		previous = plate.GetComponent<PlateBehavior>().isActivated();
	}
	
	// Update is called once per frame
	void Update () {
		if (previous != plate.GetComponent<PlateBehavior>().isActivated()) {
			switchState();
		}
		previous = plate.GetComponent<PlateBehavior>().isActivated();
	}	

	void switchState() {
		gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
		gameObject.GetComponent<Collider>().enabled = !gameObject.GetComponent<Collider>().enabled;
	}
	

}
