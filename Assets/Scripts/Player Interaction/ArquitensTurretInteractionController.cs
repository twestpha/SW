using UnityEngine;
using System.Collections;

public class ArquitensTurretInteractionController : MonoBehaviour {

	[Header("Game Objects")]
	public GameObject playerCamera;
	public GameObject playerObject;


	[Header("Primary Turret")]
	public GameObject primaryTurretObject; // Needs splitup into rotation
	public GameObject primaryGunObject;
	public GameObject primaryTurretCamera;
	public GameObject[] upperPrimaryEmissionPoints;
	public GameObject[] lowerPrimaryEmissionPoints;

	[Header("Instantiation Objects")]
	public GameObject laserBolt;
	public GameObject lightFlash;

	[Header("Turret Controls")]
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

	[Header("Firing Controls")]
	public float projectileSpeed;
	public float weaponCooldown;

	// Private Variables
	private bool playerInTurret;
	private float rotationY = 0.0f;

	private float lastShotTime;

	private Vector3 primaryGunOriginalPosition;

	private bool useUpperEmitPoints;
	private bool recoiling;

	// Constants
	private const float INTERACTION_DISTANCE = 10.0f;
	private Vector3 SCREEN_CENTER = new Vector3(0.5f, 0.5f, 0.0f);

	void Start () {
		playerInTurret = false;
		useUpperEmitPoints = true;
		recoiling = false;

		primaryGunOriginalPosition = primaryGunObject.transform.localPosition;
	}

	void FixedUpdate () {
		handleRecoil();

		if(playerInTurret){
			HandleRotation();

			if(Input.GetMouseButton(0) && CanFireWeapon()){
				FireWeapon();
			}
		}

		if(Input.GetMouseButtonDown(0) && playerInTurret == false && TurretInInteractableRange() ){
			EnterTurret();
		}

		if(Input.GetKey("q") && playerInTurret == true){
			ExitTurret();
		}
	}

	//####################################################################
	// Actions
	//####################################################################

	void FireWeapon(){
		createAndFireUpperLasers();
		// lower lasers
		flashLights();
		// play sound
		recoiling = true;
		useUpperEmitPoints = !useUpperEmitPoints;
	}

	void EnterTurret(){
		lastShotTime = Time.time;
		playerInTurret = true;
		SetPlayerLocation();
	}

	void ExitTurret(){
		playerInTurret = false;
		SetPlayerLocation();
	}

	//####################################################################
	// Handlers
	//####################################################################

	void HandleRotation(){
		float rotationX = primaryTurretObject.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

		rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

		primaryTurretObject.transform.localEulerAngles = new Vector3(270, rotationX, 0);
		primaryGunObject.transform.localEulerAngles = new Vector3(0, rotationY, 0);
	}

	void handleRecoil(){
		if(recoiling){
			float percentTimeElapsed = (Time.time - lastShotTime) / weaponCooldown;

			Vector3 newPosition = new Vector3(
				primaryGunOriginalPosition.x - (0.1f * Mathf.Sin(percentTimeElapsed * Mathf.PI)),
				primaryGunOriginalPosition.y,
				primaryGunOriginalPosition.z
			);

			primaryGunObject.transform.localPosition = newPosition;

			recoiling = !CanFireWeapon();
		}
		if(!recoiling){
			primaryGunObject.transform.localPosition = primaryGunOriginalPosition;
		}
	}

	//####################################################################
	// Queries
	//####################################################################

	bool TurretInInteractableRange(){
		RaycastHit hit;
		Ray click_ray = Camera.main.ViewportPointToRay(SCREEN_CENTER);

		if(Physics.Raycast(click_ray, out hit, INTERACTION_DISTANCE)){
			return hit.collider.gameObject == primaryTurretObject;
		}
		return false;
	}

	bool CanFireWeapon(){
		return Time.time - lastShotTime > weaponCooldown;
	}

	//####################################################################
	// Helper Functions
	//####################################################################

	void SetPlayerLocation(){
		primaryTurretCamera.GetComponent<Camera>().enabled = playerInTurret;
		playerCamera.GetComponent<Camera>().enabled = !playerInTurret;
	}

	void createAndFireUpperLasers(){
		GameObject boltA = Instantiate(laserBolt);
		GameObject boltB = Instantiate(laserBolt);

		boltA.transform.localEulerAngles = new Vector3(0, primaryTurretObject.transform.localEulerAngles.y, -1.0f * rotationY);
		boltB.transform.localEulerAngles = new Vector3(0, primaryTurretObject.transform.localEulerAngles.y, -1.0f * rotationY);


		if(useUpperEmitPoints){
			boltA.transform.position = upperPrimaryEmissionPoints[0].transform.position;
			boltB.transform.position = upperPrimaryEmissionPoints[1].transform.position;
		} else {
			boltA.transform.position = lowerPrimaryEmissionPoints[0].transform.position;
			boltB.transform.position = lowerPrimaryEmissionPoints[1].transform.position;
		}

		lastShotTime = Time.time;

		boltA.GetComponent<Rigidbody>().velocity = boltA.transform.right * projectileSpeed;
		boltB.GetComponent<Rigidbody>().velocity = boltB.transform.right * projectileSpeed;

		Destroy(boltA, 2.0f);
		Destroy(boltB, 2.0f);
	}

	void flashLights(){
		GameObject lightFlashA = Instantiate(lightFlash);
		GameObject lightFlashB = Instantiate(lightFlash);

		if(useUpperEmitPoints){
			lightFlashA.transform.position = upperPrimaryEmissionPoints[0].transform.position;
			lightFlashB.transform.position = upperPrimaryEmissionPoints[1].transform.position;
		} else {
			lightFlashA.transform.position = lowerPrimaryEmissionPoints[0].transform.position;
			lightFlashB.transform.position = lowerPrimaryEmissionPoints[1].transform.position;
		}

		Destroy(lightFlashA, 0.1f);
		Destroy(lightFlashB, 0.1f);
	}
}
