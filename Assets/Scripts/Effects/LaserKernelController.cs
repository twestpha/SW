using UnityEngine;
using System.Collections;

public class LaserKernelController : MonoBehaviour {

	public float lifespan = 2;

	private float startTime;
	private float initialScale;

	void Start(){
		startTime = Time.time;
		initialScale = transform.localScale.x;
	}

	void FixedUpdate(){
		if(Time.time - startTime > lifespan){
			Destroy(this.gameObject);
		}

		float scale = 1.0f - ((Time.time - startTime) / lifespan);
		scale *= initialScale;
		transform.localScale = new Vector3(scale, scale, scale);
	}
}
