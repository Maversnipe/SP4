using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillingUI : MonoBehaviour {

	[SerializeField]
	float FillAmount;

	[SerializeField]
	float FillRate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Image> ().fillAmount < FillAmount) {
			GetComponent<Image> ().fillAmount += FillRate;
		}
	}
}
