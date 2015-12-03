using UnityEngine;
using System.Collections;

public class TurretInteractionController : MonoBehaviour {

	[Header("Game Objects")]
	public GameObject turretObject; // Needs splitup into rotation
	public GameObject turretCamera;
	public GameObject playerCamera;
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

	private float RAYCAST_LENGTH = 10.0f;
	private bool playerInTurret;
	private float rotationY = 0.0f;
	private float lastShotTime;

	void Start () {
		playerInTurret = false;
	}

	void FixedUpdate () {
		// TODO clean up this shit
		if(Input.GetMouseButton(0) && playerInTurret == true && Time.time - lastShotTime > weaponCooldown){
			GameObject bolt = Instantiate(laserBolt);
			bolt.transform.rotation = transform.rotation;
			bolt.transform.position = projectileEmissionPoint.transform.position;

			lastShotTime = Time.time;

			bolt.GetComponent<Rigidbody>().velocity = bolt.transform.right * projectileSpeed;
			Destroy(bolt, 2.0f);
		}

		if(Input.GetMouseButtonDown(0) && playerInTurret == false){
			RaycastHit hit;
			Ray click_ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

			if(Physics.Raycast(click_ray, out hit, RAYCAST_LENGTH) ){
				if(hit.collider.gameObject == turretObject){
					print("Clicked turret");
					turretCamera.GetComponent<Camera>().enabled = true;
					playerCamera.GetComponent<Camera>().enabled = false;
					playerInTurret = true;
				}
			}
		}



		if(Input.GetKey("q") && playerInTurret == true){
			turretCamera.GetComponent<Camera>().enabled = false;
			playerCamera.GetComponent<Camera>().enabled = true;
			playerInTurret = false;
		}

		if(playerInTurret){
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(0, rotationX, rotationY);
		}
	}


}
