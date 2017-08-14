using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InterSceneData;

public class RoomManager : MonoBehaviour {

	public GameObject player;
	public GameObject[] rooms;
	public bool firstTime = true;
	public Vector3[] roomInitialLocations;

	private int playerPosition;

	// Use this for initialization
	void Start () {
		Debug.Assert(rooms.Length == roomInitialLocations.Length, "Error: Room Manager was not set up properly");
	
	}

	// Update is called once per frame
	void Update () {
		//if (Application.isEditor) {
			// For each of the following keys go to the corresponding room
			for (int i = 1; i < 10; i++)
			{
				if (Input.GetKeyDown(""+i)) {
					goToRoom(i);
					Data.currentLevel = i;
					return;
				}
			}
		//}

		
		// Resets the game
		if (Input.GetKeyDown(KeyCode.L)) {
			resets();
			SceneManager.LoadScene("Main Game");
		}

		// Toggles Fullscreen
		if (Input.GetKeyDown(KeyCode.F)) {
			Screen.fullScreen = !Screen.fullScreen;
		}

		// Goes to the main menu
		if (Input.GetKeyDown(KeyCode.M) && SceneManager.sceneCount < 2) {
			StartCoroutine(setScene("Menu"));
		}
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
}
