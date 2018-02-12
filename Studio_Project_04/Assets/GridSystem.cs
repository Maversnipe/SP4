using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour {


	public GameObject Ground;
	public int Width = 10;
	public int Height = 10;

	private GameObject[,] Grid;

	// Use this for initialization
	void Awake()
	{
		Grid = new GameObject[Width, Height];
		for (int x = 0; x < Width; ++x)
		{
			for (int z = 0; z < Height; ++z)
			{
				GameObject GridGround = (GameObject)Instantiate (Ground);
				GridGround.transform.position = new Vector3 (GridGround.transform.position.x + x,
					GridGround.transform.position.y, GridGround.transform.position.z + z);
				Grid [x, z] = GridGround;
			}
		}

	}
}
