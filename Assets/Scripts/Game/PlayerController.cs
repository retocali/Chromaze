using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InterSceneData;

public class PlayerController : MonoBehaviour {

	// Player movement constants
	public float speed;
	public float jumpHeight;
	public float minHeight = -50f;
	
	
	public float scaleFactor = 0.3f;
	public int boxLayer = 1 << 9;
	
	public GameObject bubblePrefab;

	public Camera mainCamera;
	
	public AudioClip bubble;
	public AudioClip jump;
	
	public AudioSource audioBubble;
	public AudioSource audioJump;
	
	public GameObject roomManager;
	
	// Internal variables
	public float pickUpDistance = 3;
	private Rigidbody rb;
	
	private bool onGround;
	private bool holding;
	
	// Item code
	private GameObject item;
	private GameObject currentBubble;
	private float minimum_distance = 0.1f;
	private float maximum_distance = 5f;

	private Vector3 initialPosition;
	

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		if (roomManager == null) {
			roomManager = GameObject.Find("RoomManager");
		}
		onGround = false;
		holding = false;
		initialPosition = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (SceneManager.GetActiveScene().name != "Main Game") {
			return; // Make the player inactive if the scene does not match
		}

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
			makeJump();
		}

		// Item holding
		if (Input.GetKeyDown(KeyCode.E)) {
			if (!holding) {
				pickUp();
			} else {
				drop();
			}
		}
		moveItem();
		
		checkOnGround();

		checkRespawn();
	}

	// Extra update to solve jitteriness
	void LateUpdate()
	{
		mainCamera.transform.position = transform.position - Physics.gravity.normalized/2;	
		if (holding) {
			currentBubble.transform.position = item.GetComponent<Rigidbody>().transform.position;
		}
	}


	// Item code
	void pickUp() {
		RaycastHit hit;
		// Do a raycast to find the box that the player is picking up
		if (Physics.BoxCast(transform.position, 0.2F*(Vector3.one-Vector3.forward), 
				pickUpDistance * mainCamera.transform.TransformVector(Vector3.forward),
				out hit, mainCamera.transform.rotation, 1, boxLayer)) {
			
			item = hit.collider.gameObject;
			
			// Break if item is not a box or cannot be pickedUp
			if (!(item.tag == "Box")) {
				return;	
			} else if (!item.GetComponent<BoxController>().isPickable()) {
				return;
			}
 
			item.GetComponent<BoxController>().pickUp();
			holding = true;

			// Make it float and scale it
			item.GetComponent<Rigidbody>().useGravity = false;
			item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			item.transform.localScale *= scaleFactor;


			makeBubble();
			audioBubble.PlayOneShot(bubble, Data.sfx);

			// Make it so the player cannot collide with it
			Physics.IgnoreCollision(item.GetComponent<Collider>(), GetComponent<Collider>(), true);
		}
	
	}
	void drop() {
		if (holding && item.GetComponent<BoxController>().isDroppable()) {

			// Resize it to normal and make it obey physics
			item.GetComponent<Collider>().isTrigger = false;
			item.GetComponent<Rigidbody>().useGravity = true;

			item.transform.localScale /= scaleFactor;
			Physics.IgnoreCollision(item.GetComponent<Collider>(), GetComponent<Collider>(), false);

			// Pop bubble
			Destroy(currentBubble);
			audioBubble.PlayOneShot(bubble, 0.9F);

			// Drop it
			item.GetComponent<BoxController>().drop();

			item = null;
			holding = false;
			
		}
		
	}
	void moveItem() {
		if (holding) {
			Vector3 heldPosition = transform.position + mainCamera.transform.TransformVector(transform.forward*1.5f);
			Vector3 distance = heldPosition - item.GetComponent<Rigidbody>().position;

			// If too far away drop it
			if (distance.magnitude > maximum_distance) 
			{
				drop();
			} 
			// If not within position move it
			else if (distance.magnitude > minimum_distance) 
			{
				item.GetComponent<Rigidbody>().velocity = (speed*speed/2 * distance);
			// Make its velocity zero
			} else {
				item.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
		}
	}
	void makeBubble(){
		currentBubble = Instantiate(bubblePrefab);
	}
	public bool isHolding() {
		return holding;
	}
	public void teleportItem(Vector3 position) {
		if (holding) {
			item.GetComponent<Collider>().isTrigger = true;
			item.GetComponent<Rigidbody>().position = position + mainCamera.transform.TransformVector(transform.forward*1.5f);
			item.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	// Movement Code
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

	// Jumping Code/Bounds
	void makeJump() {
		audioJump.clip = jump;
		audioJump.volume = Data.sfx;
		audioJump.Play();
		rb.velocity	 = jumpHeight * - Physics.gravity.normalized;
	}
	void checkOnGround() {
		// Checking for Jumps
		if (Physics.Raycast(transform.position, Physics.gravity.normalized, transform.lossyScale.y)) {
			onGround = true;
		} else {
			onGround = false;
		}
	} 
	void checkRespawn() {
		// Respawn if the player falls out of bounds
		if (transform.position.y <= minHeight) {
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			transform.position = initialPosition;
		}
	}
	public void updateInitialLocation() {
		initialPosition = roomManager.GetComponent<RoomManager>().roomInitialLocations[Data.currentLevel-1];
	}
}

	