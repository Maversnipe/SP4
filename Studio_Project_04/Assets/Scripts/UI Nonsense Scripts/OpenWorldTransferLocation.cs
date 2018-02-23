using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenWorldTransferLocation : MonoBehaviour {

	public GameObject Fade;

	public GameObject TransportLocation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay(Collider other) {
		if (other.name == "Player") {
			Fade.GetComponent<OpenWorldFade> ().SetFade (true);

			if (Fade.GetComponent<OpenWorldFade>().GetAlpha() >= 1) {
				other.transform.position = TransportLocation.transform.position;
				other.GetComponent<OpenControl> ().StopMoving ();
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.name == "Player") {
			Fade.GetComponent<OpenWorldFade>().SetFade(false);
		}
	}
}
