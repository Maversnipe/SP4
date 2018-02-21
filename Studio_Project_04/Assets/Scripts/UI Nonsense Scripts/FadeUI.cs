using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour {

	public float DecayRate;

	private Color Temp;

	// Use this for initialization
	void Start () {
		Temp = GetComponent<SpriteRenderer> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		if (Temp.a > 0) {
			Temp.a -= DecayRate;
		} else {
			Destroy (this.gameObject);
		}

		GetComponent<SpriteRenderer> ().color = Temp;
	}
}
