using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterSceneData;

public class PortalController : MonoBehaviour 
{

	public float RenderDistance;
	public GameObject exit;

	public AudioClip portal;
	public AudioSource audioPortal;

	public bool connecting = false;
	
	public int destination;	

	private GameObject player;
	
	private Camera cameraView;
	private Camera exitView;
	private RenderTexture view;
	
	private bool turnedOn;


	void Awake() 
	{
		player = GameObject.Find("Player");
		view = new RenderTexture(512, 512, 24, RenderTextureFormat.ARGB32);
		
		if (transform.childCount > 0) 
		{
			cameraView = transform.GetChild(0).gameObject.GetComponent<Camera>();
		}
	}


	// Use this for initialization
	void Start () 
	{
		audioPortal = GameObject.Find ("SFX").GetComponent<AudioSource>();
		turnedOn = true;
		if (exit == null) 
		{
			return;
		}
		
		exitView = exit.GetComponent<PortalController>().cameraView;
		
		if (cameraView != null) 
		{
			cameraView.targetTexture = view;
			view.Create();

			Renderer rend = GetComponent<Renderer>();
			rend.material.mainTexture = exit.GetComponent<PortalController>().texture();
		}
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (exit == null || transform.childCount == 0) 
		{
			return;
		}
		if ((player.transform.position - cameraView.transform.position).magnitude < RenderDistance) 
		{
			exitView.enabled = true;
			rotateCamera();
			
		} else {
			exitView.enabled = false;
		}

	}
	 
	private void rotateCamera()
	{
		
		Vector3 pos = transform.InverseTransformPoint(Camera.main.transform.position);
		float x = Cap(pos.x, 50);
		float y = Cap(pos.y);
		float z = Cap(pos.z);
		exitView.transform.localPosition = new Vector3(x, y, z);

	}
	private float Cap(float a, float tolerance = 1f) {
		if (a < -tolerance) {
			a = -tolerance;
		} else if (a > tolerance) {
			a = tolerance;
		}
		return a;
	}

	void OnTriggerEnter(Collider other)
	{
		if (exit == null) {
			return;
		}
		
		if (turnedOn) 
		{
			
			if (other.gameObject.tag != "Player") {
				other.gameObject.GetComponent<Collider>().isTrigger = true;
				return;
			}

			if (connecting) {
				Data.currentLevel = destination;
				player.GetComponent<PlayerController>().updateInitialLocation(); 
			}

			var deltaY = other.transform.position.y - transform.position.y;
			var deltaZ = other.transform.position.z - transform.position.z;

			float x = exit.transform.position.x;
			float y = exit.transform.position.y;
			float z = exit.transform.position.z;
		
			Vector3 position = new Vector3(x, y + deltaY, z + deltaZ);
			other.gameObject.transform.position = position;
			audioPortal.PlayOneShot(portal, Data.sfx);
		
			
			other.gameObject.GetComponent<PlayerController>().teleportItem(position);		
		
			
			Camera.main.GetComponent<CameraBehavior>().rotate((-transform.rotation.eulerAngles+exit.transform.rotation.eulerAngles) + new Vector3(0,180,0));
			
			exit.GetComponent<PortalController>().turnedOn = false;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag != "Player") {
			other.gameObject.GetComponent<Collider>().isTrigger = false;
			return;
		}
		turnedOn = true;	
	}
	
	RenderTexture texture() 
	{
		return view;
	}
}
