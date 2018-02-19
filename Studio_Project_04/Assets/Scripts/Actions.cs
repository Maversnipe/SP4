using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
	// Reference to the UnitManager's instance
	private PlayerManager playerManager;

	public bool moving;
	void Start()
	{
		playerManager = PlayerManager.Instance;	
	}

    public void StartAttack()
	{
		playerManager.SetOpenMenu (false);
        // Check if unit is available and if unit can move
		if (playerManager.GetSelectedUnit() != null && !playerManager.GetAbleToAttack())
        {
            Debug.Log("Started attack.");
			playerManager.SetAbleToAttack(true);
        }
    }

    // Start moving the selected unit
    public void StartMoving ()
	{
		playerManager.SetOpenMenu (false);
		// Check if unit is available and if unit can move
		if (playerManager.GetSelectedUnit () != null && !playerManager.GetAbleToMove())
		{
			Debug.Log ("Started moving.");
			playerManager.SetStopMoving(false);
			playerManager.SetAbleToMove(true);
            moving = true;
		}
	}

	// Skip current turn
	public void SkipTurn ()
	{
		playerManager.SetOpenMenu (false);
		// Check if unit is available and if player can change to control another unit
		if (playerManager.GetSelectedUnit () != null)
		{
			Debug.Log ("Skipped turn.");
			playerManager.GetSelectedUnit ().TurnEnd ();
			TurnManager.Instance.ExitPlayerTurn ();
		}
	}

    void Update()
    {
		if (playerManager.GetSelectedUnit () &&
			playerManager.GetOpenMenu () && 
			!moving && !playerManager.GetAbleToAttack ())
		{
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive (true);
			}
		} 
		else if (!playerManager.GetOpenMenu () || moving || 
			playerManager.GetAbleToAttack () || !TurnManager.Instance.IsPlayerTurn ())
		{
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive (false);
			}
		}

		if (playerManager.GetStopMoving ())
		{
			moving = false;
		}


//		if(!playerManager.GetAbleToChangeUnit())
//        {
//			Vector3 temp = new Vector3(playerManager.GetSelectedUnit().transform.position.x, transform.position.y, playerManager.GetSelectedUnit().transform.position.z);
//            transform.position = Camera.main.WorldToScreenPoint(temp);
//        }
    }

}
