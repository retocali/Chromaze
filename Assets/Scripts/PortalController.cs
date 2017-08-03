using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

	public float RenderDistance;
	public GameObject exit;
	private GameObject player;
	private Camera cameraView;
	private Camera exitView;
	private RenderTexture view;
	private bool turnedOn;
	

	void Awake() {
		player = GameObject.Find("Player");
		view = new RenderTexture(512, 512, 24, RenderTextureFormat.ARGB32);
		if (transform.childCount > 0) {
			cameraView = transform.GetChild(0).gameObject.GetComponent<Camera>();
		}
	}


	// Use this for initialization
	void Start () {
		turnedOn = true;
		if (exit == null) {
			return;
		}
		
		exitView = exit.GetComponent<PortalController>().cameraView;
		if (exitView != null) {
			exitView.targetTexture = view;
			view.Create();
			Renderer rend = GetComponent<Renderer>();
			rend.material.mainTexture = view;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (exit == null || transform.childCount == 0) {
			return;
		}
		if ((player.transform.position - cameraView.transform.position).magnitude < RenderDistance) {
			exitView.transform.rotation = Quaternion.LookRotation(cameraView.transform.position - player.transform.position);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (turnedOn) {
			var deltaX = other.transform.position.x - transform.position.x;
			var deltaY = other.transform.position.y - transform.position.y;
			var deltaZ = other.transform.position.z - transform.position.z;



			float x = exit.transform.position.x;
			float y = exit.transform.position.y;
			float z = exit.transform.position.z;

			other.gameObject.transform.position = new Vector3(x + deltaX, y + deltaY, z + deltaZ);
			exit.GetComponent<PortalController>().turnedOn = false;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		turnedOn = true;	
	}
}
