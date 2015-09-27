using UnityEngine;
using System.Collections;

public class weaponScript : MonoBehaviour {
	public GameObject bulletHole;
	public int magcnt = 9; //default values for pistol
	public float inBetweenshots = .1f;
	public float reload = 3;
	public float bulletReload = .5f;
	public float recoil = 1;
	public int bltspershot = 1;
	public int pocket = 160;
	public bool dropmag = true;
	public bool fullauto = false;
	public int currentChamber;
	public float caliber = 6;
	public Vector3 offset;

	public AudioClip[] ric;
	public AudioClip reloadC;
	public AudioClip breloadC;
	public AudioClip gunShot;
	private bool flash;
	private float flashIndex;
	private bool pressed, lastpressed, reloading, pin, gotMag;
	public int currentMag;
	private float timeBetween, reloadTime;
	private float timeSinceShooting;
		// Use this for initialization
	void Start () {
		//currentMag = magcnt;
		timeSinceShooting = 30;
		if (!gotMag) currentMag = magcnt;
		currentChamber = 1;
		pin = true;
	}
	void mDown() {

		pressed = true;
	}
	void mUp() {
		this.GetComponentInChildren<ParticleSystem>().Stop();
		pressed = false;
	}
	void Fire() {
		if (pin == true) {
			pin = false; //click
			if (currentChamber != 0) { 
				this.GetComponent<AudioSource>().clip = gunShot;
				this.GetComponent<AudioSource>().Play();
				timeSinceShooting = 0;
				for (int i = 0; i < bltspershot; i++) transform.FindChild("Back End").GetComponent<ParticleSystem>().Emit(1);
				transform.FindChild("Front End").GetComponent<ParticleSystem>().Emit(1);
				transform.FindChild("Muzzle Flash").GetComponent<ParticleSystem>().Emit(1);
				Vector3 fwd = transform.TransformDirection(Vector3.back);
				flash = true;
				flashIndex = 0;
				//print(fwd);
				RaycastHit hit;
				int layerMask = 1 << 9; //bit shift for layer 9
				layerMask = ~layerMask; //only layer 9
				//Debug.DrawRay(transform.position, transform.TransformDirection (Vector3.forward) * 100, Color.yellow);
				if (Physics.Raycast(transform.position, fwd, out hit, Mathf.Infinity, layerMask)) {
					//print(hit.collider.name + " " + hit.collider.transform.position);
					if (hit.collider.GetComponent<DamageSender>() != null) {
						hit.collider.SendMessage("takeDamage",this.caliber);
						//print(hit.transform.name);
					}
					if (hit.transform.GetComponent<Rigidbody>() != null) {
						hit.transform.GetComponent<Rigidbody>().AddForce(30 * (hit.point - transform.position)/hit.distance);
					}
					Debug.DrawRay(transform.position, transform.FindChild("Back End").TransformDirection (Vector3.forward) * hit.distance, Color.yellow);
					Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
					GameObject g = (GameObject)Instantiate(bulletHole, hit.point, hitRotation);
					g.GetComponent<AudioSource>().clip = ric[Random.Range(0, ric.Length)];
					
					//g.transform.localScale = new Vector3(1/hit.transform.lossyScale.x, 1/hit.transform.lossyScale.y, 1/hit.transform.lossyScale.z);
					g.transform.parent = hit.collider.transform;
					g.layer = 10;
					g.GetComponent<AudioSource>().Play();
				} else {
					//print("Nothing");
				}
				this.SendMessageUpwards("takeRecoil",recoil);
				GameObject.Find("Player").SendMessage("takeRecoil",recoil);
				currentChamber--;
				cock(); //slide action
			} 
		} else {
			//print("click");
		}
	}
	void Reload() {
		if (!reloading){
			reloading = true;
			this.transform.FindChild("Front End").GetComponent<AudioSource>().clip = breloadC;
			this.transform.FindChild("Front End").GetComponent<AudioSource>().Play();
		}
	}
	public void cock(){ 
		if (currentMag != 0) {
			currentChamber = 1;
			currentMag--;
			pin = true;
		}
	}
	// Update is called once per frame
	void Update () {
		timeSinceShooting += Time.deltaTime;
		
		//this.GetComponentInChildren<TrailRenderer>().endWidth = .0001f / timeSinceShooting;
		//this.GetComponentInChildren<TrailRenderer>().startWidth = .003f / timeSinceShooting;

		
		GameObject.Find("GuiText").GetComponent<GUIText>().text = currentChamber + " + " + currentMag + "/" + pocket + "  " + (reloading?"Reloading":"")
			+"\nHealth: " + transform.GetComponentInParent<PlayerHealth>().currentHealth.ToString("F2") + "/" + transform.GetComponentInParent<PlayerHealth>().startHealth + 
			"\nKills: " + transform.GetComponentInParent<PlayerHealth>().kills;
		timeBetween+=Time.deltaTime; //for time in between shots


		if (flash) {
			flashIndex += 150 * Time.deltaTime;
			if (flashIndex > 5f) {
				flashIndex = 5f;
				flash = false;
			}
		} else {
			if (flashIndex > 0) flashIndex -= 30 * Time.deltaTime;
			else flashIndex = 0;
		}
		GameObject.Find("Muzzle Flash").GetComponent<Light>().intensity = flashIndex;



		//print(timeBetween);
		if (pressed) {
			if (timeBetween > inBetweenshots && !reloading) {
				if (!lastpressed || fullauto) {
					timeBetween = 0;
					Fire();
				}

			}
		} 
		if (reloading) {
			pocket += currentMag;
			currentMag = 0;
			
			reloadTime += Time.deltaTime;
			if (reloadTime > reload) {
				if (magcnt <= pocket) {
					pocket -= magcnt;
					currentMag = magcnt;
				} else {
					currentMag = pocket;
					pocket = 0;
				}
				if (currentChamber == 0) cock();
				this.transform.FindChild("Front End").GetComponent<AudioSource>().clip = reloadC;
				this.transform.FindChild("Front End").GetComponent<AudioSource>().Play();
				reloading = false;
				//print("reloaded");
				reloadTime = 0;

			}
		}
		lastpressed = pressed;
	}
	public int getCurrentMag() {
		return currentMag;
	}
	public void setCurrentMag(int i) {
		currentMag = i;
		gotMag = true;
	}
	void OnTriggerEnter(Collider col) {
		//print("enter");
		this.SendMessageUpwards("againstWalls", true);
	}
	void OnTriggerExit(Collider col) {
		//print("exit");
		this.SendMessageUpwards("againstWalls", false);
	}

}
