using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMBehavior : MonoBehaviour {

	public int startingPitch;
	public int timeToDecrease;

	private int timeConstant = 100;
	AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.pitch = startingPitch;
	}

	void Update()
	{
		if (audioSource.pitch <= 3 && audioSource.pitch >= 1.5) {
			audioSource.pitch -= Time.deltaTime / timeConstant * startingPitch / timeToDecrease;

			if (audioSource.volume < 1 && audioSource.pitch > 2) {
				audioSource.volume += 0.008F;
			}
			if (audioSource.pitch < 1.55F) {
				audioSource.volume -= 0.005F;
			}
		}
		

		if (audioSource.pitch <= 1.5) {
			audioSource.pitch = 3;
		}
	}

}
