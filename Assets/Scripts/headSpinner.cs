using UnityEngine;
using System.Collections;

public class headSpinner : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 200 * Time.deltaTime, 0);
		Vector3 direction = (GameObject.Find ("Player").transform.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		if(this.GetComponentInParent<Enemy>().flying ) {
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 100); //keep light focused on player

		} 


		if (transform.GetComponentInParent<Enemy>().dying) {
			foreach (Light l in this.GetComponentsInChildren<Light>()) {
				//print(l.transform.name);
				l.range -= 100 * Time.deltaTime;
				l.intensity -= 1 * Time.deltaTime;

			}
		}
	}
}
