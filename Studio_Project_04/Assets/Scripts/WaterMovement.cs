using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour {

	[SerializeField]
	private float scrollSpeedX;

	[SerializeField]
	private float scrollSpeedY;

	private Renderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		float offsetX = Time.time * scrollSpeedX;
		float offsetY = Time.time * scrollSpeedY;
		rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX,offsetY));
	}
}
