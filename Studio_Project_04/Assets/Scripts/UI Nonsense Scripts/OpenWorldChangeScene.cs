using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenWorldChangeScene : MonoBehaviour {

	public Image Fade;

	public string SceneName;

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
				SceneManager.LoadScene (SceneName);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.name == "Player") {
			FadeIn = false;
		}
	}
}
