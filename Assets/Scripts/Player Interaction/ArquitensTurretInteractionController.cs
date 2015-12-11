using UnityEngine;
using System.Collections;

public class ArquitensTurretInteractionController : MonoBehaviour {

	[Header("Game Objects")]
	public GameObject playerCamera;
	public GameObject playerObject;

	[Header("Primary Turret")]
	public GameObject primaryTurretObject;
	public GameObject primaryGunObject;
	public GameObject primaryCameraParent;
	public GameObject primaryTurretCamera;
	public GameObject[] upperPrimaryEmissionPoints;
	public GameObject[] lowerPrimaryEmissionPoints;

	[Header("Secondary Turret")]
	public GameObject secondaryTurretObject;
	public GameObject secondaryGunObject;
	public GameObject secondaryCameraParent;
	public GameObject secondaryTurretCamera;
	public GameObject[] upperSecondaryEmissionPoints;
	public GameObject[] lowerSecondaryEmissionPoints;

	[Header("Instantiation Objects")]
	public GameObject laserBolt;
	public GameObject laserKernel;
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
	public float rangeInMeters;
	public float weaponCooldown;
	public float recoilAmount = 0.2f;
	public float intersectionAngle = 0.0f;

	// Private Variables
	private bool playerInTurret;
	private bool inUpperTurret;
	private float rotationY = 0.0f;

	private float lastShotTime;

	private Vector3 primaryGunOriginalPosition;

	private bool useUpperEmitPoints;
	private bool primaryTurretRecoiling;

	// Constants
	private const float INTERACTION_DISTANCE = 10.0f;
	private Vector3 SCREEN_CENTER = new Vector3(0.5f, 0.5f, 0.0f);

	void Start () {
		playerInTurret = false;
		inUpperTurret = true;

		useUpperEmitPoints = true;
		primaryTurretRecoiling = false;

		primaryGunOriginalPosition = primaryGunObject.transform.localPosition;
	}

	void FixedUpdate () {
		if(playerInTurret){
			handleRecoil();
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
		bool shouldFirePrimary = inUpperTurret || rotationY >= intersectionAngle;
		bool shouldFireSecondary = !inUpperTurret || rotationY >= intersectionAngle;

		if(shouldFirePrimary){
			createAndFirePrimaryLasers();
			flashPrimaryLights();
			primaryTurretRecoiling = true;
		}

		if(shouldFireSecondary){
			createAndFireSecondaryLasers();
		}

		if(shouldFirePrimary || shouldFireSecondary){
			lastShotTime = Time.time;
			useUpperEmitPoints = !useUpperEmitPoints;
			// play sound
		}
	}

	void EnterTurret(){
		lastShotTime = Time.time;
		playerInTurret = true;
		SetPlayerLocation();
	}

	void ExitTurret(){
		playerInTurret = false;
		SetPlayerLocation();
		primaryTurretRecoiling = false;
	}

	//####################################################################
	// Handlers
	//####################################################################

	void HandleRotation(){
		float rotationX = primaryTurretObject.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

		rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

		primaryTurretObject.transform.localEulerAngles = new Vector3(270, rotationX, 0);
		secondaryTurretObject.transform.localEulerAngles = new Vector3(90, rotationX, 0);

		primaryGunObject.transform.localEulerAngles = new Vector3(0, rotationY, 0);
		secondaryGunObject.transform.localEulerAngles = new Vector3(0, rotationY, 0);

		primaryCameraParent.transform.localEulerAngles = new Vector3(0, rotationY, 0);
	}

	void handleRecoil(){
		if(primaryTurretRecoiling){
			float percentTimeElapsed = (Time.time - lastShotTime) / weaponCooldown;

			float recoil_distance = (recoilAmount * Mathf.Sin(2.0f * percentTimeElapsed * Mathf.PI));

			Vector3 newPosition = new Vector3(
				primaryGunOriginalPosition.x - (recoil_distance * Mathf.Cos(Mathf.Deg2Rad * rotationY)),
				primaryGunOriginalPosition.y,
				primaryGunOriginalPosition.z + (recoil_distance * Mathf.Sin(Mathf.Deg2Rad * rotationY))
			);

			primaryGunObject.transform.localPosition = newPosition;

			primaryTurretRecoiling = percentTimeElapsed < 0.5f;
		}
		if(!primaryTurretRecoiling){
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

	void createAndFirePrimaryLasers(){
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

		boltA.GetComponent<Rigidbody>().velocity = boltA.transform.right * projectileSpeed;
		boltB.GetComponent<Rigidbody>().velocity = boltB.transform.right * projectileSpeed;

		Destroy(boltA, rangeInMeters * (1 / projectileSpeed));
		Destroy(boltB, rangeInMeters * (1 / projectileSpeed));
	}

	void createAndFireSecondaryLasers(){
		GameObject boltA = Instantiate(laserBolt);
		GameObject boltB = Instantiate(laserBolt);

		boltA.transform.localEulerAngles = new Vector3(0, secondaryTurretObject.transform.localEulerAngles.y, rotationY);
		boltB.transform.localEulerAngles = new Vector3(0, secondaryTurretObject.transform.localEulerAngles.y, rotationY);


		if(useUpperEmitPoints){
			boltA.transform.position = upperSecondaryEmissionPoints[0].transform.position;
			boltB.transform.position = upperSecondaryEmissionPoints[1].transform.position;
		} else {
			boltA.transform.position = lowerSecondaryEmissionPoints[0].transform.position;
			boltB.transform.position = lowerSecondaryEmissionPoints[1].transform.position;
		}

		boltA.GetComponent<Rigidbody>().velocity = boltA.transform.right * projectileSpeed;
		boltB.GetComponent<Rigidbody>().velocity = boltB.transform.right * projectileSpeed;

		Destroy(boltA, rangeInMeters * (1 / projectileSpeed));
		Destroy(boltB, rangeInMeters * (1 / projectileSpeed));
	}

	void flashPrimaryLights(){
		GameObject lightFlashA = Instantiate(lightFlash);
		GameObject lightFlashB = Instantiate(lightFlash);

		GameObject laserKernelA = Instantiate(laserKernel);
		GameObject laserKernelB = Instantiate(laserKernel);

		if(useUpperEmitPoints){
			// Initial Position
			lightFlashA.transform.position = upperPrimaryEmissionPoints[0].transform.position;
			lightFlashB.transform.position = upperPrimaryEmissionPoints[1].transform.position;

			laserKernelA.transform.position = upperPrimaryEmissionPoints[0].transform.position;
			laserKernelB.transform.position = upperPrimaryEmissionPoints[1].transform.position;

			// Parenting
			lightFlashA.transform.parent = upperPrimaryEmissionPoints[0].transform;
			lightFlashB.transform.parent = upperPrimaryEmissionPoints[1].transform;

			laserKernelA.transform.parent = upperPrimaryEmissionPoints[0].transform;
			laserKernelB.transform.parent = upperPrimaryEmissionPoints[1].transform;
		} else {
			// Initial Position
			lightFlashA.transform.position = lowerPrimaryEmissionPoints[0].transform.position;
			lightFlashB.transform.position = lowerPrimaryEmissionPoints[1].transform.position;

			laserKernelA.transform.position = lowerPrimaryEmissionPoints[0].transform.position;
			laserKernelB.transform.position = lowerPrimaryEmissionPoints[1].transform.position;

			// Parenting
			lightFlashA.transform.parent = lowerPrimaryEmissionPoints[0].transform;
			lightFlashB.transform.parent = lowerPrimaryEmissionPoints[1].transform;

			laserKernelA.transform.parent = lowerPrimaryEmissionPoints[0].transform;
			laserKernelB.transform.parent = lowerPrimaryEmissionPoints[1].transform;
		}

		Destroy(lightFlashA, 0.1f);
		Destroy(lightFlashB, 0.1f);
	}
}
