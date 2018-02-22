using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	// Player's AP Left
	private int APLeft;

	[SerializeField]
	public bool menuOpen;

	void Start ()
	{
		//Gathers the name from the Unit Variable
		Stats = this.gameObject.GetComponent<UnitVariables> ();
		//Gathers the stats from the Json File
		Stats.Copy(UnitDatabase.Instance.FetchUnitByName (Stats.Name));

		// Code Optimising - Get Renderer Component once only
		rend = GetComponent<Renderer> ();
		DefaultColor = rend.material.color;
		// Code Optimising - Get UnitManager instance once only
		turnManager = TurnManager.Instance;
		menuOpen = false;
		// Set a random initial position for unit
		currNode = GridSystem.Instance.GetNode (Random.Range(0, 9), Random.Range(0, 9));
		currNode.SetOccupied (this.gameObject);
		transform.position = new Vector3 (currNode.transform.position.x, transform.position.y, currNode.transform.position.z);

		// Set Unit's ID
		ID = PlayerCount;
		++PlayerCount;

		// Init the path
		path = new Stack<Nodes>();
	}

	// Run only when Mouse click onto the unit
	void OnMouseDown()
	{
		// If it is Player's turn
		if(turnManager.IsPlayerTurn ())
		{
			PlayerManager.Instance.ChangeUnit (this);
			this.transform.GetChild (0).gameObject.SetActive (true);
		}
	}

	// Run only when Mouse cursor move into the unit collision box
	void OnMouseEnter()
	{
		// If it is Player's turn
		if (turnManager.IsPlayerTurn ())
		{
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
				// Change Color of unit back to DefaultColor
				rend.material.color = DefaultColor;
			}
		}
	}

	void Update()
	{
		this.gameObject.GetComponent<UnitVariables> ().Copy (Stats);

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
				} 
				else
				{
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
		// If it is a playable unit
		this.transform.GetChild (0).gameObject.SetActive (false);

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
		GameObject cancelButton = GameObject.FindGameObjectWithTag ("CancelButton");
		// Set cancel button to not active
		cancelButton.transform.GetChild (0).gameObject.SetActive (false);
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
}
