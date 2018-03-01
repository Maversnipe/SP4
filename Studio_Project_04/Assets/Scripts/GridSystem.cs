using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridSystem : GenericSingleton<GridSystem>
{
	// Serializable private variables defining Grid
	[SerializeField]
	GameObject Ground;
	[SerializeField]
	int Rows = 10;
	[SerializeField]
	int Columns = 10;

	// 2D Array of GameObjects(Grounds)
	private GameObject[,] Grid;

	// Dimensions of the whole Grid
	private float width;
	private float height;

	void Update()
	{
		if (!SceneManager.GetActiveScene ().Equals (SceneManager.GetSceneByName ("SceneBase")))
			return;
		if (BattleManager.Instance.GetGameMode () == GAMEMODE.NONE)
			MakeGrid ();
	}

	// Make Grid
	public void MakeGrid()
	{
		// Create Grid based on input number of rows and columns
		Grid = new GameObject[Rows, Columns];

		for (int x = 0; x < Rows; ++x)
		{
			for (int z = 0; z < Columns; ++z)
			{
				// Create a Ground
				GameObject GridGround = (GameObject)Instantiate (Ground);

				// Set the position of Ground Object according to Grid
				// (0.2f * xz) - this determines the size of gap in between the Grounds
				GridGround.transform.position = new Vector3 (GridGround.transform.position.x + x + (0.2f * x),
					GridGround.transform.position.y, GridGround.transform.position.z + z + (0.2f * z));

				// Get information from Nodes class
				Nodes GroundNode = GridGround.GetComponent <Nodes> ();
				// Set Index from Grid onto each Node
				GroundNode.SetIndex (x, z);

				// Put Ground Object into the Grid Array
				Grid [x, z] = GridGround;
			}
		}
		// Calculating the dimensions of the Grid
		width = (Rows - 1) + (0.2f * (Rows - 1));
		height = (Columns - 1) + (0.2f * (Columns - 1));
	}

	// Return Grid Array
	public GameObject [,] GetGrid () { return Grid; }
	// Return Node with passed in index x and z in the Grid
	public Nodes GetNode(int _X, int _Z) { return Grid [_X, _Z].GetComponent <Nodes>(); }

	// Return number of rows of the Grid
	public int GetRows() { return Rows; }
	// Return number of columns of the Grid
	public int GetColumn() { return Columns; }

	// Return Width of Grid
	public float GetWidth() { return width; }
	// Return Height of Grid
	public float GetHeight() { return height; }
}
