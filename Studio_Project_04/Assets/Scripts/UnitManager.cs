using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	// Public Reference for other class to access UnitManager
	public static UnitManager instance;

	// Units
	public GameObject TestingPlayer;
    public GameObject TestingPlayer2;
	public List<Units> UnitList = new List<Units>();

    // Determine if can change selected unit
    public bool AbleToChangeUnit = true;
	// Determine if can move selected unit
	public bool AbleToMove = false;
    public bool AbleToAttack = false;
	// Determine if can move selected unit
	public bool StoppedMoving = false;
    public bool openMenu = false;
	// A reference to the current selected unit
	private GameObject UnitToDoActions;

    GameObject[] playerUnits;
    GameObject[] enemyUnits;

	// Taking care of Singleton for UnityManager
	void Awake ()
	{
		if (instance != null)
		{
			Debug.LogError ("More than one UnitManager in scene!");
			return;
		}
		instance = this;
        playerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");
        enemyUnits = GameObject.FindGameObjectsWithTag("enemyUnit");
	}

	// Add the unit into the list 
	public void AddUnit(GameObject GO)
	{
		Units newUnit = GO.GetComponent <Units> ();
		if(newUnit)
		{
			UnitList.Add (newUnit);
		}
	}

	// Get the currently selected unit
	public GameObject GetUnitToDoActions ()
	{
		return UnitToDoActions;
	}

	// Change the reference to current selected unit
	public void SetUnitToDoActions (GameObject Unit)
	{
		UnitToDoActions = Unit;
	}

    void Update()
    {
        if (UnitToDoActions != null && !AbleToMove && !AbleToAttack)
        {
            if (Input.GetMouseButtonDown(1))
            {
                UnitToDoActions = null;
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
}
