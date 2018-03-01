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
	private GameObject Indication;
	// Player Count
	private int playerCount;

	// Player's current quest number
	private int currQuest;

	// Breadth First Search Stuff
	private bool[,] visited;
	private List<Nodes> selectableNodes = new List<Nodes> ();

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
		// Set player's current quest
		currQuest = 0;
		// Set player count to 0
		playerCount = 0;
		// Indication ring
		Indication = GameObject.FindGameObjectWithTag ("Indication");
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
			// Indication ring position to selected player unit
			Indication.transform.GetChild (0).gameObject.SetActive (true);
			Indication.transform.GetChild (0).transform.position = new Vector3(selectedPlayer.transform.position.x, 
				Indication.transform.GetChild (0).transform.position.y, selectedPlayer.transform.position.z);

			// Update the unit's heathbar image
			selectedPlayer.GetComponent<UnitVariables>().UpdateHealthBar();

			// Update Unit Info Window
			selectedPlayer.GetComponent<UnitVariables> ().UpdateUnitInfo ();

			// Only update menu when selected unit is not null
			UpdateMenu();
		}
		else // De-Spawn Indication ring
			Indication.transform.GetChild (0).gameObject.SetActive (false);
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
	}

	// Update the menu for each unit
	public void UpdateMenu()
	{		
		// Only render the unit's menu if no action is selected
		// And if the unit is not moving
		if(!ableToAttack && !isMoving && selectedPlayer.GetStats ().AP > 0)
		{ // If player still selecting menu options
			// Find GO with ActionMenu tag
			GameObject menu = GameObject.FindGameObjectWithTag ("ActionMenu");
			// Make GO's child active, which makes the menu appear
			menu.transform.GetChild(0).gameObject.SetActive (true);

			// Get the cancel button gameobject
			GameObject cancelButton = GameObject.FindGameObjectWithTag ("CancelButtonNotSelectable");
			// Set cancel button to active
			cancelButton.transform.GetChild(0).gameObject.SetActive (true);

			// Checks if player's HP is at it's max
			if (selectedPlayer.GetStats ().HP == selectedPlayer.GetStats ().startHP)
			{
				// Find GO with RestButton tag
				GameObject restButton = GameObject.FindGameObjectWithTag ("RestButton");
				// Set rest button to false
				restButton.transform.GetChild (0).gameObject.SetActive (false);

				// Find GO with RestButtonNotSelectable tag
				restButton = GameObject.FindGameObjectWithTag ("RestButtonNotSelectable");
				// Set RestButtonNotSelectable to true
				restButton.transform.GetChild (0).gameObject.SetActive (true);
			} 
			else
			{
				// Find GO with RestButton tag
				GameObject restButton = GameObject.FindGameObjectWithTag ("RestButton");
				// Set rest button to true
				restButton.transform.GetChild (0).gameObject.SetActive (true);

				// Find GO with RestButtonNotSelectable tag
				restButton = GameObject.FindGameObjectWithTag ("RestButtonNotSelectable");
				// Set RestButtonNotSelectable to false
				restButton.transform.GetChild (0).gameObject.SetActive (false);
			}
		}
		else if(isMoving || ableToAttack)
		{ // If player already selected from menu options
			// This makes the Canvas in the unit to be inactive
			// Find GO with ActionMenu tag
			GameObject menu = GameObject.FindGameObjectWithTag ("ActionMenu");
			// Make GO's child active, which makes the menu appear
			menu.transform.GetChild(0).gameObject.SetActive (false);

			// Get the cancel button gameobject
			GameObject cancelButton = GameObject.FindGameObjectWithTag ("CancelButton");
			// Set cancel button to active
			cancelButton.transform.GetChild(0).gameObject.SetActive (true);
		}
	}

	// For deselecting an action
	public void DeselectActions()
	{
		if (ableToMove)
		{ // If move is clicked, you can deselect move
			// Find GO with ActionMenu tag
			GameObject menu = GameObject.FindGameObjectWithTag ("ActionMenu");
			// Make GO's child active, which makes the menu appear
			menu.transform.GetChild (0).gameObject.SetActive (true);

			isMoving = false;
			ableToMove = false;
			RemoveSelectable ();
		}
		if (ableToAttack)
		{ // If attack is clicked, you can deselect attack
			// Find GO with ActionMenu tag
			GameObject menu = GameObject.FindGameObjectWithTag ("ActionMenu");
			// Make GO's child active, which makes the menu appear
			menu.transform.GetChild (0).gameObject.SetActive (true);
			// Set ableToAttack to false
			ableToAttack = false;
		}

		// Get the cancel button gameobject
		GameObject cancelButton = GameObject.FindGameObjectWithTag ("CancelButton");
		// Set cancel button to not active
		cancelButton.transform.GetChild(0).gameObject.SetActive (false);

		// Get the cancel button gameobject
		cancelButton = GameObject.FindGameObjectWithTag ("CancelButtonNotSelectable");
		// Set cancel button to not active
		cancelButton.transform.GetChild(0).gameObject.SetActive (true);
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

			// Get the cancel button gameobject
			GameObject cancelButton = GameObject.FindGameObjectWithTag ("CancelButtonNotSelectable");
			// Set cancel button to active
			cancelButton.transform.GetChild(0).gameObject.SetActive (false);

			// Get the cancel button gameobject
			cancelButton = GameObject.FindGameObjectWithTag ("CancelButton");
			// Set cancel button to active
			cancelButton.transform.GetChild(0).gameObject.SetActive (true);
		}
	}

	// Set the selected unit to be able to attack
	public void StartAttacking()
	{
		// Check if unit is available and if unit can attack
		if(selectedPlayer && !ableToAttack)
		{
			ableToAttack = true;

			// Get the cancel button gameobject
			GameObject cancelButton = GameObject.FindGameObjectWithTag ("CancelButtonNotSelectable");
			// Set cancel button to active
			cancelButton.transform.GetChild(0).gameObject.SetActive (false);

			// Get the cancel button gameobject
			cancelButton = GameObject.FindGameObjectWithTag ("CancelButton");
			// Set cancel button to active
			cancelButton.transform.GetChild(0).gameObject.SetActive (true);
		}
	}

	// Skip current turn
	public void SkipTurn()
	{
		// Remove selectable nodes
		if(selectableNodes.Count > 0)
		{
			RemoveSelectable ();
		}
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

	// Player rest
	public void RestTurn()
	{
		// Get the remaining AP
		int hpAmtToAdd = selectedPlayer.GetStats().AP;
		// Set remaining AP in unit to 0
		selectedPlayer.GetStats ().AP = 0;
		// Dividing the amount of HP to add by 2
		hpAmtToAdd /= 2;
		// Checks if the amount to add is less than 1
		if (hpAmtToAdd < 1)
		{
			// If it is, set the amount to add to 1
				// This ensures that player will always receive at least 1HP
			hpAmtToAdd = 1;
		}
		// Add to the amount of HP
		selectedPlayer.GetStats ().HP += hpAmtToAdd;
		// Checks if the HP amount is more than the max HP
		if(selectedPlayer.GetStats ().HP > selectedPlayer.GetStats ().startHP)
		{
			// If the HP is more than the max HP, set the HP to max HP
				// This ensures that the player will not have more than the max health
			selectedPlayer.GetStats ().HP = selectedPlayer.GetStats ().startHP;
		}

		// Deselect unit
		ChangeUnit (null);
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

	// Set & Get Player's Current Quest Number
	public int GetCurrQuest() {return currQuest;}
	public void SetCurrQuest(int _currQuest) {currQuest = _currQuest;}

	// Set & Get Player Count
	public int GetPlayerCount() {return playerCount;}
	public void SetPlayerCount(int _count) {playerCount = _count;}

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
			if (temp.GetDist () < selectedPlayer.GetStats ().AP)
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