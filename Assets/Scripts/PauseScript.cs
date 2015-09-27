using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {
	bool IsPaused;
	bool otherPaused;
	// Use this for initialization
	void Start(){

		
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) ){
			IsPaused = !IsPaused;

		}

		if (otherPaused) Time.timeScale = 0;
		if (IsPaused) Time.timeScale = 0;
	}
	
	void OnGUI(){
		if (!otherPaused) {
			if (IsPaused){
				//this.BroadcastMessage("PauseGame", true);
				Time.timeScale = 0;
				GUI.Box (new Rect ((Screen.width * .45f),Screen.height * .1f,Screen.width * .1f,Screen.height * .1f), "Menu");
				
				// Make the Quit button.
				if (GUI.Button (new Rect (Screen.width * .2f, (Screen.height * .9f) ,Screen.width * .2f,Screen.height * .1f), "Restart")) {
					Application.LoadLevel("LastHuman");
				}

				if(GUI.Button(new Rect(Screen.width * .4f,Screen.height * .7f,Screen.width * .2f,Screen.height * .1f), "Back")){
					IsPaused = false;
				}
				if (GUI.Button (new Rect (Screen.width * .6f, (Screen.height * .9f),Screen.width * .2f ,Screen.height * .1f), "Exit Game")) {
					Application.Quit();
				}
				

			} else {
				//this.BroadcastMessage("PauseGame", false);
				IsPaused = false;
				Time.timeScale = 1;
			}
		} else {
			Time.timeScale = 0;
		}
		
	}
	void GamePaused(bool b) {
		otherPaused = b;
	}
}
