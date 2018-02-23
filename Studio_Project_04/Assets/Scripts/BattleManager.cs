using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This represents the game mode that the player is in
public enum GAMEMODE
{
	// When the player is in open world
	OPEN_WORLD = 0,
	// When the player has to kill all enemies to win game
	KILL_ALL_ENEMIES,
	// When the player has to protect a unit from the enemies' attack
	PROTECT_THE_PRESIDENT,
}

public class BattleManager : GenericSingleton<BattleManager> 
{
	// Represents the game mode the player is playing in
	private GAMEMODE game_mode;

	// Keeps track of number of enemies in battle
	private int numOfEnemies;

	// Keeps track of number of turns have passed
	private int numOfTurns;

	// Use this for initialization
	void Start () {
		// Set game mode to none
		game_mode = GAMEMODE.OPEN_WORLD;
		// Set num of enemies to 0
		numOfEnemies = 0;
		// Set num or turns to 0
		numOfTurns = 0;
	}
	
	// Update is called once per frame
	void Update () {
		switch(game_mode)
		{
			case GAMEMODE.OPEN_WORLD:
				break;
			case GAMEMODE.KILL_ALL_ENEMIES:
				break;
			case GAMEMODE.PROTECT_THE_PRESIDENT:
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
		
	// Get & Set num of enemies in battle
	public int GetNumOfEnemies() {return numOfEnemies;}
	public void SetNumOfEnemies(int _num) {numOfEnemies = _num;}

	// Get & Set num of turns that have passed in battle
	public int GetNumOfTurns() {return numOfTurns;}
	public void SetNumOfTurns(int _num) {numOfTurns = _num;}
}