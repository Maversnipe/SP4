using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawner : MonoBehaviour {
	// The Prefab for enemy AI
	[SerializeField]
	private GameObject PrefabAI;

	[SerializeField]
	private GameObject PrefabAIProtect;

	[SerializeField]
	private int  playerCount;

	[SerializeField]
	private GameObject PrefabPlayer;

	// Spawn Enemies
	public void SpawnEnemies()
	{
		// Loops the number of enemies
		for(int i = 0; i < BattleManager.Instance.GetNumOfEnemies (); ++i)
		{
			GameObject Temp;
	        Temp = Instantiate (PrefabAI, new Vector3(0,0,0), Quaternion.Euler (0,0,0));
		}

		if(BattleManager.Instance.GetGameMode () == GAMEMODE.PROTECT_THE_PRESIDENT)
		{
			GameObject Temp;
			Temp = Instantiate (PrefabAIProtect, new Vector3(0,0,0), Quaternion.Euler (0,0,0));
		}
	}

	public void SpawnPlayers()
	{
		// Loops the number of players
		for(int i = 0; i < playerCount; ++i)
		{
			GameObject Temp;
			Temp = Instantiate (PrefabPlayer, new Vector3(0,0,0), Quaternion.Euler (0,0,0));
		}
	}
}
