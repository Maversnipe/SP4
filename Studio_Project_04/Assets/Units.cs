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
	float speed = 10.0f;
	[SerializeField]
	FACTION theFaction;

	// Reference to the unit's Components
	private Renderer rend;
	private Color DefaultColor;

	// Reference to the UnitManager's instance
	private UnitManager unitmanager;

	// Nodes
	private Nodes currNode;
	private Nodes nextNode;

	void Start ()
	{
		// Code Optimising - Get Renderer Component once only
		rend = GetComponent<Renderer> ();
		DefaultColor = rend.material.color;

		unitmanager = UnitManager.instance;
		currNode = GridSystem._instance.GetNode (5, 5);
		Debug.Log ( currNode.transform.position );
		transform.position = new Vector3 (currNode.transform.position.x, 0.5f, currNode.transform.position.z);
		Debug.Log (transform.position);
	}

	// Run only when Mouse click onto the unit
	void OnMouseDown()
	{
		if (unitmanager.AbleToChangeUnit)
		{
			Debug.Log ("Standard unit selected.");
			unitmanager.AbleToChangeUnit = false;
			unitmanager.SetUnitToDoActions (unitmanager.TestingPlayer);

			// Reset variables
			nextNode = null;
			unitmanager.StoppedMoving = false;
		}
	}

	// Run only when Mouse cursor move into the unit collision box
	void OnMouseEnter()
	{
		if (unitmanager.AbleToChangeUnit)
		{
			// Change Color of unit to HoverColor
			rend.material.color = HoverColor;
		}
	}

	// Run only when Mouse cursor move out of the unit collision box
	void OnMouseExit()
	{
		if (unitmanager.AbleToChangeUnit)
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
		if (!unitmanager.AbleToMove && !unitmanager.StoppedMoving)
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
				unitmanager.AbleToChangeUnit = true;
				unitmanager.SetUnitToDoActions (null);
				rend.material.color = DefaultColor;

				unitmanager.StoppedMoving = true;

				currNode = nextNode;
				nextNode = null;
			}
		}
	}

	// Get & Set Current Node
	public Nodes GetCurrNode() {return currNode;}
	public void SetCurrNode(Nodes _currnode) {currNode = _currnode;}

	// Get & Set Next Node
	public Nodes GetNextNode() {return nextNode;}
	public void SetNextNode(Nodes _nextnode) {nextNode = _nextnode;}
}
