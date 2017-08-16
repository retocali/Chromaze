using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colors;

public class GravityPanelController : MonoBehaviour 
{

	public ColorManager.ColorName colorName;

	private bool activated;

	private Color color;
	private Color activatedColor;
	private Color deactivatedColor;
    
	private GameObject box;
    
	private bool mixedColor;
	private Dictionary<ColorManager.ColorName, bool> colorsToMix = new Dictionary<ColorManager.ColorName, bool>();
	private bool switching;

	// Use this for initialization
	void Start () 
	{
		activated = false;
	
		activatedColor = ColorManager.findColor(colorName);
		deactivatedColor = new Color(0.5f*activatedColor.r, 0.5f*activatedColor.b, 0.5f*activatedColor.g, 0f);
	
		color = deactivatedColor;
		this.GetComponent<Renderer>().material.color = deactivatedColor;
		mixedColor = ColorManager.isMixed(colorName);
	
		if (mixedColor) 
		{
			foreach (ColorManager.ColorName color in ColorManager.mixture(colorName))
			{
				colorsToMix.Add(color, false);
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{	
		if (activated && color != activatedColor) 
		{
            this.GetComponent<Renderer>().material.color = activatedColor;
            color = activatedColor;
            StartCoroutine(switchGravity());
		}
		else if (!activated && color != deactivatedColor) 
		{
			this.GetComponent<Renderer>().material.color = deactivatedColor;
			color = deactivatedColor;
            StartCoroutine(switchGravity());
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

	private void onCollision(Collision other) 
	{
		if (other.gameObject.tag != "Player") 
		{
			return;
		} 
		if (!switching) {
			StartCoroutine(switchGravity());
		}
		Debug.Log("Did it!");
        // box = other.gameObject;
        // if (other.gameObject.GetComponent<BoxController>().colorName == colorName) 
		// {
		// 	activated = true;
            
		// 	box.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //     box.transform.rotation = Quaternion.identity;
		// 	box.transform.position = transform.position-Physics.gravity.normalized.y*box.transform.lossyScale.y/2*Vector3.up;

		// } 
		// else if (mixedColor) {
			
		// 	if (colorsToMix.ContainsKey(other.gameObject.GetComponent<BoxController>().colorName)) 
		// 	{
		// 		colorsToMix[other.gameObject.GetComponent<BoxController>().colorName] = true;
		// 	}
			
		// 	foreach (bool present in colorsToMix.Values)
		// 	{
		// 		if (!present) 
		// 		{
		// 			return;
		// 		}
		// 	}
		// 	activated = true;
		// }
	}

	
	void OnCollisionExit(Collision other)
	{
		// if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Player") 
		// {
		// 	return;
		// }
        // if (mixedColor) 
		// {
		// 	if (colorsToMix.ContainsKey(other.gameObject.GetComponent<BoxController>().colorName)) 
		// 	{
		// 		colorsToMix[other.gameObject.GetComponent<BoxController>().colorName] = false;
		// 	}
		// }
		// activated = false;
	}

	IEnumerator switchGravity() 
	{
		switching = true;
        Physics.gravity *= -1;
		yield return new WaitForSeconds(0.5f);
		Camera.main.GetComponent<CameraBehavior>().slowRotate(new Vector3(0,0,180), 20);
		switching = false;
	}
}
