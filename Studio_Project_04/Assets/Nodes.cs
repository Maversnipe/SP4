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

	// Code Optimising - Get Renderer Component once only
	void Start ()
	{
		rend = GetComponent<Renderer> ();
		DefaultColor = rend.material.color;
	}

	// Run only when Mouse cursor move into the node collision box
	void OnMouseEnter()
	{
		// Change Color of Node to HoverColor
		rend.material.color = HoverColor;
	}

	// Run only when Mouse cursor move out of the node collision box
	void OnMouseExit()
	{
		// Change Color of Node back to DefaultColor
		rend.material.color = DefaultColor;
	}
}
