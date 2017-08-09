using UnityEngine;
using System.Collections;

public class IntroCamera : MonoBehaviour {
	public Transform Cube;

	// Update is called once per frame
	void Update () {
		transform.LookAt(Cube);
	}
}