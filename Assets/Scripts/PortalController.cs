using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour 
{

	public float RenderDistance;
	public GameObject exit;

	public AudioClip portal;
	public AudioSource audioPortal;

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
	void Update () {
		if (exit == null || transform.childCount == 0) 
		{
			return;
		}
		if ((player.transform.position - cameraView.transform.position).magnitude < RenderDistance) 
		{
			exitView.enabled = true;
			exitView.transform.rotation = Quaternion.LookRotation(- cameraView.transform.position + player.transform.position);
			exitView.transform.Rotate(transform.rotation.eulerAngles-exit.transform.rotation.eulerAngles);
		} else {
			exitView.enabled = false;
		}
		Debug.Log(GetComponent<Collider>().isTrigger);
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Teleporting");
		if (exit == null) {
			return;
		}
		
		if (turnedOn) 
		{
			
			if (other.gameObject.tag != "Player") {
				other.gameObject.GetComponent<Collider>().isTrigger = true;
				return;
			}

			var deltaY = other.transform.position.y - transform.position.y;
			var deltaZ = other.transform.position.z - transform.position.z;

			float x = exit.transform.position.x;
			float y = exit.transform.position.y;
			float z = exit.transform.position.z;
		
			Vector3 position = new Vector3(x, y + deltaY, z + deltaZ);
			other.gameObject.transform.position = position;
			audioPortal.PlayOneShot(portal, 2.2F);
		
			
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
