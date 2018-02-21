using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nodes : MonoBehaviour
{
	// Serializable private variable defining Node
	[SerializeField]
	Color HoverColor;

	// X and Z Index
	private int X = 0;
	private int Z = 0;

	// HoverColor's alpha value
	private float HoverAlpha;

	// Reference to the Node's Components
	private Renderer rend;

	// Reference to the UnitManager's instance
	private PlayerManager playerManager;

	// Reference to the unit currently on the node
	private GameObject _OccupiedBy;

	void Start ()
	{
		// Code Optimising - Get Renderer Component once only
		rend = GetComponent<Renderer> ();
		HoverAlpha = HoverColor.a;
		rend.material.color = HoverColor;
		// Code Optimising - Get UnitManager instance once only
		playerManager = PlayerManager.Instance;
	}

	// Run only when Mouse click onto the unit
	void OnMouseDown()
	{
		// Check if unit is available
		if (playerManager.GetSelectedUnit () != null)
		{
			// if unit can move
			if (playerManager.GetAbleToMove ())
			{
				// Get information from Units class
				Players selectedUnitClass = playerManager.GetSelectedUnit ().GetComponent<Players> ();
				Nodes unitCurrNode = selectedUnitClass.GetCurrNode ();

				// Limits move range to one grid from the player current node
				if ((unitCurrNode.GetXIndex () + 1 == this.X && unitCurrNode.GetZIndex () == this.Z) ||
				   (unitCurrNode.GetXIndex () - 1 == this.X && unitCurrNode.GetZIndex () == this.Z) ||
				   (unitCurrNode.GetZIndex () + 1 == this.Z && unitCurrNode.GetXIndex () == this.X) ||
				   (unitCurrNode.GetZIndex () - 1 == this.Z && unitCurrNode.GetXIndex () == this.X))
				{

					// Set selected unit's target to this node's position
					if (selectedUnitClass != null)
						selectedUnitClass.SetNextNode (this);

					// Change Visibility of Node back to translucent
					HoverColor.a = HoverAlpha;
					rend.material.color = HoverColor;
				}
			}

			// if unit can attack
			if (playerManager.GetAbleToAttack ())
			{
				// Get information from Units class
				Players selectedUnitClass = playerManager.GetSelectedUnit ().GetComponent<Players> ();
				Nodes unitCurrNode = selectedUnitClass.GetCurrNode ();

				// Limits move range to one grid from the player current node
				if (_OccupiedBy)
				{
					if ((unitCurrNode.GetXIndex () + 1 == this.X && unitCurrNode.GetZIndex () == this.Z) ||
					   (unitCurrNode.GetXIndex () - 1 == this.X && unitCurrNode.GetZIndex () == this.Z) ||
					   (unitCurrNode.GetZIndex () + 1 == this.Z && unitCurrNode.GetXIndex () == this.X) ||
					   (unitCurrNode.GetZIndex () - 1 == this.Z && unitCurrNode.GetXIndex () == this.X))
					{
						AI enemy = _OccupiedBy.GetComponent<AI> ();
						int damageDeal = playerManager.CalculateDamage (selectedUnitClass, enemy);

						enemy.GetStats ().HP -= damageDeal;
						if (enemy.GetStats ().HP <= 0)
						{
							Destroy (_OccupiedBy);
							SceneManager.LoadScene ("SceneCleared");
						}

					}
				}
			}
		}
	}

	// Run only when Mouse cursor move into the node collision box
	// Visual feedback for player, show that he/she can clicked on these nodes
	void OnMouseEnter()
	{
		// Check if unit is available
		if (playerManager.GetSelectedUnit () != null)
		{
			// if unit can move
			if (playerManager.GetAbleToMove ()) 
			{
				Players selectedUnitClass = playerManager.GetSelectedUnit ().GetComponent<Players> ();
				Nodes unitCurrNode = selectedUnitClass.GetCurrNode ();

				// Limits move range to one grid from the player current node
				if ((unitCurrNode.GetXIndex () + 1 == this.X && unitCurrNode.GetZIndex () == this.Z) ||
				   (unitCurrNode.GetXIndex () - 1 == this.X && unitCurrNode.GetZIndex () == this.Z) ||
				   (unitCurrNode.GetZIndex () + 1 == this.Z && unitCurrNode.GetXIndex () == this.X) ||
				   (unitCurrNode.GetZIndex () - 1 == this.Z && unitCurrNode.GetXIndex () == this.X))
				{
					// Change Visibility of Node to opague
					HoverColor.a = 1.0f;
					rend.material.color = HoverColor;
				}
			}
			// if unit can attack
			if (playerManager.GetAbleToAttack ())
			{
				// Get information from Units class
				Players selectedUnitClass = playerManager.GetSelectedUnit ().GetComponent<Players> ();
				Nodes unitCurrNode = selectedUnitClass.GetCurrNode ();

				// Limits move range to one grid from the player current node
				if (_OccupiedBy)
				{
					if(unitCurrNode.GetXIndex () + 1 == this.X && unitCurrNode.GetZIndex () == this.Z)
						Debug.Log ("Right Node Occupied.");
					if(unitCurrNode.GetXIndex () - 1 == this.X && unitCurrNode.GetZIndex () == this.Z)
						Debug.Log ("Left Node Occupied.");
					if(unitCurrNode.GetZIndex () + 1 == this.Z && unitCurrNode.GetXIndex () == this.X)
						Debug.Log ("Up Node Occupied.");
					if(unitCurrNode.GetZIndex () - 1 == this.Z && unitCurrNode.GetXIndex () == this.X)
						Debug.Log ("Down Node Occupied.");
					
					// Change Visibility of Node to opague
					HoverColor.a = 1.0f;
					rend.material.color = HoverColor;
				}
			}
		}
	}

	// Run only when Mouse cursor move out of the node collision box
	void OnMouseExit()
	{
		// Check if unit is available and if unit can move
		if (playerManager.GetSelectedUnit () != null)
		{
			if (playerManager.GetAbleToMove () || playerManager.GetAbleToAttack ()) 
			{
				// Change Visibility of Node back to translucent
				HoverColor.a = HoverAlpha;
				rend.material.color = HoverColor;
			}
		}
	}

	// Set the grid's index
	public void SetIndex(int _x, int _z)
	{
		X = _x;
		Z = _z;
	}

	// Sets the unit's reference if the node is currently being taken
	public void SetOccupied(GameObject n_NewUnit)
	{
		_OccupiedBy = n_NewUnit;
	}

	// Resets the unit reference to null
	public void SetOccupiedNULL()
	{
		_OccupiedBy = null;
	}

	// Get grid's index
	public int GetXIndex() {return X;}
	public int GetZIndex() {return Z;}
	public GameObject GetOccupied() {return _OccupiedBy;}
}
