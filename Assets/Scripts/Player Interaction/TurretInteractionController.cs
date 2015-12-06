using UnityEngine;
using System.Collections;

public class TurretInteractionController : MonoBehaviour {

	[Header("Game Objects")]
	public GameObject turretObject; // Needs splitup into rotation
	public GameObject turretCamera;
	public GameObject playerCamera;
	public GameObject playerObject;
	public GameObject projectileEmissionPoint;
	public GameObject laserBolt;

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

	// Constants
	private const float INTERACTION_DISTANCE = 10.0f;
	private Vector3 SCREEN_CENTER = new Vector3(0.5f, 0.5f, 0.0f);

	void Start () {
		playerInTurret = false;
	}

	void FixedUpdate () {
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
		GameObject bolt = Instantiate(laserBolt);
		bolt.transform.rotation = transform.rotation;
		bolt.transform.position = projectileEmissionPoint.transform.position;

		lastShotTime = Time.time;

		bolt.GetComponent<Rigidbody>().velocity = bolt.transform.right * projectileSpeed;
		Destroy(bolt, 2.0f);
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
		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

		transform.localEulerAngles = new Vector3(0, rotationX, rotationY);
	}

	//####################################################################
	// Queries
	//####################################################################

	bool TurretInInteractableRange(){
		RaycastHit hit;
		Ray click_ray = Camera.main.ViewportPointToRay(SCREEN_CENTER);

		if(Physics.Raycast(click_ray, out hit, INTERACTION_DISTANCE)){
			return hit.collider.gameObject == turretObject;
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
		turretCamera.GetComponent<Camera>().enabled = playerInTurret;
		playerCamera.GetComponent<Camera>().enabled = !playerInTurret;
	}


}
