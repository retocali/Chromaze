using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colors;

public class GravityPanelController : MonoBehaviour {

	public ColorManager.ColorName colorName;

	private bool activated;
	private Color color;
	private Color activatedColor;
	private Color deactivatedColor;
    private bool mixedColor;
    private GameObject box;
	private Dictionary<ColorManager.ColorName, bool> colorsToMix = new Dictionary<ColorManager.ColorName, bool>();

	// Use this for initialization
	void Start () {
		activated = false;
		activatedColor = ColorManager.findColor(colorName);
		deactivatedColor = new Color(0.5f*activatedColor.r, 0.5f*activatedColor.b, 0.5f*activatedColor.g, 0f);
		color = deactivatedColor;
		this.GetComponent<Renderer>().material.color = deactivatedColor;
		mixedColor = ColorManager.isMixed(colorName);
		if (mixedColor) {
			foreach (ColorManager.ColorName color in ColorManager.mixture(colorName))
			{
				colorsToMix.Add(color, false);
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {	
		if (activated && color != activatedColor) {
            this.GetComponent<Renderer>().material.color = activatedColor;
            color = activatedColor;
            switchGravity();
		}
		else if (!activated && color != deactivatedColor) {
			this.GetComponent<Renderer>().material.color = deactivatedColor;
			color = deactivatedColor;
            switchGravity();
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
		if (other.gameObject.tag != "Box") {
			return;
		} 
        box = other.gameObject;
        if (other.gameObject.GetComponent<BoxController>().colorName == colorName) {
			activated = true;
            box.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            box.transform.rotation = Quaternion.identity;
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
		if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Player") {
			return;
		}
        if (mixedColor) {
			if (colorsToMix.ContainsKey(other.gameObject.GetComponent<BoxController>().colorName)) {
				colorsToMix[other.gameObject.GetComponent<BoxController>().colorName] = false;
			}
		}
		activated = false;
	}

	void switchGravity() {
        Physics.gravity *= -1;
        
        // foreach (GameObject affectedObject in affectedObjects)
		// {

            
        //     // affectedObject.transform.Rotate(new Vector3(0, 0, 180f));
		// }
        // // box.transform.rotation = Quaternion.identity;
        // // transform.rotation = Quaternion.identity;
	}
}
