using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colors;
using InterSceneData;

public class BoxController : MonoBehaviour {

	public float minHeight = -50f;
	
	public ColorManager.ColorName colorName;
	public static Material baseMaterial;

	public AudioClip respawn;
	public AudioSource audioRespawn;

	private static Dictionary<ColorManager.ColorName, Material> materials = new Dictionary<ColorManager.ColorName, Material>();
	
	private bool droppable = false;
	private bool pickable = true;	

	private Vector3 initialPosition;

	// Use this for initialization
	void Start () 
	{
		audioRespawn = GameObject.Find ("SFX").GetComponent<AudioSource>();
		Color color = ColorManager.findColor(colorName);
		
		if (!materials.ContainsKey(colorName)) 
		{
			this.GetComponent<Renderer>().material.color = color;
			materials[colorName] = this.GetComponent<Renderer>().material;
		} 
		else 
		{
			this.GetComponent<Renderer>().material = materials[colorName];
		}
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (transform.position.y <= minHeight) {
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			transform.position = initialPosition;
			audioRespawn.PlayOneShot(respawn, Data.sfx);
		}
	}

	public bool isDroppable() 
	{
		return droppable;
	}

	public bool isPickable() 
	{
		return pickable;
	}

	public void pickUp() {
		pickable = false;
		StartCoroutine(pickuping());
	}

	public void drop() {
		StartCoroutine(dropping());
	}

	IEnumerator dropping()
	{
		yield return new WaitForSeconds(0.5f);
		pickable = true;
	}

	IEnumerator pickuping()
	{
		yield return new WaitForSeconds(0.5f);
		droppable = true;
	}

	void OnTriggerEnter(Collider other)
	{
		droppable = false;
	}

	void OnTriggerStay(Collider other)
	{
		droppable = false;
	}


	void OnTriggerExit(Collider other)
	{
		droppable = true;
	}

}
