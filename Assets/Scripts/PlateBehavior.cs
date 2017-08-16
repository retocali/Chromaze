using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colors;
using InterSceneData;

public class PlateBehavior : MonoBehaviour {

	private bool activated;

	private bool mixedColor;
	private Dictionary<ColorManager.ColorName, bool> colorsToMix = new Dictionary<ColorManager.ColorName, bool>();

	private static Dictionary<ColorManager.ColorName, Material> deactivatedMaterials = new Dictionary<ColorManager.ColorName, Material>();
	private static Dictionary<ColorManager.ColorName, Material> activatedMaterials = new Dictionary<ColorManager.ColorName, Material>();
	
	public Material materialInactive;
	public Material materialActive;


	private Material activatedColor;
	private Material deactivatedColor;
	private Material currentColor;


	public ColorManager.ColorName colorName;
	public AudioClip panel;

	// Use this for initialization
	void Start () {
		activated = false;
		
		if (!activatedMaterials.ContainsKey(colorName)) {
			Color color = ColorManager.findColor(colorName);

			
			deactivatedColor = new Material(materialInactive);
			activatedColor = new Material(materialActive);

			deactivatedColor.color = color;
			activatedColor.color = color;
			

			deactivatedMaterials[colorName] = deactivatedColor;
			activatedMaterials[colorName] = activatedColor;
			
		} else {
			deactivatedColor = deactivatedMaterials[colorName];
			activatedColor = activatedMaterials[colorName];
		}
		this.GetComponent<Renderer>().material = deactivatedColor;
		
		
		currentColor = deactivatedColor;
		mixedColor = ColorManager.isMixed(colorName);
		
		if (mixedColor) 
		{
			foreach (ColorManager.ColorName colorName in ColorManager.mixture(colorName))
			{
				colorsToMix.Add(colorName, false);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {	
		if (activated && currentColor != activatedColor) {
			this.GetComponent<Renderer>().material = activatedColor;
			 currentColor = activatedColor;
			
			AudioSource.PlayClipAtPoint(panel, transform.position, 0.5F*Data.sfx);
		}
		else if (!activated &&  currentColor != deactivatedColor) {
			this.GetComponent<Renderer>().material = deactivatedColor;
			 currentColor = deactivatedColor;
		}
	}

	void OnCollisionEnter(Collision other)	
	{
		onCollision(other);
	}

	void OnCollisionStay(Collision other)
	{
		onCollision(other);
	}

	private void onCollision(Collision other) {
		
		if (other.gameObject.tag == "Player")
		{
			activated = true;
		}
		else if (other.gameObject.tag != "Box") 
		{
			return;
		} 
		else if (other.gameObject.GetComponent<BoxController>().colorName == colorName) 
		{
			activated = true;
		} 
		else if (mixedColor) 
		{
			if (colorsToMix.ContainsKey(other.gameObject.GetComponent<BoxController>().colorName)) 
			{
				colorsToMix[other.gameObject.GetComponent<BoxController>().colorName] = true;
			}
			
			foreach (bool present in colorsToMix.Values)
			{
				if (!present) {
					return;
				}
			}
			activated = true;
		}
	}
	
	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag == "Wall") {
			return;
		}
		if (mixedColor && other.gameObject.tag != "Player") {
			if (other.gameObject.GetComponent<BoxController>() != null) {
				if (colorsToMix.ContainsKey(other.gameObject.GetComponent<BoxController>().colorName)) {
					colorsToMix[other.gameObject.GetComponent<BoxController>().colorName] = false;
				}
			}
		}
		activated = false;
	}

	public bool isActivated() {
		return activated;
	}
}
