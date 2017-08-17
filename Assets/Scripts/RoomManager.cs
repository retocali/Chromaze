using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InterSceneData;

public class RoomManager : MonoBehaviour {

	public GameObject player;
	public Text text;
	public Image textBackground;

	public GameObject[] rooms;
	public Vector3[] roomInitialLocations;
	public string[] roomQuotes;

	private bool firstTime = true;	
	private int playerPosition;

	// Use this for initialization
	void Start () {
		Screen.fullScreen = true;
		Cursor.lockState = CursorLockMode.Locked;
		Debug.Assert(rooms.Length == roomInitialLocations.Length, "Error: Room Manager was not set up properly");
		text.enabled = false;
		textBackground.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		//if (Application.isEditor) {
			// For each of the following keys go to the corresponding room
			for (int i = 1; i < 9; i++)
			{
				if (Input.GetKeyDown(""+i)) {
					goToRoom(i);
					Data.currentLevel = i;
					return;
				}
			}
		//}
		if (Input.GetKeyDown(KeyCode.H)) {
			giveHint();
		}
		
		// Resets the game
		if (Input.GetKeyDown(KeyCode.J)) {
			resets();
			SceneManager.LoadScene("Main Game");
		}

		// Goes to the main menu
		if ((Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) 
				&& SceneManager.sceneCount < 2) {
			StartCoroutine(setScene("Menu"));
			Cursor.lockState = CursorLockMode.None;
		}

		// Toggles Fullscreen
		if (Input.GetKeyDown(KeyCode.F)) {
			Screen.fullScreen = !Screen.fullScreen;
			switchCursor();
		}
	}

	private void switchCursor() {
		if (Cursor.lockState == CursorLockMode.Locked) {
			Cursor.lockState = CursorLockMode.None;
		} else {
			Cursor.lockState = CursorLockMode.Locked;
		}
	}


	// Moves the player to a given room
	public void goToRoom(int roomNumber) {
		if (roomNumber-1 < roomInitialLocations.Length) {
			player.transform.position = roomInitialLocations[roomNumber-1];
			player.GetComponent<PlayerController>().updateInitialLocation(); 
			giveHint(roomNumber-1);
			
		} else {
			Debug.LogError("Error: Not a valid room number");
		}
	}
	
	// Resets the whole world and sets the player at the start of the given room
	void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (!firstTime) {
			return;
		}
		goToRoom(Data.currentLevel);
		firstTime = false;
	}
	// called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
	
	void resets() {
		Physics.gravity = new Vector3(0, - Mathf.Abs(Physics.gravity.y), 0);	
	}

	IEnumerator setScene(string scene) {
		AsyncOperation async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
		while (!async.isDone) {
			Debug.Log("Progress"+async.progress);
			yield return new WaitForEndOfFrame();
		}	
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
	}

	void giveHint(int roomNumber) {
		transform.position = player.transform.position;
		text.text = roomQuotes[roomNumber];
		text.enabled = true;
		textBackground.enabled = true;
		StartCoroutine(disappear());
	}
	void giveHint() {
		transform.position = player.transform.position;
		text.text = roomQuotes[Data.currentLevel-1];
		text.enabled = true;
		textBackground.enabled = true;
		StartCoroutine(disappear());
	}
	IEnumerator disappear() {
		yield return new WaitForSeconds(10);
		text.enabled = false;
		textBackground.enabled = false;
	}
}
