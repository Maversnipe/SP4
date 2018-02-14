using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
	// Reference to the UnitManager's instance
	private UnitManager unitmanager;

    public bool moving;
	void Start()
	{
		unitmanager = UnitManager.instance;	
	}

    public void StartAttack()
    {
        // Check if unit is available and if unit can move
        if (unitmanager.GetUnitToDoActions() != null && !unitmanager.AbleToAttack)
        {
            Debug.Log("Started attack.");
            unitmanager.AbleToAttack = true;
        }
    }

    // Start moving the selected unit
    public void StartMoving ()
	{
		// Check if unit is available and if unit can move
		if (unitmanager.GetUnitToDoActions () != null && !unitmanager.AbleToMove)
		{
			Debug.Log ("Started moving.");
            unitmanager.StoppedMoving = false;
			unitmanager.AbleToMove = true;
            moving = true;
		}
	}

	// Skip current turn
	public void SkipTurn ()
	{
		// Check if unit is available and if player can change to control another unit
		if (unitmanager.GetUnitToDoActions () != null && !unitmanager.AbleToChangeUnit)
		{
			Debug.Log ("Skipped turn.");
			unitmanager.AbleToChangeUnit = true;
			unitmanager.SetUnitToDoActions (null);
		}
	}

    void Update()
    {
        if(unitmanager.openMenu && !unitmanager.AbleToChangeUnit && !moving && !unitmanager.AbleToAttack)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else if(!unitmanager.openMenu || unitmanager.AbleToChangeUnit || moving || unitmanager.AbleToAttack)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        if (unitmanager.StoppedMoving)
        {
            moving = false;
        }

        if(!unitmanager.AbleToChangeUnit)
        {
            Vector3 temp = new Vector3(unitmanager.GetUnitToDoActions().transform.position.x, transform.position.y, unitmanager.GetUnitToDoActions().transform.position.z);
            transform.position = Camera.main.WorldToScreenPoint(temp);
        }
    }

}
