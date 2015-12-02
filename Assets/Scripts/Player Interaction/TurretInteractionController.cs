using UnityEngine;
using System.Collections;

public class TurretInteractionController : MonoBehaviour {

	public GameObject turretObject;
	public GameObject turretCamera;
	public GameObject playerCamera;

    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

	public float trackingSpeed;

	private LineRenderer line;

    private float rotationY = 0.0f;

	private float RAYCAST_LENGTH = 10.0f;
	private bool playerInTurret;

	void Start () {
		playerInTurret = false;

		line = gameObject.GetComponent<LineRenderer>();
		line.enabled = false;
	}

	void FixedUpdate () {
		// TODO clean up this
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

		if(Input.GetMouseButtonDown(0) && playerInTurret == true){
			// pew pew
			// FireLaser();
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
