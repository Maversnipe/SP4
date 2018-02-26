using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

	/*=== For Player Movement ===*/
	// To show if the tile can be selectable
	private bool selectable;
	// To show if the tile is on the path
	private bool isPath;
	// The colour of tile when it is selectable
	private Color SelectableColor;
	// The distance away from the current tile (For showing selectable tiles)
		// Measured in num of tiles
	private int dist;
	// The Parent Node
	private Nodes parent;

	// Text to render on node
	TextMesh amtOfAP;

	void Start ()
	{
		// Code Optimising - Get Renderer Component once only
		rend = GetComponent<Renderer> ();
		HoverAlpha = HoverColor.a;
		rend.material.color = HoverColor;
		// Code Optimising - Get UnitManager instance once only
		playerManager = PlayerManager.Instance;
		// Set selectable color
		SelectableColor = Color.green;
		SelectableColor.a = 0.6f;
		// Set parent to null
		parent = null;
		// Set the dist to 0
		dist = 0;
		// Initialise amtOfAP to the text mesh component of this gameobject's child
		amtOfAP = gameObject.transform.GetChild (0).gameObject.GetComponent <TextMesh>();
	}

	// Run only when Mouse click onto the unit
	void OnMouseDown()
	{
		// Return if node is blocked by UI elements
		if (EventSystem.current.IsPointerOverGameObject ())
			return;
		
		// Check if unit is available
		if (playerManager.GetSelectedUnit () != null)
		{
			// if unit can move
			if (playerManager.GetAbleToMove ())
			{
				// Get information from Units class
				Players selectedUnitClass = playerManager.GetSelectedUnit ().GetComponent<Players> ();                

				// Check if the clicked node is selectable
				if(this.selectable)
				{
					// Set Amount Of AP Text
					amtOfAP.text = "";
					// Set the player's path
					playerManager.SetPath (this);
					// Set the player's next node
					if (selectedUnitClass != null)
						selectedUnitClass.SetNextNode (selectedUnitClass.GetPath ().Pop());
				}
			}
		}
	}

	// Run only when Mouse cursor move into the node collision box
	// Visual feedback for player, show that he/she can clicked on these nodes
	void OnMouseEnter()
	{
		// Return if node is blocked by UI elements
		if (EventSystem.current.IsPointerOverGameObject ())
			return;
		
		// Check if unit is available
		if (playerManager.GetSelectedUnit () != null)
		{
			// if unit can move
			if (playerManager.GetAbleToMove ()) 
			{
				Players selectedUnitClass = playerManager.GetSelectedUnit ().GetComponent<Players> ();
				Nodes unitCurrNode = selectedUnitClass.GetCurrNode ();

				if (this.selectable) 
				{
					// Change Visibility of Node to opague
					HoverColor.a = 1.0f;
					rend.material.color = HoverColor;
					// Set Amount Of AP Text
					amtOfAP.text = dist.ToString () + " AP";
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
				// Change the color of the node back to its normal color
				ChangeColour ();
				// Set Amount Of AP Text
				amtOfAP.text = "";
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

	// Change tile's colour
	public void ChangeColour()
	{
		// If it is selectable, change the colour to the selectable colour
		// Else change it back to original
		if(selectable || isPath)
		{ 
			rend.material.color = SelectableColor;
		}
		else
		{
			// Change Visibility of Node back to translucent
			HoverColor.a = HoverAlpha;
			rend.material.color = HoverColor;
		}
	}

	// Get grid's index
	public int GetXIndex() {return X;}
	public int GetZIndex() {return Z;}
	public GameObject GetOccupied() {return _OccupiedBy;}

	// Get & Set Selectable
	public bool GetSelectable() {return selectable;}
	public void SetSelectable(bool _select) {selectable = _select;}

	// Get & Set Dist
	public int GetDist() {return dist;}
	public void SetDist(int _dist) {dist = _dist;}

	// Get & Set Parent
	public Nodes GetParent() {return parent;}
	public void SetParent(Nodes _parent) {parent = _parent;}

	// Get & Set Is Path
	public bool GetIsPath() {return isPath;}
	public void SetIsPath(bool _path) {isPath = _path;}
}