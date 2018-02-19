using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
	// To show if Unit is an ally or enemy
	private enum FACTION
	{
		FACTION_ALLY,
		FACTION_ENEMY,
	}

	// Serializable private variable defining Unit
	[SerializeField]
	Color HoverColor;
	[SerializeField]
	float speed = 5.0f;
	[SerializeField]
	float initiative; // This determines which unit will be able to take the first turn
	[SerializeField]
	FACTION theFaction;
	[SerializeField]
	private bool isPlayable; // To show if unit is playable

	// Reference to the unit's Components
	private Renderer rend;
	private Color DefaultColor;

	// Reference to the UnitManager's instance
	private TurnManager turnManager;

    // Nodes
    public int nodeX;
    public int nodeZ;
	private Nodes currNode;
	private Nodes nextNode;


	// Unit's ID
	private int ID;
	private static int UnitCount = 5;
	private float counter = 0.0f;

	[SerializeField]
	private int HP;

	[SerializeField]
	public bool menuOpen;

	void Start ()
	{
		// Code Optimising - Get Renderer Component once only
		rend = GetComponent<Renderer> ();
		DefaultColor = rend.material.color;
		// Code Optimising - Get UnitManager instance once only
		turnManager = TurnManager.Instance;
		menuOpen = false;
		// Set a random initial position for unit
		currNode = GridSystem.Instance.GetNode (Random.Range(0, 9), Random.Range(0, 9));
		currNode.SetOccupied (this.gameObject);
		//currNode = GridSystem.Instance.GetNode(nodeX, nodeZ);
		transform.position = new Vector3 (currNode.transform.position.x, transform.position.y, currNode.transform.position.z);

		// Set Unit's ID
		ID = UnitCount;
		++UnitCount;

		// Add unit to unit manager
		UnitManager.Instance.AddUnit (this);
	}

	// Run only when Mouse click onto the unit
	void OnMouseDown()
	{
		// If it is Player's turn
		if(turnManager.IsPlayerTurn ())
		{
			// If can be played
			if(this.isPlayable)
			{
				PlayerManager.Instance.ChangeUnit (this);
				this.transform.GetChild (0).gameObject.SetActive (true);
				//nextNode = null;
			}
		}
	}

	// Run only when Mouse cursor move into the unit collision box
	void OnMouseEnter()
	{
		// If it is Player's turn
		if (turnManager.IsPlayerTurn ())
		{
			if (this.isPlayable)
			{
				// Change Color of unit to HoverColor
				rend.material.color = HoverColor;
			}
		}
	}

	// Run only when Mouse cursor move out of the unit collision box
	void OnMouseExit()
	{
		// If it is Player's turn
		if (turnManager.IsPlayerTurn ())
		{
			if (this.isPlayable && !PlayerManager.Instance.GetSelectedUnit ())
			{
				// Change Color of unit back to DefaultColor
				rend.material.color = DefaultColor;
			}
		}
	}

	void Update()
	{
		if(this.isPlayable)
		{
			if (turnManager.IsPlayerTurn ())
			{
				if (nextNode == null)
					return;
				// Move the unit to clicked Node position
				if (PlayerManager.Instance.GetAbleToMove () && !PlayerManager.Instance.GetStopMoving ())
				{
					// Movement section
					Vector3 targetPos = new Vector3 (nextNode.transform.position.x, transform.position.y, nextNode.transform.position.z);
					Vector3 dir = targetPos - transform.position;
					transform.Translate (dir.normalized * speed * Time.deltaTime, Space.World);

					// Reset section
					if (Vector3.Distance (transform.position, targetPos) <= 0.2f)
					{
						// Reset variables
						transform.position = targetPos;
						TurnEnd ();
						PlayerManager.Instance.ChangeUnit (null);
					}
				}
			}
		}
		else 
		{
			if (this == turnManager.GetCurrUnit ())
			{
				// TODO: Input AI Code
				counter += Time.deltaTime;

				if (counter >= 5.0f)
				{
					this.TurnEnd ();
					TurnManager.Instance.NextTurn ();
					counter = 0.0f;
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
		if (this.isPlayable)
		{ // If it is a playable unit
			this.transform.GetChild (0).gameObject.SetActive (false);
		}
		// Set Color back to default
		rend.material.color = DefaultColor;

		// If nextNode is not null, update currNode to be nextNode
		// And null nextNode
		if (nextNode)
		{
			currNode = nextNode;
			nextNode = null;
		}
	}


	// Get & Set Current Node
	public Nodes GetCurrNode() {return currNode;}
	public void SetCurrNode(Nodes _currnode) {currNode = _currnode;}

	// Get & Set Next Node
	public Nodes GetNextNode() {return nextNode;}
	public void SetNextNode(Nodes _nextnode) {nextNode = _nextnode;}

	// Get & Set if unit can be controlled
	public bool IsPlayable() {return isPlayable;}
	public void SetPlayable(bool _playable) {isPlayable = _playable;}

	// Get & Set Unit's ID
	public int GetID() {return ID;}
	public void SetID(int _id) {ID = _id;}

	// Get & Set Unit's Initiative
	public float GetInitiative() {return initiative;}
	public void SetInitiative(float _initiative) {initiative = _initiative;}

	// Get & Set HP
	public int GetHP() {return HP;}
	public void SetHP(int n_HP) {HP = n_HP;}
}
