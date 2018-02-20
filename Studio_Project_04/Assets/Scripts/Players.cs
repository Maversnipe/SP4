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

	private UnitVariables Stats;

	// Unit's ID
	private int ID;
	private static int PlayerCount = 0;
	private float counter = 0.0f;

	[SerializeField]
	public bool menuOpen;

	void Start ()
	{
		//Gathers the name from the Unit Variable
		Stats = GetComponent<UnitVariables> ();
		//Gathers the stats from the Json File
		Stats = UnitDatabase.Instance.FetchUnitByName (Stats.Name);

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
		// Cheat key to lose scene
		if(Input.GetKeyDown("q"))
			SceneManager.LoadScene ("SceneDefeated");

		if (PlayerManager.Instance.GetSelectedUnit () != this)
			return;
		
		if (turnManager.IsPlayerTurn ())
			return;

		if (nextNode == null)
			return;

        // Checks if next Node has a unit inside
        if (nextNode.GetOccupied() != null)
        {
            Debug.Log("Node is occupied by : " + nextNode.GetOccupied().name);
            TurnEnd();
            PlayerManager.Instance.ChangeUnit(null);
            return;
        }

        // Move the unit to clicked Node position
        if (PlayerManager.Instance.GetAbleToMove () && PlayerManager.Instance.GetIsMoving ())
		{
			// Movement section
			Vector3 targetPos = new Vector3 (nextNode.transform.position.x, transform.position.y, nextNode.transform.position.z);
			Vector3 dir = targetPos - transform.position;
			transform.Translate (dir.normalized * 5 * Time.deltaTime, Space.World);
            
            // If node is reached
            if (Vector3.Distance(transform.position, targetPos) <= 0.2f)
            {
                // Reset variables
                transform.position = targetPos;
                TurnEnd();
                PlayerManager.Instance.ChangeUnit(null);
            }
        }
		else 
		{
			if (this == turnManager.GetCurrUnit ())
			{
				// Reset variables
				transform.position = targetPos;
				TurnEnd ();
				PlayerManager.Instance.ChangeUnit (null);
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
		rend.material.color = DefaultColor;

		// If nextNode is not null, update currNode to be nextNode
		// And null nextNode
		if (nextNode)
		{
			//Debug.Log ("Before Move -> X: " + currNode.GetXIndex() + " Z: " + currNode.GetZIndex() + " Name: " + currNode.GetOccupied().name);
			currNode = nextNode;
			currNode.SetOccupied (this.gameObject);
			//Debug.Log ("After Move -> X: " + currNode.GetXIndex() + " Z: " + currNode.GetZIndex() + " Name: " + currNode.GetOccupied().name);
			nextNode = null;
		}
	}

	// BFS for unit

	// Get & Set Current Node
	public Nodes GetCurrNode() {return currNode;}
	public void SetCurrNode(Nodes _currnode) {currNode = _currnode;}

	// Get & Set Next Node
	public Nodes GetNextNode() {return nextNode;}
	public void SetNextNode(Nodes _nextnode) {nextNode = _nextnode;}

	// Get & Set Unit's ID
	public int GetID() {return ID;}
	public void SetID(int _id) {ID = _id;}

	// Get Unit's Stats
	public UnitVariables getStats() {return Stats;}
}
