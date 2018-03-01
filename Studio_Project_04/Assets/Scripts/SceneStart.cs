using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour {

    public GameObject inv;
    public GameObject shop;
    public GameObject status;

    // Use this for initialization
    void Start () {
        inv = GameObject.FindWithTag("Inventory");
        shop = GameObject.Find("Shop");
        status = GameObject.Find("StatusMenu");

        inv.SetActive(false);
        shop.SetActive(false);
        status.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
