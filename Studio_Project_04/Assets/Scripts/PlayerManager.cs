using System.Collections;
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

			selectableNodes.Add (temp);

			temp.SetSelectable (true);
			temp.ChangeColour ();

			// Do not check tiles if the dist of furthest tile is more than max dist
			if (temp.GetDist () < 3)
			{
				// Checks Tile Above
				if (temp.GetZIndex () + 1 <= GridSystem.Instance.GetColumn () - 1
				  && !visited [temp.GetXIndex (), temp.GetZIndex () + 1])
				{
					Nodes tempUp = GridSystem.Instance.GetNode (temp.GetXIndex (), temp.GetZIndex () + 1);
					tempUp.SetDist (1 + temp.GetDist ());
					tempUp.SetSelectable (true);
					tempUp.ChangeColour ();
					tempUp.SetParent (temp);
					visited [tempUp.GetXIndex (), tempUp.GetZIndex ()] = true;
					theQueue.Enqueue (tempUp);
					selectableNodes.Add (tempUp);
				}

				// Checks Tile Left
				if (temp.GetXIndex () - 1 >= 0
					&& !visited [temp.GetXIndex () - 1, temp.GetZIndex ()])
				{
					Nodes tempLeft = GridSystem.Instance.GetNode (temp.GetXIndex () - 1, temp.GetZIndex ());
					tempLeft.SetDist (1 + temp.GetDist ());
					tempLeft.SetSelectable (true);
					tempLeft.ChangeColour ();
					tempLeft.SetParent (temp);
					visited [tempLeft.GetXIndex (), tempLeft.GetZIndex ()] = true;
					theQueue.Enqueue (tempLeft);
					selectableNodes.Add (tempLeft);
				}

				// Checks Tile Below
				if (temp.GetZIndex () - 1 >= 0
					&& !visited [temp.GetXIndex (), temp.GetZIndex () - 1])
				{
					Nodes tempDown = GridSystem.Instance.GetNode (temp.GetXIndex (), temp.GetZIndex () - 1);
					tempDown.SetDist (1 + temp.GetDist ());
					tempDown.SetSelectable (true);
					tempDown.ChangeColour ();
					tempDown.SetParent (temp);
					visited [tempDown.GetXIndex (), tempDown.GetZIndex ()] = true;
					theQueue.Enqueue (tempDown);
					selectableNodes.Add (tempDown);
				}

				// Checks Tile Right
				if (temp.GetXIndex () + 1 <= GridSystem.Instance.GetRows () - 1
					&& !visited [temp.GetXIndex () + 1, temp.GetZIndex ()])
				{
					Nodes tempRight = GridSystem.Instance.GetNode (temp.GetXIndex () + 1, temp.GetZIndex ());
					tempRight.SetDist (1 + temp.GetDist ());
					tempRight.SetSelectable (true);
					tempRight.ChangeColour ();
					tempRight.SetParent (temp);
					visited [tempRight.GetXIndex (), tempRight.GetZIndex ()] = true;
					theQueue.Enqueue (tempRight);
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

		Debug.Log (selectedPlayer.GetPath ().Count);

		// Since the player's curr node's parent is null
		// Pop the path to remove the player's curr node from path
		selectedPlayer.GetPath().Pop ();
		selectedPlayer.GetCurrNode ().SetIsPath (false);

		Debug.Log (selectedPlayer.GetPath ().Count);

		// Make all the units unselectable as the player moves
		RemoveSelectable ();
	}
}