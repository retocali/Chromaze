using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colors;

public class GravityPanelController : MonoBehaviour 
{

	public ColorManager.ColorName colorName;

	private Color color;
	private Color activatedColor;
	private Color deactivatedColor;
    
	private bool activated;
    private bool switching;


	// Use this for initialization
	void Start () 
	{
		activated = false;
	
		activatedColor = ColorManager.findColor(colorName);
		deactivatedColor = new Color(0.5f*activatedColor.r, 0.5f*activatedColor.b, 0.5f*activatedColor.g, 0f);
	
		color = deactivatedColor;
		this.GetComponent<Renderer>().material.color = deactivatedColor;

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
	}

	IEnumerator switchGravity() 
	{
		// Switches gravity
		switching = true;
        Physics.gravity *= -1;

		// Waits
		yield return new WaitForSeconds(0.5f);

		// Begins rotating camera
		Camera.main.GetComponent<CameraBehavior>().slowRotate(new Vector3(0,0,180), 20);
		switching = false;
	}
}
