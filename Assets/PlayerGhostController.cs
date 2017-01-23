using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using UnityEngine.Networking;

public class PlayerGhostController : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public override void OnStartLocalPlayer(){
//		Camera.main.transform.SetParent(transform.FindChild("CameraPivot"));
//		Camera.main.transform.localPosition = Vector3.zero;
//		Camera.main.transform.localEulerAngles = Vector3.zero;

		CmdSpawn();
	}

	// Update is called once per frame
	void Update () {
		if(!isLocalPlayer)return;

	}

	[Command]
	void CmdSpawn(){
		GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Player"),transform.position,transform.rotation);
		NetworkServer.Spawn(obj);
	}

	void OnDisable(){
	}

}
