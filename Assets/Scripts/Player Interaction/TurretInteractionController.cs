using UnityEngine;
using System.Collections;

public class TurretInteractionController : MonoBehaviour {

	public GameObject turretObject;
	public GameObject turretCamera;
	public GameObject playerCamera;
	public GameObject gunEmitPoint;

	public GameObject laserBolt;

    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

	public float shootSpeed;
	public float weaponCooldown;

    private float rotationY = 0.0f;

	private float RAYCAST_LENGTH = 10.0f;
	private bool playerInTurret;

	private float lastShotTime;

	void Start () {
		playerInTurret = false;
	}

	void FixedUpdate () {
		// TODO clean up this shit
		if(Input.GetMouseButton(0) && playerInTurret == true && Time.time - lastShotTime > weaponCooldown){
			GameObject bolt = Instantiate(laserBolt);
			bolt.transform.rotation = transform.rotation;
			bolt.transform.position = gunEmitPoint.transform.position;

			lastShotTime = Time.time;

			bolt.GetComponent<Rigidbody>().velocity = bolt.transform.right * shootSpeed;
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
