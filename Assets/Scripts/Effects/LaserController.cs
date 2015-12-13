using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

	public GameObject hitExplosion;
	public GameObject hitFlash;
	public GameObject hitSparks;
	public GameObject debris;
	public GameObject smoke;
	public GameObject[] hitDecals;

	private Vector3 lastPosition;

	void Start(){
		lastPosition = transform.position;
	}

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

			if(hitDecals.Length > 0){
				GameObject decal = Instantiate(hitDecals[Random.Range(0, hitDecals.Length)]);
				decal.transform.position = transform.position + (0.5f * normal);
				// decal.transform.position = lastPosition;
				decal.transform.LookAt(transform.position);

				Destroy(decal, 10.0f);
			}

			if(hitFlash){
				GameObject flash = Instantiate(hitFlash);
				flash.transform.position = transform.position;

				Destroy(flash, 0.1f);
			}

			if(smoke){
				GameObject newSmoke = Instantiate(smoke);
				newSmoke.transform.position = transform.position;

				Destroy(newSmoke, 20.0f);
			}

			Destroy(this.gameObject);
        }
    }
}
