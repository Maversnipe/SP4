﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : GenericSingleton<PlayerManager> {
	// Determine if can move selected unit
	private bool ableToMove;
	// Determine if can use selected unit top attack
	private bool ableToAttack;
	// Determine if unit is moving
	private bool isMoving;
	// Represents the selected Unit
	private Players selectedPlayer;

	// Breadth First Search Stuff
	public bool[,] visited;
	public List<Nodes> selectableNodes = new List<Nodes> ();

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
		// Init visited array
		visited = new bool[GridSystem.Instance.GetRows(), GridSystem.Instance.GetRows ()];
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

			// Update Unit Info Window
			selectedPlayer.GetComponent<UnitVariables> ().UpdateUnitInfo ();

			// Only update menu when selected unit is not null
			UpdateMenu();
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
		{ // If player still selecting menu options
			// This makes the Canvas in the unit to be active
			selectedPlayer.transform.GetChild (0).gameObject.SetActive (true);
		}
		else if(!selectedPlayer.menuOpen || isMoving || ableToAttack)
		{ // If player already selected from menu options
			// This makes the Canvas in the unit to be inactive
			selectedPlayer.transform.GetChild (0).gameObject.SetActive (false);
		}

		if(selectedPlayer.menuOpen)
		{
			// Get the cancel button gameobject
			GameObject cancelButton = GameObject.FindGameObjectWithTag ("CancelButton");
			// Set cancel button to active
			cancelButton.transform.GetChild (0).gameObject.SetActive (true);
		}
		else
		{
			// Get the cancel button gameobject
			GameObject cancelButton = GameObject.FindGameObjectWithTag ("CancelButton");
			// Set cancel button to Inactive
			cancelButton.transform.GetChild (0).gameObject.SetActive (false);
		}

	}

	// For deselecting an action
	public void DeselectActions()
	{
		if (ableToMove)
		{ // If move is clicked, you can deselect move
			selectedPlayer.transform.GetChild (0).gameObject.SetActive (true);
			selectedPlayer.menuOpen = true;
			isMoving = true;
			ableToMove = false;
			RemoveSelectable ();
		}
		if (ableToAttack)
		{ // If attack is clicked, you can deselect attack
			selectedPlayer.transform.GetChild (0).gameObject.SetActive (true);
			selectedPlayer.menuOpen = true;
			ableToAttack = false;
		}

		// Change player back to default color
		selectedPlayer.SetToDefaultColor ();

		// Close Action Menu & Unit Info Windows
		selectedPlayer.menuOpen = false;
		selectedPlayer.GetStats ().SetUnitInfoWindow (false);
		selectedPlayer.GetStats ().SetOpponentUnitInfoWindow (false);

		// Get the cancel button gameobject
		GameObject cancelButton = GameObject.FindGameObjectWithTag ("CancelButton");
		// Set cancel button to not active
		cancelButton.transform.GetChild (0).gameObject.SetActive (false);
	}

	// Set the selected unit to be able to move
	public void StartMoving()
	{
		// Check if unit is available and if unit can move
		if(selectedPlayer && !ableToMove)
		{
			isMoving = true;
			ableToMove = true;
			FindSelectableTiles ();
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
		// Check if unit is available
		if(selectedPlayer)
		{
			selectedPlayer.TurnEnd ();
		}
		TurnManager.Instance.ExitPlayerTurn ();

		// Find the end button gameobject
		GameObject endButton = GameObject.FindGameObjectWithTag ("EndTurnButton");
		// Set end button to not active
		endButton.transform.GetChild (0).gameObject.SetActive (false);
		// Get the action menu gameobject
		GameObject ActionMenu2 = GameObject.FindGameObjectWithTag ("ActionMenu2");
		// Set action menu to Inactive
		ActionMenu2.transform.GetChild (0).gameObject.SetActive (false);
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

	// Return calculated Damage Value for attacking - Need to pass in GameObjects of Attacker and Victim
	public int CalculateDamage(GameObject attacker, GameObject victim)
	{
		// Weapon of player and Armor of enemy informations
		Weapon weapon = attacker.GetComponent<UnitVariables>()._weapon;
		Armor armor = victim.GetComponent<UnitVariables>()._armor;

		// Damage Calculations
		int damageDeal = -1;
		int advantagedDamage = (int)Mathf.Max(1, (weapon.Attack - armor.Defence) * 1.5f);
		int disadvantagedDamage = (int)Mathf.Max(1, (weapon.Attack - armor.Defence) * 0.5f);
		int normalDamage = Mathf.Max(1, weapon.Attack - armor.Defence);

		switch (weapon.Type)
		{
		case "Slash":
			{
				switch (armor.Type) 
				{
				// Strong against
				case "Light":
					{
						damageDeal = advantagedDamage;
						break;
					}
				// Weak against
				case "Heavy":
					{
						damageDeal = disadvantagedDamage;
						break;
					}
				}
				break;
			}
		case "Pierce":
			{
				switch (armor.Type) 
				{
				// Strong against
				case "Medium":
					{
						damageDeal = advantagedDamage;
						break;
					}
				// Weak against
				case "Light":
					{
						damageDeal = disadvantagedDamage;
						break;
					}
				}
				break;
			}
		case "Blunt":
			{
				switch (armor.Type) {
				// Strong against
				case "Heavy":
					{
						damageDeal = advantagedDamage;
						break;
					}
				// Weak against
				case "Medium":
					{
						damageDeal = disadvantagedDamage;
						break;
					}
				}
				break;
			}
		}
		if(damageDeal == -1)
			damageDeal = normalDamage;
		return damageDeal;
	}
		
	// BFS for unit
	public void FindSelectableTiles()
	{
		Nodes currNode = selectedPlayer.GetCurrNode ();
		Queue<Nodes> theQueue = new Queue<Nodes>();

		// Set all visited to false
		Array.Clear(visited, 0, visited.Length);

		// Set curr nodde's parent to null
		currNode.SetParent (null);
		// Set Dist of curr node to 0
		currNode.SetDist(0);
		// Push curr node into the queue
		theQueue.Enqueue (currNode);
		// Set curr node as visited
		visited [currNode.GetXIndex(), currNode.GetZIndex()] = true;

		while(theQueue.Count > 0)
		{
			Nodes temp = theQueue.Dequeue ();

			// Do not check tiles if the dist of furthest tile is more than max dist
			if (temp.GetDist () < selectedPlayer.GetAP ())
			{
				// Get the nodes adjacent to the temp node
				Nodes tempUp = null; 
				Nodes tempLeft = null;
				Nodes tempDown = null;
				Nodes tempRight = null;

				if(temp.GetZIndex () + 1 <= GridSystem.Instance.GetColumn () - 1)
					tempUp = GridSystem.Instance.GetNode (temp.GetXIndex (), temp.GetZIndex () + 1);
				if(temp.GetXIndex () - 1 >= 0)
					tempLeft = GridSystem.Instance.GetNode (temp.GetXIndex () - 1, temp.GetZIndex ());
				if(temp.GetZIndex () - 1 >= 0)
					tempDown = GridSystem.Instance.GetNode (temp.GetXIndex (), temp.GetZIndex () - 1);
				if(temp.GetXIndex () + 1 <= GridSystem.Instance.GetRows () - 1)
					tempRight = GridSystem.Instance.GetNode (temp.GetXIndex () + 1, temp.GetZIndex ());

				// Checks Tile Above
				if (tempUp != null
					&& !visited [tempUp.GetXIndex (), tempUp.GetZIndex ()]
					&& tempUp.GetOccupied () == null)
				{
					// Set the current node's distance from starting node
					tempUp.SetDist (1 + temp.GetDist ());
					// Set this node to be selectable
					tempUp.SetSelectable (true);
					// Change this node's color
					tempUp.ChangeColour ();
					// Set the node's parent to be the previous node
					tempUp.SetParent (temp);
					// Set current node's visited to be true
					visited [tempUp.GetXIndex (), tempUp.GetZIndex ()] = true;
					// Add current node to the queue
					theQueue.Enqueue (tempUp);
					// Add this node to the list of selectable node
					selectableNodes.Add (tempUp);
				}

				// Checks Tile Left
				if (tempLeft != null
					&& !visited [tempLeft.GetXIndex (), tempLeft.GetZIndex ()]
					&& tempLeft.GetOccupied () == null)
				{
					// Set the current node's distance from starting node
					tempLeft.SetDist (1 + temp.GetDist ());
					// Set this node to be selectable
					tempLeft.SetSelectable (true);
					// Change this node's color
					tempLeft.ChangeColour ();
					// Set the node's parent to be the previous node
					tempLeft.SetParent (temp);
					// Set current node's visited to be true
					visited [tempLeft.GetXIndex (), tempLeft.GetZIndex ()] = true;
					// Add current node to the queue
					theQueue.Enqueue (tempLeft);
					// Add this node to the list of selectable node
					selectableNodes.Add (tempLeft);
				}

				// Checks Tile Below
				if (tempDown != null
					&& !visited [tempDown.GetXIndex (), tempDown.GetZIndex ()]
					&& tempDown.GetOccupied () == null)
				{
					// Set the current node's distance from starting node
					tempDown.SetDist (1 + temp.GetDist ());
					// Set this node to be selectable
					tempDown.SetSelectable (true);
					// Change this node's color
					tempDown.ChangeColour ();
					// Set the node's parent to be the previous node
					tempDown.SetParent (temp);
					// Set current node's visited to be true
					visited [tempDown.GetXIndex (), tempDown.GetZIndex ()] = true;
					// Add current node to the queue
					theQueue.Enqueue (tempDown);
					// Add this node to the list of selectable node
					selectableNodes.Add (tempDown);
				}

				// Checks Tile Right
				if (tempRight != null
					&& !visited [tempRight.GetXIndex (), tempRight.GetZIndex ()]
					&& tempRight.GetOccupied () == null)
				{
					// Set the current node's distance from starting node
					tempRight.SetDist (1 + temp.GetDist ());
					// Set this node to be selectable
					tempRight.SetSelectable (true);
					// Change this node's color
					tempRight.ChangeColour ();
					// Set the node's parent to be the previous node
					tempRight.SetParent (temp);
					// Set current node's visited to be true
					visited [tempRight.GetXIndex (), tempRight.GetZIndex ()] = true;
					// Add current node to the queue
					theQueue.Enqueue (tempRight);
					// Add this node to the list of selectable node
					selectableNodes.Add (tempRight);
				}
			}
		}
	}

	// Make all selectable nodes to be not selectable
	public void RemoveSelectable()
	{
		// Iterate through the list of selectable nodes
		foreach(Nodes theNode in selectableNodes)
		{
			// Set dist to 0
			theNode.SetDist (0);
			// Set the node's parent to be null
			theNode.SetParent (null);
			// Set the node to be not selectable
			theNode.SetSelectable (false);
			// Change the colour of node back to the normal colour
			theNode.ChangeColour ();
		}
		// Clear the selectable list
		selectableNodes.Clear ();
	}

	// Set the player's path
	public void SetPath(Nodes targetNode)
	{
		// Push node onto the path
		selectedPlayer.GetPath ().Push(targetNode);
		// Set that the node is on the player's path as true
		targetNode.SetIsPath (true);
		// Set targetNode tp be it's parent node
		targetNode = targetNode.GetParent ();

		// Iterate through all the parents until reaching the player
		while(targetNode != null)
		{
			// Push node onto the path
			selectedPlayer.GetPath ().Push(targetNode);
			// Set that the node is on the player's path as true
			targetNode.SetIsPath (true);
			// Set targetNode tp be it's parent node
			targetNode = targetNode.GetParent ();
		}

		// Since the player's curr node's parent is null
		// Pop the path to remove the player's curr node from path
		selectedPlayer.GetPath().Pop ();
		selectedPlayer.GetCurrNode ().SetIsPath (false);

		// Make all the units unselectable as the player moves
		RemoveSelectable ();
	}
}