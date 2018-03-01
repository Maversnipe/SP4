using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : GenericSingleton<TurnManager> {
	// List of the units in battle
		// Identitfied by their 
	private Queue<int> queueOfUnits = new Queue<int>();
	private AI[] listOfAIUnits;
	private AI currUnit;

	// Determine if it is player's turn or not
	private bool PlayerTurn = false;

	private int playerDoneCount = 0;

	void Start()
	{
		StartBattle ();
	}

	private bool loadedDefeat = false;

	// This is called whenever the player starts a battle
	public void StartBattle()
	{
		BattleManager.Instance.SetGameMode ();
		GameObject[] ArrayOfPlayers = GameObject.FindGameObjectsWithTag ("PlayerUnit");
		Debug.Log(ArrayOfPlayers.Count ());
		// Set the game mode
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
	}

	// Update is called once per frame
	void Update () {
		if (BattleManager.Instance.GetGameMode () == GAMEMODE.NONE)
			return;
		if(Input.GetKey("q"))
		{
			BattleManager.Instance.SetGameMode (GAMEMODE.NONE);
			PlayerManager.Instance.SetPlayerCount (0);
			SceneManager.LoadScene ("SceneDefeated");
		}
		// Get all players into an array - if empty, load defeat scene
		GameObject[] ArrayOfPlayers = GameObject.FindGameObjectsWithTag ("PlayerUnit");
		if (!loadedDefeat && ArrayOfPlayers.Length <= 0) {
			BattleManager.Instance.SetGameMode (GAMEMODE.NONE);
			PlayerManager.Instance.SetPlayerCount (0);
			SceneManager.LoadScene ("SceneDefeated");
			loadedDefeat = true;
		}

		// Enter Player Update only if it is Player's turn
		if(PlayerTurn)
		{
			// Update Player
			PlayerManager.Instance.UpdatePlayerUnits();
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
			thePlayer.GetStats().AP = thePlayer.GetStats ().startAP; 
		}

		// Set Camera position to be the last player unit's position
		Players theLastPlayer = ArrayOfPlayers [ArrayOfPlayers.Count () - 1].GetComponent <Players> ();
		Camera.main.GetComponent<CameraControl> ().setFocus (theLastPlayer.gameObject);

		// Set to not be able to move unit
		PlayerManager.Instance.SetAbleToMove (false);
		// Set to not be able to attack
		PlayerManager.Instance.SetAbleToAttack (false);
		// Set is moving to false
		PlayerManager.Instance.SetIsMoving (false);

		// Get the action menu gameobject
		GameObject ActionMenu2 = GameObject.FindGameObjectWithTag ("ActionMenu2");
		// Set action menu to active
		ActionMenu2.transform.GetChild (0).gameObject.SetActive (true);
		// Get the end button gameobject
		GameObject endButton = GameObject.FindGameObjectWithTag ("EndTurnButton");
		// Set end button to active
		endButton.transform.GetChild (0).gameObject.SetActive (true);

		// Set number of players done to 0
		playerDoneCount = 0;
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
			// Set the number of turns left
			if(BattleManager.Instance.GetGameMode() == GAMEMODE.PROTECT_THE_PRESIDENT)
				BattleManager.Instance.SetNumOfTurns (BattleManager.Instance.GetNumOfTurns () - 1);
			// Start Player's Turn
			EnterPlayerTurn ();
		}
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

	// Get & Set player done count
	public int GetPlayerDoneCount() { return playerDoneCount; }
	public void SetPlayerDoneCount(int _playerDoneCount) { playerDoneCount = _playerDoneCount; }

	// Get List Of AI
	public AI[] GetListOfAI() {return listOfAIUnits;}

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
		// If damage is not set, set it to normal damage with no advantage / disadvantage
		if(damageDeal == -1)
			damageDeal = normalDamage;

		// Decrease AP required to carry out the attack
		attacker.GetComponent<UnitVariables> ().AP -= weapon.AP;

		// Decrease AP from plater required to carry out the attack
		if (attacker.GetComponent<Players> () != null)
			attacker.GetComponent<Players> ().GetStats ().AP -= weapon.AP;

		return damageDeal;
	}

	public int CalculateRestHeal(GameObject Temp)
	{
		int returnValue;

		returnValue = Temp.GetComponent<UnitVariables> ().AP / 2;

		return returnValue;
	}
}
