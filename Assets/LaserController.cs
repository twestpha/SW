using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

	public GameObject particleSystemHit;

	void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Interior (Collider)"){
			GameObject particles = Instantiate(particleSystemHit);
			particles.transform.position = transform.position;

			Destroy(particles, 1.0f);
			Destroy(this.gameObject);
        }
    }
}
