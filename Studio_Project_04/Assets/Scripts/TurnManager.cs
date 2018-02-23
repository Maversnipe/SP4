using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : GenericSingleton<TurnManager> {
	// List of the units in battle
		// Identitfied by their 
	private Queue<int> queueOfUnits = new Queue<int>();
	private AI[] listOfAIUnits;
	private AI currUnit;

	// Determine if it is player's turn or not
	private bool PlayerTurn = false;

	// This is called whenever the player starts a battle
	public void StartBattle()
	{
		listOfAIUnits = FindObjectsOfType<AI> ();
		if (listOfAIUnits.Count() == 0)
			return;


		// Sort List based on each unit's initiative
		// But if same initiative, sort by ID
		listOfAIUnits = listOfAIUnits.OrderBy(x => x.GetStats().Initiative).ThenBy(x => x.GetID()).ToArray ();

		for(int i = 0; i < listOfAIUnits.Count(); ++i)
		{ // Iterate through the list of AI
			AI theAIUnit = listOfAIUnits[i];

			// Push the AI unit into the queue
			queueOfUnits.Enqueue (theAIUnit.GetID ());
		}

		// Player will always start first for each battle
		EnterPlayerTurn ();

		// Add player to back of queue list
		// Player is represented as -1 in the queue
		queueOfUnits.Enqueue(-1);

		GameObject startbutton = GameObject.FindGameObjectWithTag ("StartBattleButton");
		startbutton.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		// Update player health barx	
		//PlayerManager.Instance.GetSelectedUnit ().GetStats ().UpdateHealthBar ();
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

		// Get array of player units
		GameObject[] ArrayOfPlayers = GameObject.FindGameObjectsWithTag ("PlayerUnit");
		// Iterate through array of player units
		for(int i = 0; i < ArrayOfPlayers.Count (); ++i)
		{
			Players thePlayer = ArrayOfPlayers [i].GetComponent <Players> ();
			// Set each of Player's unit's AP at start of player's turn
			thePlayer.SetAP (thePlayer.GetStats ().startAP); 
		}

		// Set to not be able to move unit
		PlayerManager.Instance.SetAbleToMove (false);
		// Set to not be able to attack
		PlayerManager.Instance.SetAbleToAttack (false);
		// Set is moving to false
		PlayerManager.Instance.SetIsMoving (false);

		// Get the end button gameobject
		GameObject endButton = GameObject.FindGameObjectWithTag ("EndTurnButton");
		// Set end button to active
		endButton.transform.GetChild (0).gameObject.SetActive (true);

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
		// Set is moving to false
		PlayerManager.Instance.SetIsMoving (false);

		// Start Next AI's Turn
		NextTurn();
	}

	// Set the next unit's turn
	public void NextTurn()
	{		
		// Get the next turn's unit ID
		int Num = queueOfUnits.Dequeue ();
		// Put next turn's unit ID at back of queue
		queueOfUnits.Enqueue (Num);

		// Check if the next unit is supposed to be player
		if (Num != -1)
		{
			// Set next turn's unit
			currUnit = FindAIUnit(Num);
			// Start next turn's unit's turn
			currUnit.TurnStart ();
		}
		else
		{
			// Start Player's Turn
			EnterPlayerTurn ();
		}
	}

	// Set camera to the position in the middle of the grid
	void CameraReset()
	{
		Camera.main.transform.position = new Vector3 (GridSystem.Instance.GetWidth () / 2.0f, 
			Camera.main.transform.position.y, GridSystem.Instance.GetHeight () / 2.0f); 
	}

	// Find AI Unit based on its ID
	public AI FindAIUnit(int _id)
	{
		for (int i = 0; i < listOfAIUnits.Count (); ++i) 
		{
			if (listOfAIUnits [i].GetID () == _id)
				return listOfAIUnits [i];
		}

		return null;
	}

	// Set & Get Curr Unit
	public AI GetCurrUnit() {return currUnit;}
	public void SetCurrUnit(AI _nextUnit) {currUnit = _nextUnit;}

	// Set & Get Player's Turn
	public bool IsPlayerTurn() {return PlayerTurn;}
	public void SetPlayerTurn(bool _turn) {PlayerTurn = _turn;}
}
