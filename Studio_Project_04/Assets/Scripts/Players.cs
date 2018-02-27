using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Players : MonoBehaviour
{
	// Serializable private variable defining Unit
	[SerializeField]
	Color HoverColor;

	// Reference to the unit's Components
	private Renderer rend;
	private Color DefaultColor;

	// Reference to the UnitManager's instance
	private TurnManager turnManager;

    // Nodes
	private Nodes currNode;
	private Nodes nextNode;
	private Nodes targetNode;

	// Unit Stats
	private UnitVariables Stats;

	// Unit's ID
	private int ID;
	private static int PlayerCount = 0;
	private float counter = 0.0f;

	// Player's path
	private Stack<Nodes> path;

	// Player's AP amount in the current turn
	private int turnAP;

	void Start ()
	{
		//Gathers the name from the Unit Variable
		Stats = this.gameObject.GetComponent<UnitVariables> ();
		//Gathers the stats from the Json File
		Stats.Copy(UnitDatabase.Instance.FetchUnitByName (Stats.Name));
		Debug.Log ("Stats " + Stats);
		// Code Optimising - Get Renderer Component once only
		rend = GetComponent<Renderer> ();
		DefaultColor = rend.material.color;
		// Code Optimising - Get UnitManager instance once only
		turnManager = TurnManager.Instance;
		// Set a random initial position for unit
		currNode = GridSystem.Instance.GetNode (Random.Range(0, 9), Random.Range(0, 9));
		currNode.SetOccupied (this.gameObject);
		transform.position = new Vector3 (currNode.transform.position.x, transform.position.y, currNode.transform.position.z);

		// Set Unit's ID
		ID = PlayerCount;
		++PlayerCount;

		// Init the path
		path = new Stack<Nodes>();

		// Set turnAP to player's AP
		turnAP = Stats.AP;
	}

	// Run only when Mouse click onto the unit
	void OnMouseDown()
	{
		// Return if player is blocked by UI elements
		if (EventSystem.current.IsPointerOverGameObject ())
			return;
		
		// If it is Player's turn
		if (turnManager.IsPlayerTurn () && turnAP > 0)
		{
			// Change Unit To This Unit
			PlayerManager.Instance.ChangeUnit (this);
			// Find GO with ActionMenu2 tag
			GameObject menu = GameObject.FindGameObjectWithTag ("ActionMenu");
			// Make GO's child active, which makes the menu appear
			menu.transform.GetChild (0).gameObject.SetActive (true);

			if (Stats.AP >= Stats._weapon.AP)
			{
				Debug.Log ("ATT POWAH");
				// Set Text for Attack Button
				GameObject attButton = GameObject.FindGameObjectWithTag ("AttackButton");
				Debug.Log (attButton);
				attButton.transform.GetChild(0).gameObject.GetComponentInChildren <Text> ().text = "Attack\n" + PlayerManager.Instance.GetSelectedUnit ().GetStats ()._weapon.AP + " AP";
				attButton.transform.GetChild (0).gameObject.SetActive (true);

				// Set not selectable attack button to false
				attButton = GameObject.FindGameObjectWithTag ("AttackButtonNotSelectable");
				attButton.transform.GetChild (0).gameObject.SetActive (false);
			} 
			else
			{
				Debug.Log ("ATT NO POWA");
				// Set Text for Attack Button
				GameObject attButton = GameObject.FindGameObjectWithTag ("AttackButtonNotSelectable");
				Debug.Log (attButton);
				attButton.transform.GetChild(0).gameObject.GetComponentInChildren <Text> ().text = "Attack\n" + PlayerManager.Instance.GetSelectedUnit ().GetStats ()._weapon.AP + " AP";
				attButton.transform.GetChild (0).gameObject.SetActive (true);

				// Set attack button to false
				attButton = GameObject.FindGameObjectWithTag ("AttackButton");
				attButton.transform.GetChild (0).gameObject.SetActive (false);
			}
		}
	}

	// Run only when Mouse cursor move into the unit collision box
	void OnMouseEnter()
	{
		// Return if player is blocked by UI elements
		if (EventSystem.current.IsPointerOverGameObject ())
			return;
		
		// If it is Player's turn
		if (turnManager.IsPlayerTurn ())
		{
			// Spawn Unit Info Window
			Stats.SetUnitInfoWindow(true);

			// Change Color of unit to HoverColor
			rend.material.color = HoverColor;
		}
	}

	// Run only when Mouse cursor move out of the unit collision box
	void OnMouseExit()
	{
		// If it is Player's turn
		if (turnManager.IsPlayerTurn ())
		{
			if (!PlayerManager.Instance.GetSelectedUnit ())
			{
				// De-Spawn Unit Info Window
				Stats.SetUnitInfoWindow(false);

				// Change Color of unit back to DefaultColor
				rend.material.color = DefaultColor;
			}
		}
	}

	void Update()
	{
		this.gameObject.GetComponent<UnitVariables> ().Copy (Stats);
		// Update Unit Info Window
		this.gameObject.GetComponent<UnitVariables> ().UpdateUnitInfo ();

		// Cheat key to lose scene
		if(Input.GetKeyDown("q"))
			SceneManager.LoadScene ("SceneDefeated");

		if (!turnManager.IsPlayerTurn ())
			return;

        // Move the unit to clicked Node position
        if (PlayerManager.Instance.GetAbleToMove () && PlayerManager.Instance.GetIsMoving ())
		{
			if (nextNode == null)
				return;

			// Checks if next Node has a unit inside
			if (nextNode.GetOccupied() != null)
			{
				nextNode = null;
				TurnEnd ();
				PlayerManager.Instance.ChangeUnit (null);
				return;
			}

			// Movement section
			Vector3 targetPos = new Vector3 (nextNode.transform.position.x, transform.position.y, nextNode.transform.position.z);
			Vector3 dir = targetPos - transform.position;
			transform.Translate (dir.normalized * 5 * Time.deltaTime, Space.World);
            
            // If next node is reached
            if (Vector3.Distance(transform.position, targetPos) <= 0.2f)
            {
                // Reset variables
                transform.position = targetPos;
				// Set next node to not be on path
				nextNode.SetIsPath (false);
				// Change the colour of next node to be normal
				nextNode.ChangeColour ();
				if (path.Count > 0)
				{
					// Set the curr node's occupied to null
					currNode.SetOccupiedNULL ();
					// Set curr node as the next node
					currNode = nextNode;
					// Set the new curr node's occupied to this unit
					currNode.SetOccupied (this.gameObject);
					// Set the next node
					nextNode = path.Pop ();
					// Set AP to new AP
					turnAP -= 1;
					this.Stats.AP--;
				} 
				else
				{
					// Set AP to new AP
					turnAP -= 1;
					this.Stats.AP--;
					// End the turn of the player node
					TurnEnd ();
					// Change unit back to no unit
					PlayerManager.Instance.ChangeUnit (null);
				}
            }
        }
	}

	// Init for the start of each Unit's turn
	public void TurnStart()
	{
		// Spawn Unit Info Window
		Stats.SetUnitInfoWindow(true);

		// Reset nextNode to null
		nextNode = null;
		// Set object to be highlighted
		rend.material.color = HoverColor;
		// Set Camera position to be the Unit's position
		Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
	}

	// The reset for the end of each Unit's turn
	public void TurnEnd()
	{
		// Find GO with ActionMenu2 tag
		GameObject menu = GameObject.FindGameObjectWithTag ("ActionMenu");
		// Make GO's child active, which makes the menu not appear
		menu.transform.GetChild(0).gameObject.SetActive (false);

		// De-Spawn Unit Info Window
		Stats.SetUnitInfoWindow(false);

		// Set Color back to default
		SetToDefaultColor();

		// If nextNode is not null, update currNode to be nextNode
		// And null nextNode
		if (nextNode)
		{
			currNode.SetOccupiedNULL();
			//Debug.Log ("Before Move -> X: " + currNode.GetXIndex() + " Z: " + currNode.GetZIndex() + " Name: " + currNode.GetOccupied().name);
			currNode = nextNode;
			// Set curr node as occupied by this unit
			currNode.SetOccupied (this.gameObject);
			// Set next node to be null
			nextNode = null;
		}
			
		// Get the cancel button gameobject
		GameObject cancelButton = GameObject.FindGameObjectWithTag ("CancelButtonNotSelectable");
		// Set cancel button to not active
		if(cancelButton)
			cancelButton.transform.GetChild(0).gameObject.SetActive (true);
		// Get the cancel button gameobject
		cancelButton = GameObject.FindGameObjectWithTag ("CancelButton");
		// Set cancel button to not active
		if(cancelButton)
			cancelButton.transform.GetChild(0).gameObject.SetActive (false);

		// Get array of player units
		GameObject[] ArrayOfPlayers = GameObject.FindGameObjectsWithTag ("PlayerUnit");

		for (int i = 0; i < ArrayOfPlayers.Length; ++i)
		{
			// Check if AP is less than or equal to zero
			if (ArrayOfPlayers [i].GetComponent <Players> ().GetStats ().AP <= 0)
			{
				// Count how many player finish their turns
				if (turnManager.GetPlayerDoneCount () < ArrayOfPlayers.Length)
					turnManager.SetPlayerDoneCount (turnManager.GetPlayerDoneCount () + 1);
				else
				{
					// Auto End Turn
					PlayerManager.Instance.SetSelectedUnit (null);
					PlayerManager.Instance.SkipTurn ();
					break;
				}
			}
		}
	}

	// Set To Default Color
	public void SetToDefaultColor()
	{
		rend.material.color = DefaultColor;
	}

	// Get & Set Current Node
	public Nodes GetCurrNode() {return currNode;}
	public void SetCurrNode(Nodes _currnode) {currNode = _currnode;}

	// Get & Set Next Node
	public Nodes GetNextNode() {return nextNode;}
	public void SetNextNode(Nodes _nextnode) {nextNode = _nextnode;}

	// Get & Set Target Node
	public Nodes GetTargetNode() {return targetNode;}
	public void SetTargetNode(Nodes _targetNode) {targetNode = _targetNode;}

	// Get & Set Unit's ID
	public int GetID() {return ID;}
	public void SetID(int _id) {ID = _id;}
    
	// Get Unit's Path
	public Stack<Nodes> GetPath() {return path;}

	// Get Unit's Stats
	public UnitVariables GetStats() {return Stats;}
	public void SetStats(UnitVariables n_Stats) {Stats = n_Stats;}

	// Get & Set Unit's AP for the turn
	public int GetAP() {return turnAP;}
	public void SetAP(int _ap)
	{
		turnAP = _ap;
		this.Stats.AP = _ap;
	}
}
