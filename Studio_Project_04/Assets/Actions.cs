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
		if (unitmanager.GetUnitToDoActions () != null && !unitmanager.AbleToMove)
		{
			Debug.Log ("Started moving.");
			unitmanager.AbleToMove = true;
		}
	}

	// Skip current turn
	public void SkipTurn ()
	{
		if (unitmanager.GetUnitToDoActions () != null && !unitmanager.AbleToChangeUnit)
		{
			Debug.Log ("Skipped turn.");
			unitmanager.AbleToChangeUnit = true;
			unitmanager.SetUnitToDoActions (null);
		}
	}
}
