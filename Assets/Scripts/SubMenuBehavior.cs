using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class SubMenuBehavior : MonoBehaviour, IPointerDownHandler  {

	public string sceneName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerDown(PointerEventData eventData) {
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
		SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
	}
}
