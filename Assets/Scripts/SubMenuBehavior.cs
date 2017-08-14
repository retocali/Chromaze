using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubMenuBehavior : MonoBehaviour {

	public string sceneName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
			SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
		}
	}
}
