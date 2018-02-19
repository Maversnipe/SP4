using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField]
	GameObject FollowTarget;

	Vector3 Pos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (FollowTarget != null) {
			Pos.x = FollowTarget.transform.position.x;
			Pos.z = FollowTarget.transform.position.z;
			Pos.y = this.gameObject.transform.position.y;

			this.gameObject.transform.position = Pos;
		}
	}
}
