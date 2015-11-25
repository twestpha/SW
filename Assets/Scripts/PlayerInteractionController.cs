using UnityEngine;
using System.Collections;

public class PlayerInteractionController : MonoBehaviour {

	public float pickupDistance;
	public float movementResistance;

	private GameObject interactableObject;
	Vector3 hitPointOffset;
	float hitDistance;

	void Start () {
	}

	void FixedUpdate () {
		HandleInteractions();
	}

	void HandleInteractions(){
		if(Input.GetMouseButton(0)){
			if(interactableObject){
				Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));



				Vector3 hitPoint = transform.position + (ray.direction * hitDistance);
				Vector3 newPosition = hitPoint + hitPointOffset;

				Debug.DrawRay(transform.position, newPosition - transform.position, Color.red);

				interactableObject.transform.position  = newPosition;
			} else {
				Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
				RaycastHit hit;

				if(Physics.Raycast(ray, out hit, pickupDistance)){
					if(hit.collider.gameObject.tag == "Interactable"){
						interactableObject = hit.collider.gameObject;

						interactableObject.GetComponent<Rigidbody>().useGravity = false;
						interactableObject.GetComponent<Rigidbody>().isKinematic = true;
						hitDistance = hit.distance;
						hitPointOffset = interactableObject.transform.position - hit.point;
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
