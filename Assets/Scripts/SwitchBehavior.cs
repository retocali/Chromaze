using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehavior : MonoBehaviour {

	public bool present;
	public GameObject plate;

	private bool previous;
	private Renderer rend;
	private Collider cldr;
	private Rigidbody rb;
	private PlateBehavior plateScript;

	// Use this for initialization
	void Start () {
		if (gameObject.GetComponent<Renderer>() != null) {
			rend = gameObject.GetComponent<Renderer>();
			rend.enabled = present;
		}

		if (gameObject.GetComponent<Collider>() != null) {
			cldr = gameObject.GetComponent<Collider>();
			cldr.enabled = present;
		}

		if (gameObject.GetComponent<Rigidbody>() != null) {
			rb = gameObject.GetComponent<Rigidbody>();
			if (present) {
				rb.constraints = RigidbodyConstraints.FreezeRotation;
		
			} else {
				rb.constraints = RigidbodyConstraints.FreezeAll;
			}
			
		}
		if (plate == null) {
			Debug.Log("Error:" + name + " has no plate.");
			return;
		}
		plateScript = plate.GetComponent<PlateBehavior>();
		previous = plateScript.isActivated();
	}
	
	// Update is called once per frame
	void Update () {
		if (plate == null) {
			Debug.Log("Error:" + name + " has no plate.");
			return;
		}
		if (previous != plateScript.isActivated()) {
			switchState();
		}
		previous = plateScript.isActivated();
	}	

	void switchState() {
		if (rend != null) {
			rend.enabled = !rend.enabled;
		}

		if (cldr != null) {
			cldr.enabled = !cldr.enabled;
		}
		if (rb != null) {

			if (rend.enabled) {
				rb.constraints = RigidbodyConstraints.FreezeRotation;
			
			} else {
				rb.constraints = RigidbodyConstraints.FreezeAll;
			}
			
		}
	}
	

}
