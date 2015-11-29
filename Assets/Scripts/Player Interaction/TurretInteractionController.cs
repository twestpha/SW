using UnityEngine;
using System.Collections;

public class TurretInteractionController : MonoBehaviour {

	public GameObject turretObject;
	public GameObject turretCamera;
	public GameObject playerCamera;

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

	private float RAYCAST_LENGTH = 10.0f;
	private bool playerInTurret;

	void Start () {
		playerInTurret = false;
	}

	void FixedUpdate () {
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



		if(playerInTurret){
			if (axes == RotationAxes.MouseXAndY)
	        {
	            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

	            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
	            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

	            transform.localEulerAngles = new Vector3(0, rotationX, rotationY);
	        }
	        else if (axes == RotationAxes.MouseX)
	        {
	            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
	        }
	        else
	        {
	            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
	            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

	            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, rotationY);
	        }
		}
	}
}
