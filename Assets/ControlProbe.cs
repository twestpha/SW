using UnityEngine;
using System.Collections;

public class ControlProbe : MonoBehaviour {

	public GameObject plane;
	public GameObject character;
	public float offset;
	public Direction directionFaced;

	public enum Direction{
		x, y, z
	}

	void Update () {
		if(directionFaced == Direction.x){
			offset = plane.transform.position.x - character.transform.position.x;

			Vector3 new_position = new Vector3(
				plane.transform.position.x + offset,
				character.transform.position.y,
				character.transform.position.z
			);

			transform.position = new_position;
		} else if(directionFaced == Direction.y){
			offset = plane.transform.position.y - character.transform.position.y;

			Vector3 new_position = new Vector3(
				character.transform.position.x,
				plane.transform.position.y + offset,
				character.transform.position.z
			);

			transform.position = new_position;
		} else if(directionFaced == Direction.z){
			offset = plane.transform.position.z - character.transform.position.z;

			Vector3 new_position = new Vector3(
				character.transform.position.x,
				character.transform.position.y,
				plane.transform.position.z + offset
			);

			transform.position = new_position;
		}


	}
}
