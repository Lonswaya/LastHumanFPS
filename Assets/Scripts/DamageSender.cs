using UnityEngine;
using System.Collections;

public class DamageSender : MonoBehaviour {
	public float rate = 1;

	void takeDamage(float f) {
		this.SendMessageUpwards("takeAllDamage", rate * f);
	}
	void OnParticleCollision(GameObject other) {
		//print("ow");
		Vector3 direction = transform.position - other.transform.position;
		this.GetComponentInParent<Rigidbody>().AddForce (Vector3.up * 50);
		this.GetComponentInParent<Rigidbody>().AddForce (direction.normalized * 500);
	}
}
