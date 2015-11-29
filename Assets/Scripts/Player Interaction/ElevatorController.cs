using UnityEngine;
using System.Collections;

public class ElevatorController : MonoBehaviour {

	public float elevation;
	public ElevatorPosition elevatorPosition;
	public float speed;
	public GameObject elevatorObject;
	public GameObject upButton;
	public GameObject downButton;
	public GameObject player;

	private float RAYCAST_LENGTH = 10.0f;
	private bool moving;
	private int direction;
	bool playerInElevator;

	public enum ElevatorPosition {
		up, down
	};

	void Start () {
		if(elevatorPosition == ElevatorPosition.up){
			elevatorObject.transform.position += (Vector3.up * elevation);
		}
		playerInElevator = false;
	}

	void OnTriggerEnter(Collider collider){
		if(collider.gameObject == player){
			playerInElevator = true;
		}
	}

	void OnTriggerExit(Collider collider){
		if(collider.gameObject == player){
			playerInElevator = false;
		}
	}

	void Update () {
		getInteraction();
		moveElevator();

		if(playerInElevator){
			player.transform.parent = elevatorObject.transform;
		}
	}

	void getInteraction(){
		if(Input.GetMouseButtonDown(0) && playerInElevator){
			RaycastHit hit;
			Ray click_ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

			if(Physics.Raycast(click_ray, out hit, RAYCAST_LENGTH)){
				if(hit.collider.gameObject == upButton){
					direction = 1;
				} else if(hit.collider.gameObject == downButton){
					// print("Clicked down_button")
					direction = -1;
				}
			}
		}
	}

	void moveElevator(){
		if((direction == 1 && elevatorObject.transform.position.y != elevation) || (direction == -1 && elevatorObject.transform.position.y != 0.0f)){
			elevatorObject.transform.Translate(Vector3.up * direction * speed * Time.deltaTime);
			Vector3 clamped_position = elevatorObject.transform.position;
			clamped_position.y = Mathf.Clamp(elevatorObject.transform.position.y, 0.0f, elevation);
			elevatorObject.transform.position = clamped_position;
		}
	}
}
