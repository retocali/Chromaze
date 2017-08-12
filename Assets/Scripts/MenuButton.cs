using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

	public Text theText;
	public Button yourButton;

	void Start(){
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		theText.color = Color.red; //Or however you do your color
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		theText.color = Color.white; //Or however you do your color
	}

	public void OnPointerDown(PointerEventData eventData) {
		Debug.Log (transform.name);
		if (transform.name == "Start") {
			SceneManager.LoadScene ("Main Game");
		}
		if (transform.name == "How To Play") {
			SceneManager.LoadScene ("Intro");
		}
	}
}