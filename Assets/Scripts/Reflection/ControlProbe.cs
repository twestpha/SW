using UnityEngine;
using System.Collections;

public class ControlProbe : MonoBehaviour {

	public GameObject characterBody;
	public GameObject characterCamera;

	void FixedUpdate () {
		// Get distance to the floor
		RaycastHit hit;
		if(Physics.Raycast(characterBody.transform.position, Vector3.down, out hit)){
			float ground_height = hit.point.y;
			float distance_to_ground = characterCamera.transform.position.y - ground_height;

			Vector3 new_position = new Vector3(
				characterCamera.transform.position.x,
				hit.point.y - distance_to_ground,
				characterCamera.transform.position.z
			);

			transform.position = new_position;
		}
	}
}
