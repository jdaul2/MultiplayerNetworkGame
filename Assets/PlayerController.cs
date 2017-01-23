using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	float speed = 5000f;
	float aimSpeed = .2f;
	float cooldown = 0f;
	float fireRate = .065f;
	GameObject weapon;

	// Use this for initialization
	void Start () {
		weapon = transform.GetChild(0).gameObject;
		weapon.transform.GetChild(0).gameObject.SetActive(false);
	}

	public override void OnStartLocalPlayer(){
		Camera.main.transform.SetParent(transform.FindChild("CameraPivot"));
		Camera.main.transform.localPosition = Vector3.zero;
		Camera.main.transform.localEulerAngles = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isLocalPlayer)return;

		cooldown += Time.deltaTime;

		if(Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical")!=0){
			transform.GetComponent<Rigidbody>().AddForce(new Vector3(Input.GetAxis("Horizontal")*speed*Time.deltaTime,0f,Input.GetAxis("Vertical")*speed*Time.deltaTime));
		}

		if(!(Input.GetAxis("XBox-RightAnalogX")==0f && Input.GetAxis("XBox-RightAnalogY")==0f)){
			if(!weapon.transform.GetChild(0).gameObject.activeInHierarchy){
				CmdSetActive(true);
			}
			//weapon.transform.GetChild(0).gameObject.SetActive(true);
			weapon.transform.localEulerAngles = new Vector3(0f,Mathf.LerpAngle(weapon.transform.localEulerAngles.y,-90f+Mathf.Atan2(Input.GetAxis("XBox-RightAnalogX"),-1*Input.GetAxis("XBox-RightAnalogY"))*Mathf.Rad2Deg,aimSpeed),0f);
		}
		else{
			if(weapon.transform.GetChild(0).gameObject.activeInHierarchy){
				CmdSetActive(false);
			}
			//weapon.transform.GetChild(0).gameObject.SetActive(false);
		}
		if(Input.GetAxis("XBox-RTrigger")>=1){
			if(cooldown>=fireRate){
				CmdSpawn(weapon.transform.parent.GetComponent<Rigidbody>().velocity, weapon.transform.localEulerAngles.y);
				cooldown = 0;
				GamePad.SetVibration(PlayerIndex.One,1f,1f);
			}
			else{
				GamePad.SetVibration(PlayerIndex.One,0f,0f);
			}
		}
		else{
			GamePad.SetVibration(PlayerIndex.One,0f,0f);
		}
	}

	[Command]
	void CmdSpawn(Vector3 velocity, float direction){
		Debug.Log("CmdSpawn");

		GameObject obj = GameObject.Instantiate(Resources.Load("Projectile"),weapon.transform.GetChild(1).transform.position,Quaternion.identity) as GameObject;

		obj.GetComponent<Projectile>().sv_Velocity = velocity; //  weapon.transform.parent.GetComponent<Rigidbody>().velocity
		obj.GetComponent<Projectile>().sv_Direction = direction; // weapon.transform.localEulerAngles.y;

		NetworkServer.Spawn(obj);
		Destroy(obj,2.0f);
	}

	[Command]
	void CmdSetActive(bool active){
		Debug.Log("CmdSetActive");

		RpcSetActive(active);
	}

	[ClientRpc]
	void RpcSetActive(bool active){
		if(weapon!=null)
		weapon.transform.GetChild(0).gameObject.SetActive(active);
	}

	void OnDisable(){
		if(isLocalPlayer){
			GamePad.SetVibration(PlayerIndex.One,0f,0f);
		}
	}
}
