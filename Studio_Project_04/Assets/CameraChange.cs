using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour {

    bool freecam = false;
    private UnitManager unitmanager;
    // Use this for initialization
    void Start () {
        unitmanager = UnitManager.instance;
    }
	
	// Update is called once per frame
	void Update () {

        if (!freecam)
        {
            if (Input.GetKeyDown("c"))
            {
                freecam = true;
            }

        }
        if (freecam)
        {
            if (Input.GetKeyDown("v"))
            {
                freecam = false;
                transform.position = new Vector3(unitmanager.GetUnitToDoActions().transform.position.x, transform.position.y, unitmanager.GetUnitToDoActions().transform.position.z);
            }
            if (Input.GetKey("w"))
            {
                transform.position += transform.up * 10 * Time.deltaTime;
            }
            if (Input.GetKey("a"))
            {
                transform.position -= transform.right * 10 * Time.deltaTime;
            }
            if (Input.GetKey("s"))
            {
                transform.position -= transform.up * 10 * Time.deltaTime;
            }
            if (Input.GetKey("d"))
            {
                transform.position += transform.right * 10 * Time.deltaTime;
            }
        }
        
    }
}
