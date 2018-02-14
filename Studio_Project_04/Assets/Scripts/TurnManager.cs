using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
	// List of the units in battle
		// Identitfied by their 
	private Stack<int> _listOfUnits;
	private int currUnit;


	// Use this for initialization
	void Awake () {
		GameObject[] listOfGO = GameObject.FindGameObjectsWithTag ("Unit");
		for(int i = 0; i < listOfGO.Length; ++i)
		{
			Units theUnit = listOfGO [i].GetComponent<Units> ();
			if (theUnit.IsPlayable ())
				continue;


		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
