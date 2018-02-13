using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
	public static GridSystem _instance;
	// Serializable private variables defining Grid
	[SerializeField]
	GameObject Ground;
	[SerializeField]
	int Rows = 10;
	[SerializeField]
	int Columns = 10;

	// 2D Array of GameObjects(Grounds)
	private GameObject[,] Grid;

	void Awake()
	{
		// Check if there is already an instance of the class
		if(_instance != null)
		{
			Debug.LogError ("More than one GridSystem in scene!");
			return;
		}
		_instance = this;

		// Create Grid based on inputted number of rows and columns
		Grid = new GameObject[Rows, Columns];

		for (int x = 0; x < Rows; ++x)
		{
			for (int z = 0; z < Columns; ++z)
			{
				// Create a Ground
				GameObject GridGround = (GameObject)Instantiate (Ground);

				// Set the position according to Grid
				// (0.2f * xz) - this determines the size of gap in between the Grounds
				GridGround.transform.position = new Vector3 (GridGround.transform.position.x + x + (0.2f * x),
					GridGround.transform.position.y, GridGround.transform.position.z + z + (0.2f * z));
				
				Nodes GroundNode = GridGround.GetComponent <Nodes> ();
				GroundNode.SetIndex (x, z);

				// Put it into the Grid Array
				Grid [x, z] = GridGround;
			}
		}
	}

	public GameObject [,] GetGrid ()
	{
		return Grid;
	}

	public Nodes GetNode(int _X, int _Z)
	{
		Debug.Log ("X: " + Grid [_X, _Z].GetComponent <Nodes> ().GetXIndex () + " Z: " + Grid [_X, _Z].GetComponent <Nodes> ().GetZIndex ());
		return Grid [_X, _Z].GetComponent <Nodes>();
	}

}
