using UnityEngine;
using System.Collections;

public class FirstPersonMouseFixes : MonoBehaviour {

	public GameObject laserBolt;
	public GameObject ejectionPoint;
	public GameObject laserKernel;
	public GameObject laserFlash;
	public bool hasWeapon;
	public float weaponCooldown;
	public float projectileSpeed;

	private bool lockedAndHidden;
	private float lastShotTime;

	void Start () {
		lockedAndHidden = true;
		setCursorState();
	}

	void FixedUpdate(){
		// TODO still a little buggy
		if(Input.GetKeyDown(KeyCode.Escape)){
			lockedAndHidden = !lockedAndHidden;
			setCursorState();
		}

		if(Input.GetMouseButton(0) && hasWeapon && Time.time - lastShotTime > weaponCooldown){
			GameObject bolt = Instantiate(laserBolt);
			bolt.transform.rotation = ejectionPoint.transform.rotation;
			bolt.transform.position = ejectionPoint.transform.position;

			if(laserKernel){
				GameObject kernel = Instantiate(laserKernel);
				kernel.transform.position = ejectionPoint.transform.position;
				kernel.transform.parent = ejectionPoint.transform;
				kernel.transform.eulerAngles = new Vector3(
					Random.Range(0.0f, 360.0f),
					Random.Range(0.0f, 360.0f),
					Random.Range(0.0f, 360.0f)
				);
			}

			// laserFlash
			if(laserFlash){
				GameObject flash = Instantiate(laserFlash);
				flash.transform.position = ejectionPoint.transform.position;
				flash.transform.parent = ejectionPoint.transform;
			}

			lastShotTime = Time.time;

			bolt.GetComponent<Rigidbody>().velocity = bolt.transform.right * projectileSpeed;
			Destroy(bolt, 2.0f);
		}
	}

	void setCursorState(){
		Cursor.visible = !lockedAndHidden;
		Cursor.lockState = lockedAndHidden ? CursorLockMode.Locked : CursorLockMode.Confined;
	}
}
