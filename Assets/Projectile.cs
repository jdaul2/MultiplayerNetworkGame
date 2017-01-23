using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

	private float speed = 1000f;

	[SyncVar]
	public float sv_Direction = 0f;
	[SyncVar]
	public Vector3 sv_Velocity = Vector3.zero;

	// Use this for initialization

	void Start(){
		transform.GetComponent<Rigidbody>().AddForce(sv_Velocity,ForceMode.Impulse);
		transform.GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Cos(Mathf.Deg2Rad * sv_Direction)*speed*Time.deltaTime,0f,-1f*Mathf.Sin(Mathf.Deg2Rad * sv_Direction)*speed*Time.deltaTime),ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update(){
		if(!isServer)return;
	}
}
