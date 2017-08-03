using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colors;

public class BoxController : MonoBehaviour {

	

	private bool droppable = true;
	public ColorManager.ColorName colorName;

	// Use this for initialization
	void Start () {
		Color color = ColorManager.findColor(colorName);
		this.GetComponent<Renderer>().material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool isDroppable() {
		return droppable;
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
