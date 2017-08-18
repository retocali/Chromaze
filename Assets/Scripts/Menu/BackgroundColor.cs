using UnityEngine;
using System.Collections;

/**
	Controls the changing background color on the menu screen
*/
public class BackgroundColor : MonoBehaviour
{
	
	public Color color1 = Color.red;
	public Color color2 = Color.blue;
	public float duration = 5.0F;

	Camera cam;

	void Start()
	{
		cam = GetComponent<Camera>();
		cam.clearFlags = CameraClearFlags.SolidColor;
	}

	void Update()
	{
		float t = Mathf.PingPong (Time.time, duration) / duration;
		cam.backgroundColor = Color.Lerp (color1, color2, t);
	}
}