using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitVariables : GenericSingleton<PlayerUnitVariables> {
	// Player Unit Prefab
	[SerializeField]
	private GameObject PlayerPrefab;

	// List of player's unit variables
		// This is to assign into the player when they get instantiated
	private List<UnitVariables> ListOfUnitVariables = new List<UnitVariables> () ;

	// Use this for initialization
	void Start () {
		// Add some basic unit variables into list
		for(int i = 0; i < 4; ++i)
		{
			//Gathers the name from the Unit Variable
			UnitVariables Stats = PlayerPrefab.GetComponent<UnitVariables> ();
			//Gathers the stats from the Json File
			Stats.Copy(UnitDatabase.Instance.FetchUnitByName (Stats.Name));
			ListOfUnitVariables.Add (Stats);
		}
	}

	// Get List Of Unit Variables
	public List<UnitVariables> GetListVariables() {return ListOfUnitVariables;}
}