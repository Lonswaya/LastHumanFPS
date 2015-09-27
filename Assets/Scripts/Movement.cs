using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	public KeyCode kforward = KeyCode.W;
	public KeyCode kback = KeyCode.S;
	public KeyCode kleft = KeyCode.D;
	public KeyCode kright = KeyCode.A;
	public KeyCode kcrouch = KeyCode.LeftShift;
	public KeyCode kjump = KeyCode.Space;
	public KeyCode kleanRight = KeyCode.E;
	public KeyCode kleanLeft = KeyCode.Q;
	public AudioClip[] footsteps;
	
	public float maxForwardSpeed = 50; //basically how fast we can run forwards
	public float maxBackSpeed = 50;	   //how fast we can strafe
	public float maxStrafeSpeed = 50;
	public float sensitivity = 1;
	private Vector3 recoilVec;
	private float crouchIndex, leanIndex; //what we use for crouching smoothly
	private bool jumped, leaned, stepThing;
	private float stepTime;
	private bool dead;
	//private float i;
	// Use this for initialization
	void Start () {
		//i = 0;
		leanIndex = 2;
		crouchIndex = 2;
	}
	void isDead() {
		dead = true;
	}
	// Update is called once per frame
	void Update () {

		Rigidbody r = this.GetComponent<Rigidbody>();
		Vector2 transVec = new Vector2(r.velocity.x, r.velocity.z);
		bool moving = Vector2.SqrMagnitude (transVec) > .5f;
		//print(Vector2.SqrMagnitude (transVec));
		Vector3 relVel = (transform.InverseTransformDirection(r.velocity)); //so we find velocity relative to us, not the world
		//print( ((crouchIndex == 2)?maxForwardSpeed/2:maxForwardSpeed));
		float modifier =  ((crouchIndex == 1)?(.5f):1) * (leaned?(.5f):1) * (jumped?(.5f):1) * (this.GetComponentInChildren<WeaponController>().aimed?(.5f):1); //for crouched, in air, or aimed
	//	print(modifier);
		if (!dead && relVel.z < modifier * maxForwardSpeed) { //get the translation speed, see if we are too fast
			if (Input.GetKey(kforward)) {
				//print(Vector3.Magnitude(r.velocity));
				r.AddForce(Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * Vector3.forward * 50);
			}
		}
		if (!dead && relVel.x < modifier * maxStrafeSpeed) {
			if (Input.GetKey(kleft)) {
				r.AddForce(Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * Vector3.right * 50 );
			}
		}
		if (!dead && relVel.x > -1 * modifier * maxStrafeSpeed) {
			if (Input.GetKey(kright)) {
				r.AddForce(Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * Vector3.left * 50);
			}
		}
		if (!dead && relVel.z > -1 * modifier * maxBackSpeed) {
			if (Input.GetKey(kback)) {
				r.AddForce(Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * Vector3.back * 50 );
			}
		}
		if (!dead && Input.GetKeyDown(kjump) && !jumped) {
			jumped = true;
			r.AddForce(Vector3.up * 200);
			this.BroadcastMessage("FallDown",-3); //for animation

		}
		float maxLean = 10;
		if (!dead && Input.GetKey(kleanRight)) {

			leaned = true;
			if (leanIndex > 1) {
				leanIndex -= Time.deltaTime * 5 / (2 * leanIndex);
				this.transform.position += Time.deltaTime * (Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * Vector3.right) / leanIndex;
			} else {
				leanIndex = 1;
			}

		} else if (!dead && Input.GetKey(kleanLeft)) {

			leaned = true;
			if (leanIndex < 3) {
				leanIndex += Time.deltaTime * 5 / (2 * leanIndex);
				this.transform.position += Time.deltaTime * (Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * Vector3.left)  / leanIndex;
			} else {
				leanIndex = 3;
			}

		} else {
			leaned = false;
			if (Mathf.Abs(leanIndex - 2) > .1f) {
				leanIndex += Time.deltaTime *8 * (2 - leanIndex);
				if (leanIndex > 2) this.transform.position += Time.deltaTime * (8/5) *(Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * Vector3.right)  / leanIndex;
				else this.transform.position += Time.deltaTime * (8/5) *(Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * Vector3.left)  / leanIndex;
			} else {
				leanIndex = 2;
			}
		}
		Transform player = GameObject.Find("Player").transform;
		player.localEulerAngles = new Vector3(player.localEulerAngles.x , player.localEulerAngles.y , (leanIndex - 2)* maxLean);


		if (!dead && Input.GetKey(kcrouch)) {
			//adjust capsule collider
			if (crouchIndex > 1) {
				crouchIndex -= Time.deltaTime * 10 / crouchIndex;
				this.GetComponent<CapsuleCollider>().height = crouchIndex;
			} else {
				crouchIndex = 1;
			}
		} else {
			if (crouchIndex < 2) {
				crouchIndex += Time.deltaTime * 15 / crouchIndex;
				this.GetComponent<CapsuleCollider>().height = crouchIndex;
			} else {
				crouchIndex = 2;
			}
		}
		if (moving && !jumped && !dead) { //footstep code
			stepTime += Time.deltaTime;
			//print(stepTime);
			if ((int)(3 * stepTime) % 2 == 0) {
				if (stepThing != true) {
					//play
					this.GetComponent<AudioSource>().clip = footsteps[Random.Range(0, footsteps.Length)];
					this.GetComponent<AudioSource>().Play();
				}
				stepThing = true;
				//print("left");
				//step = true;
				//play sound here
			} else {
				if (stepThing != false) {
					this.GetComponent<AudioSource>().clip = footsteps[Random.Range(0, footsteps.Length)];
					this.GetComponent<AudioSource>().Play();
					//play
				}
				stepThing = false;
				//print("right");
				//step = false;
			}
			this.BroadcastMessage("MovementAnimation",true); //for viewmodel animations
			//print(Vector2.SqrMagnitude(transVec));
			//this.BroadcastMessage("SendVel",Vector2.SqrMagnitude(transVec));
		} else {
			stepTime = 0;
			this.BroadcastMessage("MovementAnimation", false);
		}

		float verM = Input.GetAxis("Mouse Y");
		float horM = Input.GetAxis("Mouse X");
		if (!dead && Time.timeScale > 0) {
			this.GetComponentInChildren<Camera>().transform.Rotate(verM  * sensitivity * -1 + (recoilVec.x * -1), recoilVec.y, recoilVec.z);
			this.transform.Rotate(0, horM * sensitivity, 0);
		}
		if (recoilVec.x > 0) recoilVec = new Vector3(recoilVec.x - 10 * Time.deltaTime, recoilVec.y, recoilVec.z);
		else recoilVec = new Vector3(0, recoilVec.y, recoilVec.z);
		/*if (recoilVec.y > 0) recoilVec = new Vector3(recoilVec.x, recoilVec.y - 10 * Time.deltaTime, recoilVec.z);
		else recoilVec = new Vector3(recoilVec.x, 0, recoilVec.z);
		if (recoilVec.z > 0) recoilVec = new Vector3(recoilVec.x, recoilVec.y, recoilVec.z - 10 * Time.deltaTime);
		else recoilVec = new Vector3(recoilVec.x, recoilVec.y, 0);*/

	}
	void OnCollisionEnter(Collision col) {
		if (col.relativeVelocity.y > 2) {
			//print(col.relativeVelocity.y);
			this.BroadcastMessage("FallDown",col.relativeVelocity.y); //for antmation
		}
	}

	void OnCollisionStay(Collision col) {
		//print(this.GetComponent<Rigidbody>().velocity.y);

		if (this.GetComponent<Rigidbody>().velocity.y < .01f) jumped = false; //cannot be 0 cause dumb unity rounding
	}
	void takeRecoil(float f) {
		//print("Parent " + f);
		recoilVec += new Vector3(Random.Range(f * .1f, f * .5f), 0, /*Random.Range (f * -1.5f, f * 1.5f)*/0);
	}
}
