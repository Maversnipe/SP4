using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonScript : MonoBehaviour {

    [SerializeField]
    public GameObject inventory;
    [SerializeField]
    public GameObject status;

	// Use this for initialization
	void Start () {
		
	}

    public void openInventory()
    {
        inventory.SetActive(true);
    }

    public void openStatus()
    {
        status.SetActive(true);
    }
}
