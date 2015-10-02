using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float health = 30;
	public bool flying;
	private float dieTime;
	public bool dying;
	public GameObject ammo;
	public AudioClip[] shock;
	public AudioClip[] death;

	private float timeSinceHeadshot;
	void Start() {
		timeSinceHeadshot = 30;
		if (flying) {
			this.GetComponent<Rigidbody>().useGravity = false;
			this.GetComponent<Rigidbody>().freezeRotation = false;
		}

	}

	void takeAllDamage(float f) {
		health -= f;
	}
	void Headshot() {
		timeSinceHeadshot = 0;
	}
	void Update () {
		timeSinceHeadshot += Time.deltaTime;
		if (health < 10) {
			this.GetComponent<Rigidbody>().freezeRotation = false;

		}
		if (health <= 0) {
			if (!dying) {
				GameObject.Find("Player").SendMessage("kill");
				this.GetComponent<AudioSource>().clip = death[Random.Range(0, death.Length)];
				this.GetComponent<AudioSource>().Play();
				this.GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
				this.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), Random.Range(-8, 8)));

				if (Random.Range(0f, 1f) > .7f) {
					GameObject.Instantiate(ammo, this.transform.position, this.transform.rotation);

				}
			}
			this.GetComponent<Rigidbody>().useGravity = true;
			dying = true;
			

		}
		if (dying) { 
			dieTime+= Time.deltaTime;
			if (transform.FindChild("Head") && !transform.FindChild("Head").GetComponent<Rigidbody>() && timeSinceHeadshot < 3) {
				transform.FindChild("Head").gameObject.AddComponent<Rigidbody>();
				transform.FindChild("Head").GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
				transform.FindChild("Head").GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), Random.Range(-8, 8)));
			}/*
			if (!flying) {
				if (transform.FindChild("Body") && !transform.FindChild("Body").FindChild("Arm 1").GetComponent<Rigidbody>()) {
					transform.FindChild("Body").FindChild("Arm 1").gameObject.AddComponent<Rigidbody>();
					transform.FindChild("Body").FindChild("Arm 1").GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
					transform.FindChild("Body").FindChild("Arm 1").GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), Random.Range(-8, 8)));
				}
				if (transform.FindChild("Body") && !transform.FindChild("Body").FindChild("Arm 2").GetComponent<Rigidbody>()) {
					transform.FindChild("Body").FindChild("Arm 2").gameObject.AddComponent<Rigidbody>();
					transform.FindChild("Body").FindChild("Arm 2").GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
					transform.FindChild("Body").FindChild("Arm 2").GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), Random.Range(-8, 8)));

				}
			} else {
				if (transform.FindChild("Arm 1") && !transform.FindChild("Arm 1").GetComponent<Rigidbody>()) {
					transform.FindChild("Arm 1").gameObject.AddComponent<Rigidbody>();
					transform.FindChild("Arm 1").GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
					transform.FindChild("Arm 1").GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), Random.Range(-8, 8)));
				}
				if (transform.FindChild("Arm 2") && !transform.FindChild("Arm 2").GetComponent<Rigidbody>()) {
					transform.FindChild("Arm 2").gameObject.AddComponent<Rigidbody>();
					transform.FindChild("Arm 2").GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
					transform.FindChild("Arm 2").GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), Random.Range(-8, 8)));
					
				}
				if (transform.FindChild("Arm 3") && !transform.FindChild("Arm 3").GetComponent<Rigidbody>()) {
					transform.FindChild("Arm 3").gameObject.AddComponent<Rigidbody>();
					transform.FindChild("Arm 3").GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
					transform.FindChild("Arm 3").GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), Random.Range(-8, 8)));
				}
				if (transform.FindChild("Arm 4") && !transform.FindChild("Arm 4").GetComponent<Rigidbody>()) {
					transform.FindChild("Arm 4").gameObject.AddComponent<Rigidbody>();
					transform.FindChild("Arm 4").GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
					transform.FindChild("Arm 4").GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), Random.Range(-8, 8)));
					
				}
			}*/
			foreach (AudioSource a in this.GetComponentsInChildren<AudioSource>()) {
				a.pitch = a.pitch - dieTime * Time.deltaTime;
				a.volume = a.pitch = a.pitch - dieTime * Time.deltaTime;
			}

			if (dieTime > 3) {
				GameObject.Destroy(this.gameObject);
			}
		}

		if (Vector3.Distance(GameObject.Find("Player").transform.position, this.transform.position) < 100 && !dying && Time.timeScale > 0) {
			Vector3 direction = (GameObject.Find ("Player").transform.position - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			if(!flying ) { //face the enemy, different ways to if flying or not
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1);
				
			} else {

			}
			 	
			if (!flying)  {
				this.GetComponent<Rigidbody>().AddForce( (GameObject.Find("Player").transform.position - this.transform.position) * Mathf.Log(Vector3.Distance(GameObject.Find("Player").transform.position, this.transform.position)) / 10);
			} else {
				transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
				this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 50, 0);
				this.GetComponent<Rigidbody>().AddForce(Vector3.up * -1.2f * (1 - (Mathf.Log(Vector3.Distance(GameObject.Find("Player").transform.position , this.transform.position)))));
				this.GetComponent<Rigidbody>().AddForce(.4f * (GameObject.Find("Player").transform.position - this.transform.position));
			}
			this.GetComponent<Rigidbody>().AddForce(1 * this.GetComponent<Rigidbody>().velocity * -1);
		}
	}
	void OnCollisionEnter(Collision col) {
		if (col.transform.GetComponent<PlayerHealth>() != null && !dying) {
			this.GetComponent<AudioSource>().clip = shock[Random.Range(0, shock.Length)];
			this.GetComponent<AudioSource>().Play();
			col.transform.SendMessage("takeDamage", Random.Range(10f, 30f));
			col.transform.GetComponent<Rigidbody>().AddForce(50 * Vector3.up);
			col.transform.GetComponent<Rigidbody>().AddForce(10 * (col.transform.position - this.transform.position));
		}
	}
}
