using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

	public GameObject[] particleSystemHits;

	void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Laser Collider"){
			for(int i = 0; i < particleSystemHits.Length; ++i){
				GameObject particles = Instantiate(particleSystemHits[i]);
				particles.transform.position = transform.position;

				Destroy(particles, 2.0f);
				Destroy(this.gameObject);
			}
        }
    }
}
