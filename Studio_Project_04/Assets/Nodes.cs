using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{
	// Serializable private variable defining Node
	[SerializeField]
	Color HoverColor;

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

		unitmanager = UnitManager.instance;
	}

	// Run only when Mouse click onto the unit
	void OnMouseDown()
	{
		Debug.Log ("Clicked on Node.");
		if (unitmanager.GetUnitToDoActions () != null && unitmanager.AbleToMove)
		{
			Debug.Log ("Unit moved.");
			unitmanager.AbleToMove = false;

			// Set selected unit's target to this node's position
			Units selectedUnitClass = unitmanager.GetUnitToDoActions ().GetComponent<Units> ();
			if (selectedUnitClass != null)
			{
				selectedUnitClass.target = transform;
			}

			// Change Color of Node back to DefaultColor
			rend.material.color = DefaultColor;
		}
	}

	// Run only when Mouse cursor move into the node collision box
	void OnMouseEnter()
	{
		if (unitmanager.GetUnitToDoActions () != null && unitmanager.AbleToMove)
		{
			// Change Color of Node to HoverColor
			rend.material.color = HoverColor;
		}
	}

	// Run only when Mouse cursor move out of the node collision box
	void OnMouseExit()
	{
		if (unitmanager.GetUnitToDoActions () != null && unitmanager.AbleToMove)
		{
			// Change Color of Node back to DefaultColor
			rend.material.color = DefaultColor;
		}
	}
}
