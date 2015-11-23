using UnityEngine;
using System.Collections;

public class ControlProbe : MonoBehaviour {

	public GameObject plane;
	public GameObject character;

	void FixedUpdate () {
		float offset = plane.transform.position.y - character.transform.position.y;

		Vector3 new_position = new Vector3(
			character.transform.position.x,
			plane.transform.position.y + offset,
			character.transform.position.z
		);

		transform.position = new_position;
	}
}
