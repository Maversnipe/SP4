using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
	// Reference to the UnitManager's instance
	private TurnManager turnManager;

    public bool moving;
	void Start()
	{
		turnManager = TurnManager.Instance;	
	}

    public void StartAttack()
    {
        // Check if unit is available and if unit can move
		if (turnManager.GetCurrUnit() != null && !turnManager.GetAbleToAttack())
        {
            Debug.Log("Started attack.");
			turnManager.SetAbleToAttack(true);
        }
    }

    // Start moving the selected unit
    public void StartMoving ()
	{
		// Check if unit is available and if unit can move
		if (turnManager.GetCurrUnit () != null && !turnManager.GetAbleToMove())
		{
			Debug.Log ("Started moving.");
			turnManager.SetStopMoving(false);
			turnManager.SetAbleToMove(true);
            moving = true;
		}
	}

	// Skip current turn
	public void SkipTurn ()
	{
		// Check if unit is available and if player can change to control another unit
		if (turnManager.GetCurrUnit () != null)
		{
			Debug.Log ("Skipped turn.");
			turnManager.GetCurrUnit ().TurnEnd ();
			turnManager.NextTurn ();
		}
	}

    void Update()
    {
		if(turnManager.GetOpenMenu() && !moving && !turnManager.GetAbleToAttack())
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
		else if(!turnManager.GetOpenMenu() || moving || turnManager.GetAbleToAttack())
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

		if (turnManager.GetStopMoving())
        {
            moving = false;
        }

		if(!turnManager.GetAbleToChangeUnit())
        {
            Vector3 temp = new Vector3(turnManager.GetCurrUnit().transform.position.x, transform.position.y, turnManager.GetCurrUnit().transform.position.z);
            transform.position = Camera.main.WorldToScreenPoint(temp);
        }
    }

}
