using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMBehavior : MonoBehaviour {

	public int startingPitch;
	public float maxPitch;
	public float minPitch;
	public int timeToDecrease;

	private int timeConstant = 100;
	private float delta = -1;
	AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.pitch = startingPitch;
	}

	void FixedUpdate()
	{
		if (audioSource.pitch <= maxPitch && audioSource.pitch >= minPitch) {
			audioSource.pitch += delta*Time.deltaTime / timeConstant * startingPitch / timeToDecrease;

			if (audioSource.volume < 1 && audioSource.pitch > maxPitch) {
				audioSource.volume += 0.008F;
			}
			if (audioSource.pitch < minPitch) {
				audioSource.volume -= 0.005F;
			}
		} else {
			delta *= -1;
			audioSource.pitch += delta*Time.deltaTime / timeConstant * startingPitch / timeToDecrease;
		}
	}

}
