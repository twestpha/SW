using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

	public GameObject hitExplosion;
	public GameObject hitSparks;
	public GameObject debris;
	public GameObject hitDecal;

	private Vector3 lastPosition;

	void FixedUpdate(){
		lastPosition = transform.position;
	}

	void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Laser Collider"){

			// Explosion effect
			GameObject explosion = Instantiate(hitExplosion);
			explosion.transform.position = transform.position;
			Destroy(explosion, 2.0f);

			// Sparks effect
			RaycastHit hit;
			Vector3 direction = transform.position - lastPosition;
			Vector3 reflection = direction;
			Vector3 normal = Vector3.zero;
			if(Physics.Raycast(lastPosition, direction, out hit)){
				 reflection = Vector3.Reflect(direction, hit.normal);
				 normal = hit.normal;
			}

			if(hitSparks){
				GameObject sparks = Instantiate(hitSparks);
				sparks.transform.position = transform.position;
				sparks.transform.LookAt(sparks.transform.position + reflection);
				Destroy(sparks, 2.0f);
			}

			if(hitDecal){
				GameObject decal = Instantiate(hitDecal);
				decal.transform.position = transform.position;
				decal.transform.LookAt(direction);
				decal.transform.Rotate(Vector3.right * 90.0f);

				Debug.DrawRay(decal.transform.position, decal.transform.right * 10.0f, Color.red);

				Destroy(decal, 10.0f);
			}

			Destroy(this.gameObject);
        }
    }
}
