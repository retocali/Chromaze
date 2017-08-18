using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
	Changes from one of the sub-menus to the menu
**/
public class SubMenuBehavior : MonoBehaviour, IPointerDownHandler  {

	public string sceneName;

	public void OnPointerDown(PointerEventData eventData) {
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
		SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
	}


}
