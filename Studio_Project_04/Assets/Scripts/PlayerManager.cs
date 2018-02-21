using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : GenericSingleton<PlayerManager> {
	// Determine if can move selected unit
	private bool ableToMove;
	// Determine if can use selected unit top attack
	private bool ableToAttack;
	// Determine if unit is moving
	private bool isMoving;
	// Represents the selected Unit
	private Players selectedPlayer;

	// Use this for initialization
	void Start ()
	{
		// Set to not be able to move unit
		ableToMove = false;
		// Set to not be able to attack
		ableToAttack = false;
		// Set stopped moving to false
		isMoving = false;
		// Set the selected unit to NULL first
		selectedPlayer =  null;
	}

	// Update Player during Player's turn
	public void UpdatePlayerUnits ()
	{
		// Exit function if it is not player's turn
		if (!TurnManager.Instance.IsPlayerTurn ())
			return;
		// Check if there is a unit selected
		if (selectedPlayer)
		{
			// Update the unit's heathbar image
			selectedPlayer.GetComponent<UnitVariables>().UpdateHealthBar();

			// Only update menu when selected unit is not null
			UpdateMenu();

			// Update the unit's movement
		}

	}

	// If Player selects another unit
	public void ChangeUnit(Players newUnit)
	{
		if (selectedPlayer)
		{
			// To reset the values for the unit
			selectedPlayer.TurnEnd ();
		}
		// Set to the new selected unit
		selectedPlayer = newUnit;
		if (selectedPlayer)
		{
			// Init the new selected unit
			selectedPlayer.TurnStart ();
		}

		// Set to not be able to move unit
		ableToMove = false;
		// Set to not be able to attack
		ableToAttack = false;
		// Set unit is moving to false
		isMoving = false;
		// Set for menu to be open
		if (selectedPlayer)
		{
			selectedPlayer.menuOpen = true;
		}
	}

	// Update the menu for each unit
	public void UpdateMenu()
	{
		// Only render the unit's menu if no action is selected
		// And if the unit is not moving
		if(selectedPlayer.menuOpen && !ableToAttack && !isMoving)
		{
			// This makes the Canvas in the unit to be active
			selectedPlayer.transform.GetChild (0).gameObject.SetActive (true);

		}
		else if(!selectedPlayer.menuOpen || isMoving || ableToAttack)
		{
			// This makes the Canvas in the unit to be inactive
			selectedPlayer.transform.GetChild (0).gameObject.SetActive (false);
		}

		// Check for deselection at the end
		DeselectActions ();
	}

	// For deselecting an action
	public void DeselectActions()
	{
		if (!ableToMove && !ableToAttack)
		{ // To deselect the selected unit
			if (Input.GetMouseButtonDown(1))
			{
				selectedPlayer.transform.GetChild (0).gameObject.SetActive (false);
				selectedPlayer.menuOpen = false;
				selectedPlayer.TurnEnd ();
				selectedPlayer = null;
			}
		}
		if (ableToMove)
		{ // If move is clicked, you can deselect move
			if (Input.GetMouseButtonDown(1))
			{
				selectedPlayer.transform.GetChild (0).gameObject.SetActive (true);
				selectedPlayer.menuOpen = true;
				isMoving = true;
				ableToMove = false;
			}
		}
		if (ableToAttack)
		{ // If attack is clicked, you can deselect attack
			if (Input.GetMouseButtonDown(1))
			{
				selectedPlayer.transform.GetChild (0).gameObject.SetActive (true);
				selectedPlayer.menuOpen = true;
				ableToAttack = false;
			}
		}
	}

	// Set the selected unit to be able to move
	public void StartMoving()
	{
		// Check if unit is available and if unit can move
		if(selectedPlayer && !ableToMove)
		{
			isMoving = true;
			ableToMove = true;
		}
	}

	// Set the selected unit to be able to attack
	public void StartAttacking()
	{
		// Check if unit is available and if unit can attack
		if(selectedPlayer && !ableToAttack)
		{
			ableToAttack = true;
		}
	}

	// Skip current turn
	public void SkipTurn()
	{
		// Check if unit is available and if unit can move
		if(selectedPlayer)
		{
			selectedPlayer.TurnEnd ();
			TurnManager.Instance.ExitPlayerTurn ();
		}
	}

	// Set & Get Selected Unit
	public Players GetSelectedUnit() {return selectedPlayer;}
	public void SetSelectedUnit(Players _nextPlayerUnit) {selectedPlayer = _nextPlayerUnit;}

	// Set & Get Unit Can Move
	public bool GetAbleToMove() {return ableToMove;}
	public void SetAbleToMove(bool _canMove) {ableToMove = _canMove;}

	// Set & Get Unit Can Attack
	public bool GetAbleToAttack() {return ableToAttack;}
	public void SetAbleToAttack(bool _canAttack) {ableToAttack = _canAttack;}

	// Set & Get Unit Stop Moving
	public bool GetIsMoving() {return isMoving;}
	public void SetIsMoving(bool _stopMove) {isMoving = _stopMove;}

	// Calculation of Damage Value for attacking
	public int CalculateDamage(Players player, AI enemy)
	{
		Weapon weapon = player.GetStats ()._weapon;
		Armor armor = enemy.GetStats ()._armor;
		int damageDeal = 1;

		if (weapon.Type == "Slash")
		{
			// Strong against
			if (armor.Type == "Light")
			{
				damageDeal = (int)Mathf.Max(1, (weapon.Attack - armor.Defence) * 1.5f);
			}
			// Weak against
			else if (armor.Type == "Heavy")
			{
				damageDeal = (int)Mathf.Max(1, (weapon.Attack - armor.Defence) * 0.5f);
			}
			// Normal
			else
			{
				damageDeal = Mathf.Max(1, weapon.Attack - armor.Defence);
			}
		}
		else if (weapon.Type == "Pierce")
		{
			// Strong against
			if (armor.Type == "Medium")
			{
				damageDeal = (int)Mathf.Max(1, (weapon.Attack - armor.Defence) * 1.5f);
			}
			// Weak against
			else if (armor.Type == "Light")
			{
				damageDeal = (int)Mathf.Max(1, (weapon.Attack - armor.Defence) * 0.5f);
			}
			// Normal
			else
			{
				damageDeal = Mathf.Max(1, weapon.Attack - armor.Defence);
			}
		}
		else if (weapon.Type == "Blunt")
		{
			// Strong against
			if (armor.Type == "Heavy")
			{
				damageDeal = (int)Mathf.Max(1, (weapon.Attack - armor.Defence) * 1.5f);
			}
			// Weak against
			else if (armor.Type == "Medium")
			{
				damageDeal = (int)Mathf.Max(1, (weapon.Attack - armor.Defence) * 0.5f);
			}
			// Normal
			else
			{
				damageDeal = Mathf.Max(1, weapon.Attack - armor.Defence);
			}
		}

		return damageDeal;
	}
}