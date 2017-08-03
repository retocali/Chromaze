using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Player movement constants
	public float speed;
	public float jumpHeight;
	public float xCameraThreshold;
	public float yCameraThreshold;
	public float zCameraThreshold;


	public Camera mainCamera;
	
	// Internal variables
	private Rigidbody rb;
	private bool onGround;
	private bool holding;
	private GameObject item;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		onGround = false;
		holding = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
		// Moving
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			moveRight(Time.deltaTime);
		} 
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			moveLeft(Time.deltaTime);
		} 
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			moveUp(Time.deltaTime);
		} 
		else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			moveDown(Time.deltaTime);
		} 

		// Jumping
		if (Input.GetKey(KeyCode.Space) && onGround) {
			rb.velocity = jumpHeight * - Physics.gravity.normalized;
		}

		// Camera Fix for Jittery collisions
		float y = transform.position.y;
		float x = transform.position.x;
		float z = transform.position.z;

		if ( Math.Abs(mainCamera.transform.position.x - x) > xCameraThreshold || 
			 Math.Abs(mainCamera.transform.position.y - y) > yCameraThreshold || 
			 Math.Abs(mainCamera.transform.position.z - z) > zCameraThreshold
		   ) {
			mainCamera.transform.position = new Vector3(x, y, z);	
		}

		if (holding) {
			item.GetComponent<Rigidbody>().position = new Vector3(x, y, z) + mainCamera.transform.TransformVector(transform.forward * 2);
		}

		// Item holding
		if (Input.GetKeyDown(KeyCode.E)) {
			if (!holding && item != null) {
				pickUp();
			} else {
				drop();
			}
		}
		if (Physics.Raycast(transform.position, Physics.gravity.normalized, transform.lossyScale.y)) {
			onGround = true;
		} else {
			onGround = false;
		}
			
	}
	void OnCollisionEnter(Collision collision) {
		if (!holding) {
			item = collision.gameObject;
		}
	}

	void OnCollisionExit(Collision collision) {
		if (!holding) {
			item = null;
		}
	}

	void pickUp() {
		if (item.tag == "Box") {
			holding = true;
			item.GetComponent<Collider>().isTrigger = true;
			item.GetComponent<Rigidbody>().useGravity = false;
			item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			item.GetComponent<Rigidbody>().position = transform.position + mainCamera.transform.TransformVector(transform.forward * 2);
			
		}
	
	}
	
	void drop() {
		if (holding && item.GetComponent<BoxController>().isDroppable()) {
			item.GetComponent<Collider>().isTrigger = false;
			item.GetComponent<Rigidbody>().useGravity = true;
			item = null;
			holding = false;
		}
		
	}

	void moveRight(float time) {
		Vector3 movement = mainCamera.transform.TransformVector(speed * transform.right * time);
		rb.position += new Vector3(movement.x, 0, movement.z);
	}
	void moveLeft(float time) {
		Vector3 movement = mainCamera.transform.TransformVector(speed * transform.right * time);
		rb.position -= new Vector3(movement.x, 0, movement.z);
	}
	void moveUp(float time) {
		Vector3 movement = mainCamera.transform.TransformVector(speed * transform.forward * time);
		rb.position += new Vector3(movement.x, 0, movement.z);
	}
	void moveDown(float time) {
		Vector3 movement = mainCamera.transform.TransformVector(speed * transform.forward * time);
		rb.position -= new Vector3(movement.x, 0, movement.z);
	}
		


}



