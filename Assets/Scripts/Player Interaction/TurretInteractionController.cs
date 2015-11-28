using UnityEngine;
using System.Collections;

public class TurretInteractionController : MonoBehaviour {

	public GameObject turretObject;
	public GameObject turretCamera;
	public GameObject playerCamera;

	private float RAYCAST_LENGTH = 10.0f;
	private bool playerInTurret;

	void Start () {
		playerInTurret = false;
	}

	void FixedUpdate () {
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray click_ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

			if(Physics.Raycast(click_ray, out hit, RAYCAST_LENGTH)){
				if(hit.collider.gameObject == turretObject){
					print("Clicked turret");
					turretCamera.GetComponent<Camera>().enabled = true;
					playerCamera.GetComponent<Camera>().enabled = false;
				}
			}
		}
	}
}
