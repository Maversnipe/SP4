using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : GenericSingleton<UnitManager>
{
	// Units
	private List<Units> AIUnitList = new List<Units>();
	private List<Units> PlayerUnitList = new List<Units>();


	void Awake ()
	{
	}

	// Add the unit into the list using GameObject
	public void AddUnit(GameObject GO)
	{
		Units newUnit = GO.GetComponent <Units> ();
		if(newUnit)
		{
			if (newUnit.IsPlayable ())
			{
				PlayerUnitList.Add (newUnit);
			} 
			else
			{
				AIUnitList.Add (newUnit);
			}
		}
	}

	// Add the unit into the list using Units class
	public void AddUnit(Units newUnit)
	{
		if(newUnit)
		{
			if (newUnit.IsPlayable ())
			{
				Debug.Log ("Playable");
				PlayerUnitList.Add (newUnit);
			} 
			else
			{
				Debug.Log ("Not Playable");
				AIUnitList.Add (newUnit);
			}
		}
	}

	// Get the currently selected unit
	public Units GetUnit (int _unitID)
	{
		foreach (var GO in PlayerUnitList)
		{
			if (GO.GetID () == _unitID)
			{
				return GO;
			}
		}
		return null;
	}

	// Get the List of AI Units
	public List<Units> GetAIList() {return AIUnitList;}

	// Get the List of Player Units
	public List<Units> GetPlayerList() {return PlayerUnitList;}

    void Update()
    {
       
    }
}
