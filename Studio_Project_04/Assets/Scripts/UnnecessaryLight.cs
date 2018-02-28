using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnnecessaryLight : MonoBehaviour {

	float xRotation;
	bool GoBackX;
	float yRotation;

	// Use this for initialization
	void Start () {
		xRotation = this.gameObject.transform.eulerAngles.x;
		yRotation = this.gameObject.transform.eulerAngles.y;

		GoBackX = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (!GoBackX) {
			if (xRotation < 90) {
				xRotation += 0.001f;
			} else {
				GoBackX = true;
			}
		} else {
			if (xRotation > 72) {
				xRotation -= 0.5f;
			} else {
				GoBackX = false;
			}
		}

		yRotation += 0.05f;

		if (yRotation >= 360) {
			yRotation = 0;
		}

		Vector3 TempRot = new Vector3(xRotation,yRotation,0);

		this.transform.rotation = Quaternion.Euler(TempRot);
	}
}
