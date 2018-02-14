using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
	// Reference to the UnitManager's instance
	private UnitManager unitmanager;

	void Start()
	{
		unitmanager = UnitManager.instance;	
	}

	// Start moving the selected unit
	public void StartMoving ()
	{
		// Check if unit is available and if unit can move
		if (unitmanager.GetUnitToDoActions () != null && !unitmanager.AbleToMove)
		{
			Debug.Log ("Started moving.");
			unitmanager.AbleToMove = true;
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
}
