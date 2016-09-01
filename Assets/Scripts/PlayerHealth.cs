using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	public float startHealth = 100;
	public float currentHealth;
	public bool dying;
	private float dyingTime;
	public int kills;
	
	// Use this for initialization
	void Start () {
		currentHealth = startHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth <= 0) {
			if (!dying) {
				this.GetComponent<Rigidbody>().freezeRotation = false;
				this.GetComponent<Rigidbody>().angularDrag = 2;
				this.GetComponent<Rigidbody>().AddTorque(new Vector3(40, 0, 0));
				this.GetComponent<CapsuleCollider>().material.dynamicFriction = .5f;
			}
			this.BroadcastMessage("isDead");
			dying = true;

		}
		if (dying) {
			dyingTime += Time.deltaTime;

		}
		//GameObject.Find("GuiText").GetComponent<GUIText>().text = GameObject.Find("GuiText").GetComponent<GUIText>().text +  "\nHealth: " + currentHealth.ToString("F2") + "/" + startHealth; 
	}
	void OnGUI() {
		if (dyingTime > 5) {
			Cursor.visible = true;
			GUIStyle customButton = new GUIStyle();
			customButton.fontSize = 20;
			GUI.Box(new Rect(Screen.width * .4f, Screen.height * .35f, Screen.width * .2f, Screen.height * .1f), "<color=red><size=40>GAME OVER</size></color>");
			if (GUI.Button(new Rect(Screen.width * .5f, Screen.height * .95f, Screen.width * .1f, Screen.height * .05f), "Retry")) {
				Application.LoadLevel("LastHuman");
			}
			if (GUI.Button(new Rect(Screen.width * .4f, Screen.height * .95f, Screen.width * .1f, Screen.height * .05f),  "Exit Game")) {
				Application.Quit();
			}
		} else {
			if (Time.timeScale > 0) {
				Cursor.visible = false;
			}
		}
	}
	void takeDamage(float f) {
		if (currentHealth - f > 0) currentHealth -= f;
		else currentHealth = 0;
	}
	void kill() {
		kills++;
	}
}
