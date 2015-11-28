using UnityEngine;
using System.Collections;

public class PlayerInteractionController : MonoBehaviour {

	public float pickupDistance;

	private GameObject interactableObject;
	private float hitDistance;
	private float hitBuffer = 1.0f;

	void Start () {
	}

	void FixedUpdate () {
		HandleInteractions();
	}

	void HandleInteractions(){
		if(Input.GetMouseButton(0)){
			if(interactableObject){
				Ray viewRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
				// RaycastHit hit;

				Vector3 newPosition = viewRay.direction * hitDistance;

				Debug.DrawRay(transform.position, newPosition - transform.position, Color.red);

				// if(Physics.Raycast(transform.position, newPosition - transform.position, out hit, hitDistance) && hit.collider.gameObject != interactableObject){
					// Our object needs to be moved back along the ray
				//
				// 	Vector3 hitPoint = transform.position + (ray.direction * (hit.distance - hitBuffer));
				// 	Vector3 newPosition = hitPoint + hitPointOffset;
				//
					// interactableObject.transform.position  = newPosition;
				// } else {
				// 	Debug.DrawRay(transform.position, (newPosition - transform.position).normalized * (hitDistance + hitBuffer), Color.green);
				// 	// We're hot to trot
				// 	// interactableObject.transform.position  = newPosition;
				// }
			} else {
				Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
				RaycastHit hit;

				if(Physics.Raycast(ray, out hit, pickupDistance)){
					if(hit.collider.gameObject.tag == "Interactable"){
						interactableObject = hit.collider.gameObject;

						interactableObject.GetComponent<Rigidbody>().useGravity = false;
						interactableObject.GetComponent<Rigidbody>().isKinematic = true;

						hitDistance = (interactableObject.transform.position - transform.position).magnitude;
						Debug.DrawRay(transform.position, interactableObject.transform.position - transform.position, Color.green);
					}
				}
			}
		} else if(interactableObject){
			print("Dropped it!");
			interactableObject.GetComponent<Rigidbody>().isKinematic = false;
			interactableObject.GetComponent<Rigidbody>().useGravity = true;
			interactableObject = null;
		}
	}
}
