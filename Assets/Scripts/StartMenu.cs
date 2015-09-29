using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {
	bool started;
	// Use this for initialization
	void Start () {
		Time.timeScale = 0;
		Cursor.visible = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnGUI() {
		if (!started && GUI.Button(new Rect(Screen.width * .45f, Screen.height * .95f, Screen.width * .1f, Screen.height * .05f), "Start")) {
			Time.timeScale = 1;
			started = true;
			//GameObject.Find("GuiText").BroadcastMessage("GamePaused", false);
			Cursor.visible = false;
			GameObject.Destroy(this.gameObject);
		} else {
			if (!started) {
				Cursor.visible = true;
				GUI.Box(new Rect(Screen.width * .15f, Screen.height * .4f, Screen.width * .7f, Screen.height * .2f), 
			        "Controls: " +
			        "\n Fire = Mouse1, Aim = Mouse2, Reload = R, Change Weapons = Scroll Wheel, Flashlight = T" +
			        "\n Move = W, A, S, D, Crouch = Shift, Jump = Space, Lean Right = E, Lean Left = Q, Holster Weapon = G" +
			        "\n\n Shooting in the golden heads does more damage" +
			        "\n\n Ammo packs are highlighted on the ground and can spawn after a killed robot." +
			        "\n\n GOOD LUCK!");
				//GameObject.Find("GuiText").BroadcastMessage("GamePaused", true);
			}
		}
	}
}
