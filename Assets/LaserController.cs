using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

	public GameObject particleSystemHit;

	void Start(){
	}

	void FixedUpdate(){
	}

	void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag == "Interior (Collider)"){
			GameObject particles = Instantiate(particleSystemHit);
			particles.transform.position = transform.position;

            Destroy(gameObject);
			Destroy(particles, 1.0f);
        }
    }
}
