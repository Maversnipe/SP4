using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This represents the game mode that the player is in
public enum GAMEMODE
{
	// When the player is in open world
	NONE = 0,
	// When the player has to kill all enemies to win game
	KILL_ALL_ENEMIES,
	// When the player has to protect a unit from the enemies' attack
	PROTECT_THE_PRESIDENT,
}

public class BattleManager : GenericSingleton<BattleManager> 
{
	// Represents the game mode the player is playing in
	private GAMEMODE game_mode;

	// Total Num Of Enemies
	public int TOTAL_ENEMIES = 5;

	// Total Num Of Turns
	public int TOTAL_TURNS = 30;

	// Keeps track of number of enemies in battle
	private int numOfEnemies;

	// Keeps track of number of turns have passed
	private int numOfTurns;

	// Use this for initialization
	void Start () {
		// Set game mode to none
		game_mode = GAMEMODE.NONE;
		// Set num of enemies to 0
		numOfEnemies = 0;
		// Set num or turns to 0
		numOfTurns = 0;

		BattleManager.Instance.SetGameMode ();
	}

	// Update is called once per frame
	void Update () {
		switch(game_mode)
		{
			case GAMEMODE.NONE:
				// If in open world, don't do anything
				break;
			case GAMEMODE.KILL_ALL_ENEMIES:
				// Checks if enemy count is less than or equal to 0
				if (numOfEnemies <= 0)
				{
					// Game Win
					PlayerManager.Instance.SetPlayerCount (0);
					PlayerManager.Instance.SetCurrQuest (PlayerManager.Instance.GetCurrQuest () + 1);
					SceneManager.LoadScene ("SceneCleared");
				}
				// Set the text for BattleInfo
				GameObject.FindGameObjectWithTag ("BattleInfo").GetComponentInChildren<Text> ().text = "Enemies Left: " + numOfEnemies;
				break;
			case GAMEMODE.PROTECT_THE_PRESIDENT:	
				// Checks if num of turns is less than or equal to 0
				if (numOfTurns <= 0)
				{
					// Game Win
					PlayerManager.Instance.SetPlayerCount (0);
					PlayerManager.Instance.SetCurrQuest (PlayerManager.Instance.GetCurrQuest () + 1);
					SceneManager.LoadScene ("SceneCleared");
				}
				AI AIProtect = GameObject.FindGameObjectWithTag ("aiProtect").GetComponent <AI>();
				if(AIProtect.GetStats ().HP <= 0)
				{
					// Gameover
					PlayerManager.Instance.SetPlayerCount (0);
					SceneManager.LoadScene ("SceneDefeated");
				}
				// Set the text for BattleInfo
				GameObject.FindGameObjectWithTag ("BattleInfo").GetComponentInChildren<Text> ().text = "Turns Left: " + numOfTurns;
				break;
		}
	}

	// Reset all values
	public void ResetVal()
	{
		// Set num of enemies to 0
		numOfEnemies = 0;
		// Set num or turns to 0
		numOfTurns = 0;
	}

	// Start Kill All Game Mode
	public void StartKillAll()
	{
		// Set Game Mode
		game_mode = GAMEMODE.KILL_ALL_ENEMIES;
		// Set total num of enemies
		numOfEnemies = TOTAL_ENEMIES;
		// Set the text for BattleInfo
		GameObject.FindGameObjectWithTag ("BattleInfo").GetComponentInChildren<Text> ().text = "Enemies Left: " + numOfEnemies;
		// Do spawning of AI
		Spawner spawner = GameObject.FindGameObjectWithTag ("Spawner").GetComponent <Spawner>();
		spawner.SpawnEnemies ();
	}

	// Start Protect The President Game Mode
	public void StartProtect()
	{
		// Set Game Mode
		game_mode = GAMEMODE.PROTECT_THE_PRESIDENT;
		// Set total num of turns
		numOfTurns = TOTAL_TURNS;
		// Set total num of enemies
		numOfEnemies = TOTAL_ENEMIES;
		// Set the text for BattleInfo
		GameObject.FindGameObjectWithTag ("BattleInfo").GetComponentInChildren<Text> ().text = "Turns Left: " + numOfTurns;
		// Do spawning of AI
		Spawner spawner = GameObject.FindGameObjectWithTag ("Spawner").GetComponent <Spawner>();
		spawner.SpawnEnemies ();
	}

	// Set Game Mode
	public void SetGameMode()
	{
		// Set based on the player's current mission
		switch(PlayerManager.Instance.GetCurrQuest ())
		{
			case 1:
				// Start Kill All Enemies Game Mode
				StartKillAll ();
				break;
			case 0:
				// Start Protect The President Game Mode
				StartProtect ();
				break;
			default:
				break;
		}
	}
		
	// Get & Set num of enemies in battle
	public int GetNumOfEnemies() {return numOfEnemies;}
	public void SetNumOfEnemies(int _num) {numOfEnemies = _num;}

	// Get & Set num of turns that have passed in battle
	public int GetNumOfTurns() {return numOfTurns;}
	public void SetNumOfTurns(int _num) {numOfTurns = _num;}

	// Get Gamemode
	public GAMEMODE GetGameMode() {return game_mode;}
}