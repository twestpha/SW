using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

	public GameObject particleSystemHit;

	private Vector3 lastPosition;

	void FixedUpdate(){
		lastPosition = transform.position;
	}

	void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Interior (Collider)"){
			GameObject particles = Instantiate(particleSystemHit);
			particles.transform.position = transform.position;

			Destroy(particles, 1.0f);
			Destroy(this.gameObject);
        }
    }
}
