using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : GenericSingleton<TurnManager> {
	// List of the units in battle
		// Identitfied by their 
	private Queue<int> _queueOfUnits = new Queue<int>();
	private Units currUnit;

	// Determine if can change selected unit
	private bool AbleToChangeUnit = true;
	// Determine if can move selected unit
	private bool AbleToMove = false;
	private bool AbleToAttack = false;
	// Determine if can move selected unit
	private bool StoppedMoving = false;
	private bool OpenMenu = false;
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



//		int Num = _queueOfUnits.Dequeue ();
//		currUnit = UnitManager.Instance.GetUnit (_queueOfUnits.Dequeue ());
//
//		_queueOfUnits.Enqueue (currUnit.GetID ());
//		currUnit.TurnStart ();
	}

	// Update is called once per frame
	void Update () {
		// Enter Player Update only if it is Player's turn
		if(PlayerTurn)
		{
			PlayerUpdate ();
		}

		if (currUnit != null && !AbleToMove && !AbleToAttack)
		{
			if (Input.GetMouseButtonDown(1))
			{
				currUnit = null;
				AbleToChangeUnit = true;
			}
		}
		if (AbleToMove)
		{ 
			if (Input.GetMouseButtonDown(1))
			{
				StoppedMoving = true;
				AbleToMove = false;
			}
		}
		if (AbleToAttack)
		{
			if (Input.GetMouseButtonDown(1))
			{
				AbleToAttack = false;
			}
		}
	}

	// Player's Update
	public void PlayerUpdate()
	{
		
	}

	// Enter Player's turn
	public void EnterPlayerTurn()
	{
		// Set player's turn to true
		PlayerTurn = true;
		// Remove player from front of queue list
		_queueOfUnits.Dequeue ();
		// Set currUnit to null so that can switch to different unit's
		// according to which unit player clicks on
		currUnit = null;
	}

	// Exit Player's Turn
	public void ExitPlayerTurn()
	{
		// Set player's turn to false
		PlayerTurn = false;

		// Add player to back of queue list
		// Player is represented as -1 in the queue
		_queueOfUnits.Enqueue(-1);
	}

	// Set the next unit's turn
	public void NextTurn()
	{		
		// Next turn's unit
		currUnit = UnitManager.Instance.GetUnit (_queueOfUnits.Dequeue ());
		_queueOfUnits.Enqueue (currUnit.GetID ());
		currUnit.TurnStart ();
	}

	// Set & Get Curr Unit
	public Units GetCurrUnit() {return currUnit;}
	public void SetCurrUnit(Units _nextUnit) {currUnit = _nextUnit;}

	// Set & Get Unit Can Change
	public bool GetAbleToChangeUnit() {return AbleToChangeUnit;}
	public void SetAbleToChangeUnit(bool _canChange) {AbleToChangeUnit = _canChange;}

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
