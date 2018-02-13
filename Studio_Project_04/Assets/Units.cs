using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
	// Serializable private variable defining Unit
	[SerializeField]
	Color HoverColor;
	[SerializeField]
	float speed = 10.0f;

	// Reference to the unit's Components
	private Renderer rend;
	private Color DefaultColor;

	// Reference to the target
	public Transform target = null;

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
		if (unitmanager.AbleToChangeUnit)
		{
			Debug.Log ("Standard unit selected.");
			unitmanager.AbleToChangeUnit = false;
			unitmanager.SetUnitToDoActions (this.gameObject);
            Camera.main.transform.position = new Vector3(unitmanager.GetUnitToDoActions().transform.position.x, Camera.main.transform.position.y, unitmanager.GetUnitToDoActions().transform.position.z);
            // Reset variables
            target = null;
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
		if (target == null)
			return;
		
		// Move the unit to clicked Node position
		if (!unitmanager.AbleToMove && !unitmanager.StoppedMoving)
		{
			// Movement section
			Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
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
			}
		}
	}
}
