using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InterSceneData;

public class RoomManager : MonoBehaviour {

	public GameObject player;
	public GameObject[] rooms;
	public Vector3[] roomInitialLocations;

	private int playerPosition;

	// Use this for initialization
	void Start () {
		Debug.Assert(rooms.Length == roomInitialLocations.Length, "Error: Room Manager was not set up properly");
	
	}

	// Update is called once per frame
	void Update () {
		if (!Application.isEditor) {
			// Do not take keyboard input in the actual build or if not setup
			return;
		}

		// For each of the following keys go to the corresponding room
		for (int i = 1; i < 10; i++)
		{
			if (Input.GetKeyDown(""+i)) {
				goToRoom(i);
				Data.currentLevel = i;
				return;
			}
		}
		// Resets the game
		if (Input.GetKeyDown(KeyCode.R)) {
			resets();
			SceneManager.LoadScene ("Main Game");
		}

		// Goes to the main menu
		if (Input.GetKeyDown(KeyCode.M)) {
			resets();
			SceneManager.LoadScene ("Menu");
		}

		// Toggles Fullscreen
		Screen.fullScreen = !Screen.fullScreen;
	}

	// Moves the player to a given room
	public void goToRoom(int roomNumber) {
		if (roomNumber-1 < roomInitialLocations.Length) {
			player.transform.position = roomInitialLocations[roomNumber-1];
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
		Debug.Log("Going to Room 1");
		goToRoom(Data.currentLevel);
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
}
