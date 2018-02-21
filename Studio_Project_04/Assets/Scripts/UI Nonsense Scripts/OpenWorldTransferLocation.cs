using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenWorldTransferLocation : MonoBehaviour {

	public Image Fade;

	public GameObject TransportLocation;

	private Color Temp;

	private bool FadeIn;

	// Use this for initialization
	void Start () {
		Temp = Fade.color;
		Temp.a = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Fade.color = Temp;

		if (FadeIn) {
			if (Temp.a < 1) {
				Temp.a += 0.05f;
			}
		}

		if (!FadeIn) {
			if (Temp.a > 0) {
				Temp.a -= 0.05f;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.name == "Player") {
			FadeIn = true;

			if (Temp.a >= 1) {
				other.transform.position = TransportLocation.transform.position;
				other.GetComponent<OpenControl> ().StopMoving ();
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.name == "Player") {
			FadeIn = false;
		}
	}
}
