using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour {

	public Image Ref;

	private Color Temp;

	private bool Pressed;

	// Use this for initialization
	void Start () {
		Temp = Ref.color;

		Temp.a = 1;
	}
	
	// Update is called once per frame
	void Update () {
		Ref.color = Temp;
		if (Pressed == true) {
			if (Temp.a < 1) {
				Temp.a += 0.05f;
			} else {
				SceneManager.LoadScene ("SceneMenu");
			}
		} else {
			if (Temp.a > 0) {
				Temp.a -= 0.05f;
			}
		}
	}

	public void GoBackToMenu()
	{
		Pressed = true;
	}
}
