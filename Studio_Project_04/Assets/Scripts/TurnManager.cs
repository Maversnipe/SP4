using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : GenericSingleton<TurnManager> {
	// List of the units in battle
		// Identitfied by their 
	private Queue<int> _queueOfUnits = new Queue<int>();
	private Units currUnit;

	// Determine if it is player's turn or not
	private bool PlayerTurn = false;

	// This is called whenever the player starts a battle
	public void StartBattle()
	{
		List<Units> listOfUnits = UnitManager.Instance.GetAIList ();
		if (listOfUnits.Count == 0)
			return;

		// Sort List based on each unit's initiative
		// But if same initiative, sort by ID
		listOfUnits = listOfUnits.OrderBy(x => x.GetInitiative()).ThenBy(x => x.GetID()).ToList ();

		for(int i = 0; i < listOfUnits.Count; ++i)
		{
			Units theUnit = listOfUnits[i];

			// Push the unit into the queue
			_queueOfUnits.Enqueue (theUnit.GetID ());

		}

		// Player will always start first for each battle
		EnterPlayerTurn ();

		// Add player to back of queue list
		// Player is represented as -1 in the queue
		_queueOfUnits.Enqueue(-1);
	}

	// Update is called once per frame
	void Update () {
		// Enter Player Update only if it is Player's turn
		if(PlayerTurn)
		{
			// Update Player
			PlayerManager.Instance.UpdatePlayerUnits();
		}
		else
		{
			// Update Enemy
		}
	}

	// Enter Player's turn
	public void EnterPlayerTurn()
	{
		// Set player's turn to true
		PlayerTurn = true;
		// Set currUnit to null so that can switch to different unit's
		// according to which unit player clicks on
		currUnit = null;

		// Set to not be able to move unit
		PlayerManager.Instance.SetAbleToMove (false);
		// Set to not be able to attack
		PlayerManager.Instance.SetAbleToAttack (false);
		// Set stopped moving to false
		PlayerManager.Instance.SetStopMoving (false);

		// Center the camera into the middle of the Grid
		CameraReset ();
	}

	// Exit Player's Turn
	public void ExitPlayerTurn()
	{
		// Set player's turn to false
		PlayerTurn = false;

		// Set the selected unit to null
		PlayerManager.Instance.SetSelectedUnit (null);
	
		// Set to not be able to move unit
		PlayerManager.Instance.SetAbleToMove (false);
		// Set to not be able to attack
		PlayerManager.Instance.SetAbleToAttack (false);
		// Set stopped moving to false
		PlayerManager.Instance.SetStopMoving (false);

		// Start Next AI's Turn
		NextTurn();
	}

	// Set the next unit's turn
	public void NextTurn()
	{		
		// Get the next turn's unit ID
		int Num = _queueOfUnits.Dequeue ();
		// Put next turn's unit ID at back of queue
		_queueOfUnits.Enqueue (Num);

		// Check if the next unit is supposed to be player
		if (Num != -1)
		{
			// Set next turn's unit
			currUnit = UnitManager.Instance.GetUnit (Num);
			// Start next turn's unit's turn
			currUnit.TurnStart ();
		}
		else
		{
			// Start Player's Turn
			EnterPlayerTurn ();
		}
	}

	void CameraReset()
	{
		Camera.main.transform.position = new Vector3 (GridSystem.Instance.GetWidth () / 2.0f, 
			Camera.main.transform.position.y, GridSystem.Instance.GetHeight () / 2.0f); 
	}

	// Set & Get Curr Unit
	public Units GetCurrUnit() {return currUnit;}
	public void SetCurrUnit(Units _nextUnit) {currUnit = _nextUnit;}

	// Set & Get Player's Turn
	public bool IsPlayerTurn() {return PlayerTurn;}
	public void SetPlayerTurn(bool _turn) {PlayerTurn = _turn;}
}
