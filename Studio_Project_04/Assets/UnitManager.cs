using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	// Public Reference for other class to access UnitManager
	public static UnitManager instance;

	// Units
	public GameObject TestingPlayer;

	// Determine if can change selected unit
	public bool AbleToChangeUnit = true;
	// Determine if can move selected unit
	public bool AbleToMove = false;
	// Determine if can move selected unit
	public bool StoppedMoving = false;

	// A reference to the current selected unit
	private GameObject UnitToDoActions;

	// Taking care of Singleton for UnityManager
	void Awake ()
	{
		if (instance != null)
		{
			Debug.LogError ("More than one UnitManager in scene!");
			return;
		}
		instance = this;
	}

	// Get the currently selected unit
	public GameObject GetUnitToDoActions ()
	{
		return UnitToDoActions;
	}

	// Change the reference to current selected unit
	public void SetUnitToDoActions (GameObject Unit)
	{
		UnitToDoActions = Unit;
	}
}
