using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InterSceneData;

/**
	 Controls the sliders in the settings page
**/
public class SliderBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Sets up the preset values on the sliders
		if (transform.name == "Slider SensitivityX") {
			GetComponent<Slider>().value = Data.sensitivityX/14F;
		}		

		if (transform.name == "Slider SensitivityY") {
			GetComponent<Slider>().value = Data.sensitivityY/14F;
		}

		if (transform.name == "Slider Sound") {
			GetComponent<Slider>().value = Data.volume;
		}

		if (transform.name == "Slider SFX") {
			GetComponent<Slider>().value = Data.sfx;
		}
	}

	// Update is called once per frame
	void Update () {
		if (transform.name == "Slider SensitivityX") {
			Data.sensitivityX = 10.0F * GetComponent<Slider>().value;
		}		

		if (transform.name == "Slider SensitivityY") {
			Data.sensitivityY = 10.0F * GetComponent<Slider>().value;
		}

		if (transform.name == "Slider Sound") {
			Data.volume = 1.0F* GetComponent<Slider>().value;
		}

		if (transform.name == "Slider SFX") {
			Data.sfx = 1.0F*GetComponent<Slider>().value;
		}
		
	}
}
