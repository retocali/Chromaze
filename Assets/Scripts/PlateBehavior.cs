using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colors;

public class PlateBehavior : MonoBehaviour {

	private bool activated;
	private Color color;

	private bool mixedColor;
	private Dictionary<ColorManager.ColorName, bool> colorsToMix = new Dictionary<ColorManager.ColorName, bool>();
	private Color activatedColor;
	private Color deactivatedColor;
	public ColorManager.ColorName colorName;


	// Use this for initialization
	void Start () {
		activated = false;
		activatedColor = ColorManager.findColor(colorName);
		deactivatedColor = new Color(0.5f*activatedColor.r, 0.5f*activatedColor.g, 0.5f*activatedColor.b, 0f);
		this.GetComponent<Renderer>().material.color = deactivatedColor;
		color = deactivatedColor;
		mixedColor = ColorManager.isMixed(colorName);
		if (mixedColor) {
			foreach (ColorManager.ColorName color in ColorManager.mixture(colorName))
			{
				colorsToMix.Add(color, false);
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {	
		if (activated && color != activatedColor) {
			this.GetComponent<Renderer>().material.color = activatedColor;
			color = activatedColor;
		}
		else if (!activated && color != deactivatedColor) {
			this.GetComponent<Renderer>().material.color = deactivatedColor;
			color = deactivatedColor;
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
		if (other.gameObject.tag == "Wall") {
			return;
		}
		if (other.gameObject.tag == "Player") {
			activated = true;
		} else if (other.gameObject.GetComponent<BoxController>().colorName == colorName) {
			activated = true;
		} else if (mixedColor) {
			if (colorsToMix.ContainsKey(other.gameObject.GetComponent<BoxController>().colorName)) {
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
			if (colorsToMix.ContainsKey(other.gameObject.GetComponent<BoxController>().colorName)) {
				colorsToMix[other.gameObject.GetComponent<BoxController>().colorName] = false;
			}
		}
		activated = false;
	}

	public bool isActivated() {
		return activated;
	}
}
