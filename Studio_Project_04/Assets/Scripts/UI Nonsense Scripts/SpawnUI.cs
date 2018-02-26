using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnUI : MonoBehaviour {

	[SerializeField]
	private Image Ref;

	[SerializeField]
	private float FillToSpawn;

	private bool Active;

	private Color Temp;

	// Use this for initialization
	void Start () {
		Active = false;

		Temp = this.GetComponent<Image> ().color;

		Temp.a = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Ref.fillAmount >= FillToSpawn) {
			Active = true;
		}

		this.GetComponent<Image> ().color = Temp;

		if (Active) {
			this.GetComponent<Image> ().enabled = true;

			if (Temp.a < 1) {
				Temp.a += 0.1f;
			}

			if (Temp.a > 1) {
				Temp.a = 1;
				this.GetComponent<Button> ().enabled = true;
			}
		} else {
			this.GetComponent<Button> ().enabled = false;
			this.GetComponent<Image> ().enabled = false;
		}
	}
}
