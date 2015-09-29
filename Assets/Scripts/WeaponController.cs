using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
	public float adsIndex; //aiming down sight 
	public GameObject[] weps;
	public bool holstered;
	public KeyCode holsterKey = KeyCode.G;
	public GameObject bullet;
	public KeyCode kreload = KeyCode.R;
	public bool aimed, dead;
	public int startWeapon;
	public float switchTime = .3f;
	public AudioClip itemSwitch;
	public AudioClip pickup;

	private Vector3 offset;
	private int[] pocket;
	private int[] mag;
	private int[] chamber;
	private float currentSwitchTime;
	//Vector3 adsLocation, restLocation;
	private Transform wep, adsLocation, restLocation, holsteredLocation, wallLocation, moveTo;
	private Transform frontEnd, backEnd;

	private float fallAmt;
	private float recoilPower;
	private int changingWeps, currentWepIndex;
	private bool againstWall, anyAiming;
	private Vector3 recoilVec, desiredRotation, lastFrom, lastTo;
	private bool unholstering;
	private bool moveToRandom, step, aimToWall, wallToAim;
	private float stepTime;
	// Use this for initialization
	void Start () {
		//againstWall = true;
		pocket = new int[5];
		mag = new int[5];
		chamber = new int[5];
		currentWepIndex = startWeapon;
		for (int i = 0; i < weps.Length; i++) {
			//print(weps[i]);
			pocket[i] = weps[i].GetComponent<weaponScript>().pocket;
			mag[i] = weps[i].GetComponent<weaponScript>().magcnt;
			chamber[i] = weps[i].GetComponent<weaponScript>().currentChamber;
		}
		adsLocation = this.transform.FindChild("ADSLocation");
		restLocation = this.transform.FindChild("restLocation");
		wallLocation = this.transform.FindChild("wallLocation");
		moveTo = this.transform.FindChild("moveTo");
		wep = ((GameObject)GameObject.Instantiate(weps[startWeapon], restLocation.position, new Quaternion())).transform;
		wep.parent = this.transform;
		//wep.localPosition = new Vector3(0,0,0);
		//wep.localScale = new Vector3(1,1,1);
		wep.localRotation = Quaternion.Euler(new Vector3(0,0,0));
		holsteredLocation = this.transform.FindChild("holsteredLocation");
		frontEnd = wep.FindChild("Front End");
		backEnd = wep.FindChild("Back End");
		moveTo.position = wep.position;
		wep.gameObject.layer = 8;
		//restLocation.transform.position = wep.transform.position;
		offset = wep.GetComponent<weaponScript>().offset;
		adsIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {


		aimed = !dead && Input.GetMouseButton(1);
		//print(aimed);
		if (!dead && Input.GetKeyDown(holsterKey) && !aimed) { //cannot holter during ads
		
			holstered = !holstered;
		}

		if ((aimed || againstWall || holstered)) { //aiming or against wall
				//aimToWall = false;
				//wallToAim = false;
				//print(adsIndex);
			unholstering = false;
			if (adsIndex < 1) {
				adsIndex += 5 * Time.deltaTime / (adsIndex+1);
				if (adsIndex > 1) adsIndex = 1;
			}
			else adsIndex = 1;

		} else { //not aiming
			if (adsIndex > 0) {
				adsIndex -= 3 * Time.deltaTime / (adsIndex+1);
				if (adsIndex < 0) adsIndex = 0;
			}
			else adsIndex = 0;

		}

		if (againstWall) {
			desiredRotation = wallLocation.localEulerAngles;
			if (aimed) {
				lastFrom = adsLocation.position;
				anyAiming = true;
			} else {
				anyAiming = false;
				lastFrom = restLocation.position;
			} 
			//print(wallLocation.localEulerAngles);
			//lastFrom = wallLocation.position;

		} else if (holstered) {
			anyAiming = false;
			lastFrom = holsteredLocation.position;
			//print("lastfrom is holst");
			desiredRotation = holsteredLocation.localEulerAngles;

		}
		else if (aimed) {

			anyAiming = true;
			lastFrom = adsLocation.position;
			//print("lastfrom is ads");
			desiredRotation = adsLocation.localEulerAngles;
		}
		else  {
			//lastTo = restLocation.position;
			//print("lastto is rest");
			//lastFocus = restLocation.position;
			desiredRotation = restLocation.localEulerAngles;
		}
		/*if (aimed && againstWall) { //swap
			Vector3 temp = lastFrom;
			lastFrom = lastTo;
			lastTo = temp;
		}*/
		/*if(aimToWall) {
			print(adsIndex);
			print("aimtowall");
			moveTo.position = adsLocation.position + ((wallLocation.position - adsLocation.position) * adsIndex);
		}
		else if (wallToAim) {
			print("walltoaim");
			moveTo.position = adsLocation.position  + ((wallLocation.position  - adsLocation.position ) * adsIndex);
		}
		else*/ 
		moveTo.position = restLocation.position + ((lastFrom - restLocation.position) * adsIndex); //move back from whatever

		if (anyAiming || adsIndex < 0) {
			this.GetComponentInParent<Camera>().fieldOfView = 45 + ((1-adsIndex) * 15);
			GameObject.Find("WepCamera").GetComponent<Camera>().fieldOfView = 45 + ((1-adsIndex) * 15);
			GameObject.Find("ParticleCamera").GetComponent<Camera>().fieldOfView = 45 + ((1-adsIndex) * 15);
		}


			 
			//if (againstWall) moveTo.position = wallLocation.position + ((holsteredLocation.position - restLocation.position) * adsIndex);
			//ParticleSystem part = wep.GetComponentInChildren<ParticleSystem>();
		if (Input.mouseScrollDelta.y > 0 && currentWepIndex < weps.Length - 1 && !holstered) {
			holstered = true;
			changingWeps = 1;
		} 
		if (Input.mouseScrollDelta.y < 0 && currentWepIndex > 0 && !holstered) {
			holstered = true;
			changingWeps = -1;
		}
		if (Input.GetKeyDown(kreload)) wep.SendMessage("Reload");
		if (!holstered) {
			if (Input.GetMouseButton(0)  && !againstWall && !dead) {
				wep.SendMessage("mDown");

					//GameObject spawnedBullet = (GameObject)GameObject.Instantiate(bullet, backEnd.position, Quaternion.Euler(new Vector3(0, 0, 90)));
					//spawnedBullet.GetComponent<Rigidbody>().AddForce((backEnd.position - frontEnd.position) * 50);
					//print(spawnedBullet.transform.position);
			} else {

				wep.SendMessage("mUp");
			}

		}

		//final addition, movement from left to right
		Vector3 finalMoveTo = moveTo.position;
		if (moveToRandom)  { 
			stepTime += Time.deltaTime;
			float stepEscalation = (aimed?.1f:1);
			float stepSwing = (5*stepTime) % 2; //swings from -.2 to 0 to -.2
			stepSwing = -1 *  Mathf.Abs(.1f * stepSwing + .1f); //math
			//print((5*stepTime) % 2);

			if ((int)(3*stepTime) % 2 == 0) {
				//left side
				//print("left");

				finalMoveTo += Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * new Vector3(stepEscalation * .15f,stepEscalation * stepSwing,0);
			} else {
				//right side
				//print("not left");

				finalMoveTo += Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * new Vector3(stepEscalation * -.15f,stepEscalation * stepSwing,0);


			}
			//moveTo.position = new Vector3(moveTo.position.x +
			//print("foo");
		}
		if (Mathf.Abs(fallAmt) > 0) {
			finalMoveTo += Vector3.up * fallAmt / -20;
			fallAmt = fallAmt + (fallAmt>0?-1:1) * Time.deltaTime * 5;
		}
		//(moveTo.position + " " + wep.position);
		finalMoveTo += offset;
		if (!dead) {
			if (Vector3.Distance(finalMoveTo, wep.position) > .5f) {

				wep.position = wep.position + (finalMoveTo - wep.position) * (Mathf.Sqrt(Vector3.Distance(finalMoveTo, wep.position) / 5)) * 30 * Time.deltaTime; //so if we get closer and closer we don't stop indefinitley 
			}
			else 
				wep.position = wep.position + (finalMoveTo - wep.position)*(Vector3.Distance(finalMoveTo, wep.position)) * 60 * Time.deltaTime;
		}
		if (changingWeps != 0) currentSwitchTime += Time.deltaTime;

		if (currentSwitchTime > switchTime) {
			currentSwitchTime = 0;
			mag[currentWepIndex] = wep.GetComponent<weaponScript>().getCurrentMag();
			chamber[currentWepIndex] = wep.GetComponent<weaponScript>().currentChamber;
			pocket[currentWepIndex] = wep.GetComponent<weaponScript>().pocket;
			//print(mag[currentWepIndex]);
			
			GameObject.Destroy(wep.gameObject);
			currentWepIndex += changingWeps;
			changingWeps = 0;
			//print(currentWepIndex);
			
			wep = ((GameObject)GameObject.Instantiate(weps[currentWepIndex], holsteredLocation.position, new Quaternion())).transform;
			//print(mag[currentWepIndex]);
			this.GetComponent<AudioSource>().clip = itemSwitch;
			this.GetComponent<AudioSource>().Play();
			wep.GetComponent<weaponScript>().setCurrentMag(mag[currentWepIndex]);
			wep.GetComponent<weaponScript>().pocket = pocket[currentWepIndex];
			wep.GetComponent<weaponScript>().currentChamber = chamber[currentWepIndex];
			wep.parent = this.transform;
			wep.localPosition = holsteredLocation.localPosition;
			//wep.localScale = new Vector3(1,1,1);
			wep.localRotation = Quaternion.Euler(new Vector3(0,0,0));
			holsteredLocation = this.transform.FindChild("holsteredLocation");
			frontEnd = wep.FindChild("Front End");
			backEnd = wep.FindChild("Back End");
			moveTo.position = wep.position;
			wep.gameObject.layer = 8;
			offset = wep.GetComponent<weaponScript>().offset;
			holstered = false;
		}

		///for recoil and moving the gun after firing
		/// add recoil calculated when recoil

		wep.localEulerAngles += recoilVec/3.5f;
		float moveSpeed = 2; //change the speed we look back
		if (this.GetComponentInChildren<WeaponController>().aimed) moveSpeed = 8; //if aimed, move the gun quicker
		//this chunk is used to move the weapon's rotation back to its original state, until it gets too close then locks to it.
		bool moved = false;
		if (!dead) {
				if (Mathf.Abs(wep.localEulerAngles.x - desiredRotation.x) > .1f) {
				moved = true;
				if (wep.localEulerAngles.x < 180) {
					wep.localEulerAngles = new Vector3(wep.localEulerAngles.x - (moveSpeed * Time.deltaTime * (wep.localEulerAngles.x - desiredRotation.x ) / ((recoilVec.x) + .5f)), wep.localEulerAngles.y, wep.localEulerAngles.z );
					recoilVec -= Time.deltaTime * 3 * recoilVec / 2;
				} else {
					wep.localEulerAngles = new Vector3(wep.localEulerAngles.x + (moveSpeed * Time.deltaTime * (wep.localEulerAngles.x- desiredRotation.x  ) / (recoilVec.x + .5f)), wep.localEulerAngles.y, wep.localEulerAngles.z );
					recoilVec -= Time.deltaTime * 3 * recoilVec / 2;
				}
			} else {
				recoilVec = new Vector3(0, recoilVec.y, recoilVec.z);
				wep.localEulerAngles = new Vector3(desiredRotation.x,wep.localEulerAngles.y,wep.localEulerAngles.z);
			}
			if (Mathf.Abs(wep.localEulerAngles.y - desiredRotation.y) > .2f) {
				moved = true;
				if (wep.localEulerAngles.y < 180) { 
					wep.localEulerAngles = new Vector3(wep.localEulerAngles.x, wep.localEulerAngles.y - (moveSpeed * Time.deltaTime * (wep.localEulerAngles.y - desiredRotation.y) / (recoilVec.y + .5f)), wep.localEulerAngles.z);
					recoilVec -= Time.deltaTime * 3 * recoilVec / 2;
				} else { 
					wep.localEulerAngles = new Vector3(wep.localEulerAngles.x, wep.localEulerAngles.y + (moveSpeed * Time.deltaTime * (wep.localEulerAngles.y - desiredRotation.y) / (recoilVec.y + .5f)), wep.localEulerAngles.z);
					recoilVec -= Time.deltaTime * 3 * recoilVec / 2;
				} 
			} else {
				recoilVec = new Vector3(recoilVec.x, 0, recoilVec.z);
				wep.localEulerAngles = new Vector3(wep.localEulerAngles.x,desiredRotation.y,wep.localEulerAngles.z);
			}
			if (Mathf.Abs(wep.localEulerAngles.z- desiredRotation.z) > .2f) {
				moved = true;
				if (wep.localEulerAngles.z < 180) {
					wep.localEulerAngles = new Vector3(wep.localEulerAngles.x, wep.localEulerAngles.y, wep.localEulerAngles.z  - (moveSpeed * Time.deltaTime / (Vector3.Magnitude(recoilVec) + .5f)));
					recoilVec -= Time.deltaTime * 3 * recoilVec / 2;
				} else {
					wep.localEulerAngles = new Vector3(wep.localEulerAngles.x, wep.localEulerAngles.y,wep.localEulerAngles.z + (moveSpeed* Time.deltaTime / (Vector3.Magnitude(recoilVec) + .5f)));
					recoilVec -= Time.deltaTime * 3 * recoilVec / 2;
				}
			} else {
				recoilVec = new Vector3(recoilVec.x, recoilVec.y, 0);
				wep.localEulerAngles = new Vector3(wep.localEulerAngles.x,wep.localEulerAngles.y,desiredRotation.z);
			}
		}

		if (!moved) {
		} else {
			//print(recoilVec);
		}

		//print(wep.localEulerAngles);
		//print(recoilPower);
		if (recoilPower > 0) recoilPower -= 20 *  Time.deltaTime;
		else recoilPower = 0;

	}
	void takeRecoil(float force) {

		//float spread = wep.localEulerAngles.x + wep.localEulerAngles.z;
		//if (spread > 0) force *= spread;
		//print(recoilPower);
		if (wep.localEulerAngles.x + force + recoilPower < 5 || wep.localEulerAngles.z + force + recoilPower < 5) recoilPower += force;


		wep.position += Random.Range(force * .01f, force * .02f) * (frontEnd.position - backEnd.position) ; //recoil
		force = recoilPower;
		if (aimed) recoilVec = new Vector3(Random.Range (force * .1f, force * 1), 0, Random.Range (force * -.2f, force * .8f)); //recoil turn if aiming down sight
		else if (unholstering) recoilVec = new Vector3(Random.Range (force * .1f, force * 1f), 0, Random.Range (force * -1.5f, force * 1.5f)); //recoil turn if firing after holstering
		else recoilVec = new Vector3(Random.Range (force * .1f, force * 1), 0, Random.Range (force * -1.1f, force * 1.1f )); //recoil otherwise


	}
	void MovementAnimation(bool b) {
		moveToRandom = b;
	}
	//void SendVel(float f) {
	//	vel = f;
	//}
	void OnTriggerEnter(Collider col) {
		//print("enter");
		if (col.GetComponentInParent<Enemy>() == null) againstWall = true;
		//this.SendMessageUpwards("againstWalls", true);
	}
	void OnTriggerExit(Collider col) {
		//print("exit");
		againstWall = false;
		//this.SendMessageUpwards("againstWalls", false);
	}
	void againstWalls(bool c) {
		//print("exit");
		againstWall = c;
	}
	void FallDown(float f) {
		fallAmt = f;
	}
	void GetAmmo(int i) {
		this.GetComponent<AudioSource>().clip = pickup;
		this.GetComponent<AudioSource>().Play();
		wep.GetComponent<weaponScript>().pocket += wep.GetComponent<weaponScript>().magcnt * i;
	}
	void isDead() {
		if (!dead) {
			wep.transform.gameObject.AddComponent<Rigidbody>();
			MeshCollider m = wep.transform.gameObject.AddComponent<MeshCollider>();
			m.convex = true;
			if (wep.GetComponent<MeshFilter>() != null) m.sharedMesh = wep.GetComponent<MeshFilter>().mesh;
			else m.sharedMesh = wep.FindChild("Model").GetComponent<MeshFilter>().mesh;
		}
		
		dead = true;
	}
}
