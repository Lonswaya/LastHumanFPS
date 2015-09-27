using UnityEngine;
using System.Collections;

public class AmmoPack : MonoBehaviour {
	public int amt = 30;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Transform light = transform.FindChild("Light") ;
		light.transform.position = this.transform.position + Vector3.up * .5f;
		Vector3 direction = (transform.position - light.transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		light.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1);
	}
	void OnTriggerEnter(Collider col) {
		if (col.GetComponentInChildren<WeaponController>() != null) {
		
			col.GetComponentInChildren<WeaponController>().SendMessage("GetAmmo", amt);
			
			GameObject.Destroy(this.gameObject);
		}
	}
}
