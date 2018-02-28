using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawner : MonoBehaviour {
	// The Prefab for enemy AI
	[SerializeField]
	private GameObject PrefabAI;

	// The Prefab for the player
	[SerializeField]
	private GameObject PrefabPlayer;

	// Total number of Player units
	[SerializeField]
	private int numOfPlayers = 4;

	// Spawn Enemies
	public void SpawnEnemies()
	{
		// Loops the number of enemies
		for(int i = 0; i < BattleManager.Instance.GetNumOfEnemies (); ++i)
		{
			GameObject Temp;
	        Temp = Instantiate (PrefabAI, new Vector3(0,0,0), Quaternion.Euler (0,0,0));
		}
	}

	public void SpawnPlayers()
	{
		// Check if the list of player items is empty
		if(PlayerManager.Instance.GetListOfPlayerItems ().Count == 0)
		{ // If empty, means the player units have not been initialised before
			// Init Player units
			for(int i = 0; i < 4; ++i)
			{
				GameObject Temp;
				Temp = Instantiate (PrefabPlayer, new Vector3(0,0,0), Quaternion.Euler (0,0,0));
				PlayerManager.Instance.UpdatePlayerItems ();
			}
		}
		else
		{ // If not empty, it means that the player units have been initialised before
			for(int i = 0; i < PlayerManager.Instance.GetListOfPlayerItems ().Count; ++i)
			{
				GameObject Temp;
				Temp = Instantiate (PrefabPlayer, new Vector3(0,0,0), Quaternion.Euler (0,0,0));
				Players tempPlayer = Temp.GetComponent <Players>();
				tempPlayer.GetStats ()._weapon = PlayerManager.Instance.GetListOfPlayerItems ().ElementAt (i)._weapon;
				tempPlayer.GetStats ()._armor = PlayerManager.Instance.GetListOfPlayerItems ().ElementAt (i)._armor;
			}
		}
	}
}
