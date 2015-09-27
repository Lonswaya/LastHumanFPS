using UnityEngine;
using System.Collections;

public class BulletTaker : MonoBehaviour {
	public float health = 1;
	private float dieTime;
	private bool dying;
	// Use this for initialization
	void Start () {
	
	}
	void OnParticleCollision(GameObject other) {
		health--;
		if (health == 0) {
			dying = true;
		}
	//	print(other.name);
		Vector3 direction = transform.position - other.transform.position;
		if (dying) this.GetComponent<Rigidbody>().AddForce (direction.normalized * 1500);
		else this.GetComponent<Rigidbody>().AddForce (direction.normalized * 500);
	}
	// Update is called once per frame
	void Update () {
		if (dying) dieTime+= Time.deltaTime;
		if (dieTime > 3) GameObject.Destroy(this.gameObject);
	}
}
