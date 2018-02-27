using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenWorldFade : MonoBehaviour {

	private bool Fade;
	private Color Temp;

	// Use this for initialization
	void Start () {
		Fade = false;
		Temp = GetComponent<Image> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		if (Fade) {
			if (Temp.a > 1) {
				Temp.a = 1;
			} else {
				Temp.a += 0.05f;
			}
		} else {
			if (Temp.a < 0) {
				Temp.a = 0;
			} else {
				Temp.a -= 0.05f;
			}
		}
		GetComponent<Image> ().color = Temp;
	}

	public void SetFade (bool n_new)
	{
		Fade = n_new;
	}

	public bool GetFade ()
	{
		return Fade;
	}

	public float GetAlpha ()
	{
		return Temp.a;
	}
}
