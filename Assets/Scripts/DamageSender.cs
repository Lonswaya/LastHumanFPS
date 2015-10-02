using UnityEngine;
using System.Collections;

public class DamageSender : MonoBehaviour {
	public float rate = 1;
	public float health = 5;
	public bool stays;
	public bool alone;


	void takeDamage(float f) {

		health -= f;
		if (health < 0 && !stays)  { 
			if (!alone) {
				this.SendMessageUpwards("takeAllDamage", rate * f);
				if (rate > 1 && !alone) this.SendMessageUpwards("Headshot");
				this.gameObject.AddComponent<Rigidbody>();
				this.transform.SetParent(null);
				Alive a = this.gameObject.AddComponent<Alive>();
				a.time = 10;
			}
			alone = true;

		} else {
			this.SendMessageUpwards("takeAllDamage", rate * f);
			if (rate > 1 && !alone) this.SendMessageUpwards("Headshot");
		}
		
	}
	void OnParticleCollision(GameObject other) {
		//print("ow");
		if (!alone) {
			Vector3 direction = transform.position - other.transform.position;
			this.GetComponentInParent<Rigidbody>().AddForce (Vector3.up * 50);
			this.GetComponentInParent<Rigidbody>().AddForce (direction.normalized * 500);
		}
	}
}
