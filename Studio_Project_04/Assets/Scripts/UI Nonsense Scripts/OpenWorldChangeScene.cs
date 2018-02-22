using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenWorldChangeScene : MonoBehaviour {

	public GameObject Fade;

	public string SceneName;

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
				SceneManager.LoadScene(SceneName);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.name == "Player") {
			Fade.GetComponent<OpenWorldFade>().SetFade(false);
		}
	}
}
