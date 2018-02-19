using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : GenericSingleton<PlayerManager> {
	// Determine if can move selected unit
	private bool AbleToMove;
	// Determine if can use selected unit top attack
	private bool AbleToAttack;
	// Determine if can move selected unit
	private bool StoppedMoving;
	// Determine if menu can open
	private bool OpenMenu;
	// Represents the selected Unit
	private Units selectedUnit;
	// Represents the list of player units in the current battle
	List<Units> ThePlayers;

	// Use this for initialization
	void Start () 
	{
		// Set to not be able to move unit
		AbleToMove = false;
		// Set to not be able to attack
		AbleToAttack = false;
		// Set stopped moving to false
		StoppedMoving = false;
		// Set for menu to be closed
		OpenMenu = false;
		// Set the selected unit to NULL first
		selectedUnit = 	null;
	}
	
	// Update Player Units during Player's turn
	public void UpdatePlayerUnits () 
	{
		if (selectedUnit != null && !AbleToMove && !AbleToAttack)
		{ // To deselect the selected unit
			if (Input.GetMouseButtonDown(1))
			{
				selectedUnit.TurnEnd ();
				selectedUnit = null;
				OpenMenu = false;
			}
		}
		if (AbleToMove)
		{ // If move is clicked, you can deselect move 
			if (Input.GetMouseButtonDown(1))
			{
				StoppedMoving = true;
				AbleToMove = false;
				OpenMenu = true;
			}
		}
		if (AbleToAttack)
		{ // If attack is clicked, you can deselect attack
			if (Input.GetMouseButtonDown(1))
			{
				AbleToAttack = false;
				OpenMenu = true;
			}
		}
	}

	// If Player selects another unit
	public void ChangeUnit(Units newUnit)
	{
		if (selectedUnit)
		{
			// To reset the values for the unit
			selectedUnit.TurnEnd ();
		}
		// Set to the new selected unit
		selectedUnit = newUnit;
		if (selectedUnit)
		{
			// Init the new selected unit
			selectedUnit.TurnStart ();
		}

		// Set to not be able to move unit
		AbleToMove = false;
		// Set to not be able to attack
		AbleToAttack = false;
		// Set stopped moving to false
		StoppedMoving = false;
		// Set for menu to be open
		OpenMenu = true;
	}

	// Update the list of player units
	public void SetPlayerUnits()
	{
		// Set list to the new list
		ThePlayers = UnitManager.Instance.GetPlayerList ();
	}

	// Set & Get Selected Unit
	public Units GetSelectedUnit() {return selectedUnit;}
	public void SetSelectedUnit(Units _nextUnit) {selectedUnit = _nextUnit;}

	// Set & Get Unit Can Move
	public bool GetAbleToMove() {return AbleToMove;}
	public void SetAbleToMove(bool _canMove) {AbleToMove = _canMove;}

	// Set & Get Unit Can Attack
	public bool GetAbleToAttack() {return AbleToAttack;}
	public void SetAbleToAttack(bool _canAttack) {AbleToAttack = _canAttack;}

	// Set & Get Unit Stop Moving
	public bool GetStopMoving() {return StoppedMoving;}
	public void SetStopMoving(bool _stopMove) {StoppedMoving = _stopMove;}

	// Set & Get Unit Menu Open
	public bool GetOpenMenu() {return OpenMenu;}
	public void SetOpenMenu(bool _menu) {OpenMenu = _menu;}
}
