using UnityEngine;
using System.Collections;

public class FlashlightController : MonoBehaviour {
	public float intensity;
	public bool on;
	public KeyCode kon = KeyCode.T;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Light l = this.GetComponent<Light>();
		if (on && Input.GetKeyDown(KeyCode.T)) {
			if (l.intensity > 0) {
				l.intensity = 0;
			} else {
				l.intensity = intensity;
			}
		}
	}
}
