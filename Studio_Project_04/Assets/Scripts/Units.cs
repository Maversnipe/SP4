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

	// To show if unit is playable
	private bool isPlayable;

	// Unit's ID
	private int ID;
	private static int UnitCount = 5;

	void Start ()
	{
		// Code Optimising - Get Renderer Component once only
		rend = GetComponent<Renderer> ();
		DefaultColor = rend.material.color;
		// Code Optimising - Get UnitManager instance once only
		turnManager = TurnManager.Instance;

		// Set a random initial position for unit
		//currNode = GridSystem._instance.GetNode (Random.Range(0, 9), Random.Range(0, 9));
		currNode = GridSystem.Instance.GetNode(nodeX, nodeZ);
		transform.position = new Vector3 (currNode.transform.position.x, transform.position.y, currNode.transform.position.z);

		// Set Unit's ID
		ID = UnitCount;
		++UnitCount;

		isPlayable = true;

		// Add unit to unit manager
		UnitManager.Instance.AddUnit (this);
	}

	// Run only when Mouse click onto the unit
	void OnMouseDown()
	{
//		if (turnManager.GetAbleToChangeUnit ())
//		{
//			Debug.Log ("Unit Selected.");
//			turnManager.SetAbleToChangeUnit (false);
//			//turnManager.SetUnitToDoActions (this.gameObject);
//			Camera.main.transform.position = new Vector3(turnManager.GetCurrUnit ().transform.position.x, Camera.main.transform.position.y, turnManager.GetCurrUnit().transform.position.z);
//			turnManager.SetOpenMenu (true);
//			// Reset variables
//			nextNode = null;
//			turnManager.SetStopMoving (false);
//		}
	}

	// Run only when Mouse cursor move into the unit collision box
	void OnMouseEnter()
	{
//		if (turnManager.GetAbleToChangeUnit ())
//		{
//			// Change Color of unit to HoverColor
//			rend.material.color = HoverColor;
//		}
	}

	// Run only when Mouse cursor move out of the unit collision box
	void OnMouseExit()
	{
		if (turnManager.GetAbleToChangeUnit ())
		{
			// Change Color of unit back to DefaultColor
			rend.material.color = DefaultColor;
		}
	}

	void Update()
	{
		if (nextNode == null)
			return;
		
        // Move the unit to clicked Node position
		if (!turnManager.GetAbleToMove () && !turnManager.GetStopMoving ())
		{
			// Movement section
			Vector3 targetPos = new Vector3(nextNode.transform.position.x, transform.position.y, nextNode.transform.position.z);
			Vector3 dir = targetPos - transform.position;
			transform.Translate (dir.normalized * speed * Time.deltaTime, Space.World);

			// Reset section
			if (Vector3.Distance (transform.position, targetPos) <= 0.2f)
			{
				// Reset variables
				transform.position = targetPos;
				TurnEnd ();
				TurnManager.Instance.NextTurn ();
			}
		}
	}

	public void TurnStart()
	{
		// Change camera's position to where the unit is located
		Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
		turnManager.SetOpenMenu (true);
		// Reset variables
		nextNode = null;
		turnManager.SetStopMoving (false);

		rend.material.color = HoverColor;
	}

	public void TurnEnd()
	{
		turnManager.SetOpenMenu (false);
		rend.material.color = DefaultColor;

		turnManager.SetStopMoving (true);
		currNode = nextNode;
		nextNode = null;
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
}
