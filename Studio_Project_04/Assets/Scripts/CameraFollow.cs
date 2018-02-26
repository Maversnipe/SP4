using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField]
	GameObject FollowTarget = null;

	Vector3 Pos;

	Vector3 TempPos;

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

		TempPos = this.gameObject.transform.position;

		if (Input.GetAxis ("Mouse ScrollWheel") > 0f) {
			if (this.gameObject.transform.position.y > 15) {
				TempPos.y -= Input.GetAxis ("Mouse ScrollWheel") * 3;
				this.gameObject.transform.position = TempPos;
			}
		} else if (Input.GetAxis ("Mouse ScrollWheel") < 0f) {
			if (this.gameObject.transform.position.y < 30) {
				TempPos.y -= Input.GetAxis ("Mouse ScrollWheel") * 3;
				this.gameObject.transform.position = TempPos;
			}
		}
	}
}
