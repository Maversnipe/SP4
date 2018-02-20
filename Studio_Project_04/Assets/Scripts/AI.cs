using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStrategy{
	AGGRESSIVE,
	RANDOM,
	DEFENSIVE,
	STRATEGIC
};

public class AI : MonoBehaviour {

	[SerializeField]
	public EnemyStrategy Personality;

	Vector3 TargetMovement;

	private UnitVariables Stats;
	private GridSystem GridRef;

	// AI Unit's ID
	private int ID;
	private static int AICount = 0;
	private float counter = 0.0f;

	List<Nodes> m_visited = new List<Nodes>();
	Nodes currNode;
	Nodes nextNode;

	GameObject EnemyTarget;

	// Use this for initialization
	void Start () {
		//Gathers the name from the Unit Variable
		Stats = GetComponent<UnitVariables> ();
		//Gathers the stats from the Json File
		Stats = UnitDatabase.Instance.FetchUnitByName (Stats.Name);

		// Set AI Unit's ID
		ID = AICount;
		++AICount;
	}
	
	// Update is called once per frame
	void Update () {
		// Used to see the variables change in real time in the inspector
		GetComponent<UnitVariables> ().Debug (Stats);

		// Updates the Grid Ref with the current grid
		GridRef = FindObjectOfType<GridSystem> ();

		if (Stats.AP != 0) {

		}

		this.transform.position += (TargetMovement - this.transform.position).normalized * 0.05f;
	}

	void AggressiveAction()
	{
//		if (EnemyTarget == null) {
//
//			GameObject Temp = null;
//
//			for (int x = 0; x < FindObjectOfType<GridSystem> ().GetRows (); ++x) {
//				for (int z = 0; z < FindObjectOfType<GridSystem> ().GetColumn (); ++z) {
//					if (FindObjectOfType<GridSystem> ().GetNode (x, z).GetOccupied () != null &&
//					    FindObjectOfType<GridSystem> ().GetNode (x, z).GetOccupied ().tag == "PlayerUnit") {
//						if (Temp == null) {
//							Temp = FindObjectOfType<GridSystem> ().GetNode (x, z).GetOccupied ();
//						} else {
//							if ((FindObjectOfType<GridSystem> ().GetNode (x, z).GetOccupied ().transform.position - CurrNode.transform.position).magnitude > (Temp.transform.position - CurrNode.transform.position).magnitude) {
//								Temp = FindObjectOfType<GridSystem> ().GetNode (x, z).GetOccupied ();
//							}
//						}
//					}
//				}
//			}
//
//			EnemyTarget = Temp;
//		} else {
//			if (((FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex (), CurrNode.GetZIndex () + 1).GetOccupied() == EnemyTarget) && CurrNode.GetZIndex () != FindObjectOfType<GridSystem> ().GetColumn() - 1) // Up Check
//				|| ((FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex () + 1, CurrNode.GetZIndex ()).GetOccupied() == EnemyTarget) && CurrNode.GetXIndex () != FindObjectOfType<GridSystem> ().GetRows() - 1) // Right Check
//				|| ((FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex (), CurrNode.GetZIndex () - 1).GetOccupied() == EnemyTarget) && CurrNode.GetZIndex () != 0) // Down Check
//				|| ((FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex () - 1, CurrNode.GetZIndex ()).GetOccupied() == EnemyTarget) && CurrNode.GetXIndex () != 0)) // Left Check
//			{
//				EnemyTarget.GetComponent<Players> ().SetHP (EnemyTarget.GetComponent<Players> ().GetHP () - 1);
//			}
//			else
//			{
//				float UpMag = (CurrNode.transform.position - EnemyTarget.transform.position).magnitude,
//				RightMag = (CurrNode.transform.position - EnemyTarget.transform.position).magnitude,
//				DownMag = (CurrNode.transform.position - EnemyTarget.transform.position).magnitude,
//				LeftMag = (CurrNode.transform.position - EnemyTarget.transform.position).magnitude;
//
//				if (CurrNode.GetZIndex () != FindObjectOfType<GridSystem> ().GetColumn() - 1) {
//					UpMag = (FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex(), CurrNode.GetZIndex() + 1).transform.position - EnemyTarget.transform.position).magnitude;
//				}
//				if (CurrNode.GetXIndex () != FindObjectOfType<GridSystem> ().GetRows() - 1) {
//					RightMag = (FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex() + 1, CurrNode.GetZIndex()).transform.position - EnemyTarget.transform.position).magnitude;
//				}
//				if (CurrNode.GetZIndex () != 0) {
//					DownMag = (FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex(), CurrNode.GetZIndex() - 1).transform.position - EnemyTarget.transform.position).magnitude;
//				}
//				if (CurrNode.GetXIndex () != 0) {
//					LeftMag = (FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex() - 1, CurrNode.GetZIndex()).transform.position - EnemyTarget.transform.position).magnitude;
//				}
//
//				float Closest = Mathf.Min (UpMag, RightMag, DownMag, LeftMag);
//
//				if (UpMag == Closest) {
//
//					//print (UpMag + " vs " + Closest);
//					CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex(), CurrNode.GetZIndex() + 1);
//					print (UpMag);
//				}
//
//				else if (RightMag == Closest) {
//					//print (RightMag + " vs " + Closest);
//					CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex() + 1, CurrNode.GetZIndex());
//					print (RightMag);
//				}
//
//				else if (DownMag == Closest) {
//					//print (DownMag + " vs " + Closest);
//					CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex(), CurrNode.GetZIndex() - 1);
//					print (DownMag);
//				}
//
//				else if (LeftMag == Closest) {
//					//print (LeftMag + " vs " + Closest);
//					CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex() - 1, CurrNode.GetZIndex());
//					print (LeftMag);
//				}
//			}
//
//			AP--;
//		}
	}

	void DefensiveAction()
	{

	}

	void RandomAction()
	{
//		int Choice = Random.Range (1, 5);
//		switch (Choice) {
//		case(1): // Up
//			if (CurrNode.GetZIndex () == FindObjectOfType<GridSystem> ().GetColumn () - 1
//				|| CurrNode.GetOccupied() == null) {
//				return;
//			}
//			PrevNode = CurrNode;
//			CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex (), CurrNode.GetZIndex () + 1);
//
//			for (int i = 0; i < m_visited.Count; i++) {
//				//Checks if the current random node was visited before.
//				if (CurrNode.GetXIndex () == m_visited [i].GetXIndex () &&
//				    CurrNode.GetZIndex () == m_visited [i].GetZIndex ()) {
//					CurrNode = PrevNode;
//					return;
//				}
//			}
//
//			m_visited.Add (CurrNode);
//			PrevNode.SetOccupiedNULL ();
//			CurrNode.SetOccupied (this.gameObject);
//			break;
//		case(2): // Right
//			if (CurrNode.GetXIndex () == FindObjectOfType<GridSystem> ().GetRows () - 1
//				|| CurrNode.GetOccupied() == null) {
//				return;
//			}
//
//			PrevNode = CurrNode;
//			CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex () + 1, CurrNode.GetZIndex ());
//
//			for (int i = 0; i < m_visited.Count; i++) {
//				//Checks if the current random node was visited before.
//				if (CurrNode.GetXIndex () == m_visited [i].GetXIndex () &&
//					CurrNode.GetZIndex () == m_visited [i].GetZIndex ()) {
//					CurrNode = PrevNode;
//					return;
//				}
//			}
//
//			m_visited.Add (CurrNode);
//			PrevNode.SetOccupiedNULL ();
//			CurrNode.SetOccupied (this.gameObject);
//			break;
//		case(3): // Down
//			if (CurrNode.GetZIndex () == 0
//				|| CurrNode.GetOccupied() == null) {
//				return;
//			}
//
//			PrevNode = CurrNode;
//			CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex (), CurrNode.GetZIndex () - 1);
//
//			for (int i = 0; i < m_visited.Count; i++) {
//				//Checks if the current random node was visited before.
//				if (CurrNode.GetXIndex () == m_visited [i].GetXIndex () &&
//				    CurrNode.GetZIndex () == m_visited [i].GetZIndex ()) {
//					CurrNode = PrevNode;
//					return;
//				}
//			}
//
//			m_visited.Add (CurrNode);
//			PrevNode.SetOccupiedNULL ();
//			CurrNode.SetOccupied (this.gameObject);
//			break;
//		case(4): // Left
//			if (CurrNode.GetXIndex () == 0
//				|| CurrNode.GetOccupied() == null) {
//				return;
//			}
//
//			PrevNode = CurrNode;
//			CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex () - 1, CurrNode.GetZIndex ());
//
//			for (int i = 0; i < m_visited.Count; i++) {
//				//Checks if the current random node was visited before.
//				if (CurrNode.GetXIndex () == m_visited [i].GetXIndex () &&
//				    CurrNode.GetZIndex () == m_visited [i].GetZIndex ()) {
//					CurrNode = PrevNode;
//					return;
//				}
//			}
//
//			m_visited.Add (CurrNode);
//			PrevNode.SetOccupiedNULL ();
//			CurrNode.SetOccupied (this.gameObject);
//			break;
//		}
//		AP--;
	}

	void StrategicAction()
	{
		
	}

	void PathCheck(Nodes Start, Nodes End)
	{
		
	}

	void DFS()
	{
		
	}

	// Init for the start of each Unit's turn
	public void TurnStart()
	{
		// Reset nextNode to null
		nextNode = null;
		// Set Camera position to be the Unit's position
		Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
	}

	// The reset for the end of each Unit's turn
	public void TurnEnd()
	{
		// If nextNode is not null, update currNode to be nextNode
		// And null nextNode
		if (nextNode)
		{
			currNode = nextNode;
			nextNode = null;
		}
	}

	// Get & Set Unit's ID
	public int GetID() {return ID;}
	public void SetID(int _id) {ID = _id;}

	// Get Unit's Stats
	public UnitVariables GetStats() {return Stats;}
}
