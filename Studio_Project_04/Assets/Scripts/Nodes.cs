using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{
	// Serializable private variable defining Node
	[SerializeField]
	Color HoverColor;

	// X and Z Index
	private int X = 0;
	private int Z = 0;

	// Reference to the Node's Components
	private Renderer rend;
	private Color DefaultColor;

	// Reference to the UnitManager's instance
	private UnitManager unitmanager;

	void Start ()
	{
		// Code Optimising - Get Renderer Component once only
		rend = GetComponent<Renderer> ();
		DefaultColor = rend.material.color;
		// Code Optimising - Get UnitManager instance once only
		unitmanager = UnitManager.instance;
	}

	// Run only when Mouse click onto the unit
	void OnMouseDown()
	{
		// Check if unit is available and if unit can move
		if (unitmanager.GetUnitToDoActions () != null && unitmanager.AbleToMove)
		{
			// Get information from Units class
			Units selectedUnitClass = unitmanager.GetUnitToDoActions ().GetComponent<Units> ();
			Nodes unitCurrNode = selectedUnitClass.GetCurrNode ();

			// Limits move range to one grid from the player current node
			if ((unitCurrNode.GetXIndex () + 1 == this.X && unitCurrNode.GetZIndex () == this.Z) ||
				(unitCurrNode.GetXIndex () - 1 == this.X && unitCurrNode.GetZIndex () == this.Z) ||
				(unitCurrNode.GetZIndex () + 1 == this.Z && unitCurrNode.GetXIndex () == this.X) ||
				(unitCurrNode.GetZIndex () - 1 == this.Z && unitCurrNode.GetXIndex () == this.X))
			{
				Debug.Log ("Node Selected.");
				unitmanager.AbleToMove = false;

				// Set selected unit's target to this node's position
				if (selectedUnitClass != null)
					selectedUnitClass.SetNextNode (this);

				// Change Color of Node back to DefaultColor
				rend.material.color = DefaultColor;	
			}
		}
	}

	// Run only when Mouse cursor move into the node collision box
	// Visual feedback for player, show that he/she can clicked on these nodes
	void OnMouseEnter()
	{
		// Check if unit is available and if unit can move
		if (unitmanager.GetUnitToDoActions () != null && unitmanager.AbleToMove)
		{
			Units selectedUnitClass = unitmanager.GetUnitToDoActions ().GetComponent<Units> ();
			Nodes unitCurrNode = selectedUnitClass.GetCurrNode ();

			// Limits move range to one grid from the player current node
			if ((unitCurrNode.GetXIndex () + 1 == this.X && unitCurrNode.GetZIndex () == this.Z) ||
				(unitCurrNode.GetXIndex () - 1 == this.X && unitCurrNode.GetZIndex () == this.Z) ||
				(unitCurrNode.GetZIndex () + 1 == this.Z && unitCurrNode.GetXIndex () == this.X) ||
				(unitCurrNode.GetZIndex () - 1 == this.Z && unitCurrNode.GetXIndex () == this.X))
			{
				// Change Color of Node to HoverColor
				rend.material.color = HoverColor;
			}
		}
	}

	// Run only when Mouse cursor move out of the node collision box
	void OnMouseExit()
	{
		// Check if unit is available and if unit can move
		if (unitmanager.GetUnitToDoActions () != null && unitmanager.AbleToMove)
		{
			// Change Color of Node back to DefaultColor
			rend.material.color = DefaultColor;
		}
	}

	// Set the grid's index
	public void SetIndex(int _x, int _z)
	{
		X = _x;
		Z = _z;
	}

	// Get grid's index
	public int GetXIndex() {return X;}
	public int GetZIndex() {return Z;}
}
