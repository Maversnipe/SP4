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

	// AI Unit's ID
	private int ID;
	private static int AICount = 0;
	private float counter = 0.0f;

	Nodes currNode;
	Nodes nextNode;

	GameObject EnemyTarget;

	// Use this for initialization
	void Start () {
		//Gathers the name from the Unit Variable
		Stats = this.gameObject.GetComponent<UnitVariables> ();
		//Gathers the stats from the Json File
		Stats.Copy(UnitDatabase.Instance.FetchUnitByName (Stats.Name));

		// Set AI Unit's ID
		ID = AICount;
		++AICount;

		// Set the AI starting Position & occupies the current node
		currNode = GridSystem.Instance.GetNode(Random.Range(0,GridSystem.Instance.GetRows()),Random.Range(0,GridSystem.Instance.GetColumn()));
		//currNode = GridSystem.Instance.GetNode(0,0);
		currNode.SetOccupied (this.gameObject);
		this.transform.position = currNode.transform.position;
		TargetMovement = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<UnitVariables> ().Copy (Stats);


		if (Stats.AP != 0) {
			print (this.gameObject.name + " " + Stats.AP);
			if ((this.transform.position - TargetMovement).magnitude < 0.1f) {
				switch (Personality) {
				case(EnemyStrategy.AGGRESSIVE):
					AggressiveAction ();
					break;
				case(EnemyStrategy.DEFENSIVE):
					break;
				case(EnemyStrategy.RANDOM):
					RandomAction ();
					break;
				case(EnemyStrategy.STRATEGIC):
					break;
				}

				TargetMovement = currNode.transform.position;
			}
		} else {
			TurnEnd ();
		}

		this.transform.position += (TargetMovement - this.transform.position).normalized * 0.05f;
	}

	void AggressiveAction()
	{
		if (EnemyTarget == null) {

			GameObject Temp = null;

			for (int x = 0; x < GridSystem.Instance.GetRows (); ++x) {
				for (int z = 0; z < GridSystem.Instance.GetColumn (); ++z) {
					if (GridSystem.Instance.GetNode (x, z).GetOccupied () != null &&
						GridSystem.Instance.GetNode (x, z).GetOccupied ().tag == "PlayerUnit") {
						if (Temp == null) {
							Temp = GridSystem.Instance.GetNode (x, z).GetOccupied ();
						} else {
							if ((GridSystem.Instance.GetNode (x, z).GetOccupied ().transform.position - currNode.transform.position).magnitude > (Temp.transform.position - currNode.transform.position).magnitude) {
								Temp = GridSystem.Instance.GetNode (x, z).GetOccupied ();
							}
						}
					}
				}
			}

			EnemyTarget = Temp;
		} else {
			
			if ((currNode.GetZIndex() != GridSystem.Instance.GetColumn() - 1 && GridSystem.Instance.GetNode (currNode.GetXIndex (), currNode.GetZIndex () + 1).GetOccupied () == EnemyTarget)
				|| (currNode.GetZIndex () != 0 && GridSystem.Instance.GetNode (currNode.GetXIndex (), currNode.GetZIndex () - 1).GetOccupied () == EnemyTarget)
				|| (currNode.GetXIndex () != GridSystem.Instance.GetRows() - 1 && GridSystem.Instance.GetNode (currNode.GetXIndex () + 1, currNode.GetZIndex ()).GetOccupied () == EnemyTarget)
				|| (currNode.GetXIndex () != 0 && GridSystem.Instance.GetNode (currNode.GetXIndex () - 1, currNode.GetZIndex ()).GetOccupied () == EnemyTarget)) {
				EnemyTarget.GetComponent<UnitVariables> ().HP--;
				print ("Player Hit");
			} else {
				float UpMag = (currNode.transform.position - EnemyTarget.transform.position).magnitude,
				RightMag = (currNode.transform.position - EnemyTarget.transform.position).magnitude,
				DownMag = (currNode.transform.position - EnemyTarget.transform.position).magnitude,
				LeftMag = (currNode.transform.position - EnemyTarget.transform.position).magnitude;

				if (currNode.GetZIndex () != GridSystem.Instance.GetColumn() - 1) {
					UpMag = (GridSystem.Instance.GetNode (currNode.GetXIndex(), currNode.GetZIndex() + 1).transform.position - EnemyTarget.transform.position).magnitude;
				}
				if (currNode.GetXIndex () != GridSystem.Instance.GetRows() - 1) {
					RightMag = (GridSystem.Instance.GetNode (currNode.GetXIndex() + 1, currNode.GetZIndex()).transform.position - EnemyTarget.transform.position).magnitude;
				}
				if (currNode.GetZIndex () != 0) {
					DownMag = (GridSystem.Instance.GetNode (currNode.GetXIndex(), currNode.GetZIndex() - 1).transform.position - EnemyTarget.transform.position).magnitude;
				}
				if (currNode.GetXIndex () != 0) {
					LeftMag = (GridSystem.Instance.GetNode (currNode.GetXIndex() - 1, currNode.GetZIndex()).transform.position - EnemyTarget.transform.position).magnitude;
				}

				float Closest = Mathf.Min (UpMag, RightMag, DownMag, LeftMag);

				if (UpMag == Closest) {
					if (GridSystem.Instance.GetNode (currNode.GetXIndex (), currNode.GetZIndex () + 1).GetOccupied() != null) {
						Closest = Mathf.Min (RightMag, DownMag, LeftMag);
					} else {
						//print (UpMag + " vs " + Closest);
						currNode.SetOccupiedNULL();
						currNode = GridSystem.Instance.GetNode (currNode.GetXIndex(), currNode.GetZIndex() + 1);
						currNode.SetOccupied (this.gameObject);
						print ("Up Mag = " + UpMag);
					}
				}

				if (RightMag == Closest) {
					//print (RightMag + " vs " + Closest);
					currNode.SetOccupiedNULL();
					currNode = GridSystem.Instance.GetNode (currNode.GetXIndex() + 1, currNode.GetZIndex());
					currNode.SetOccupied (this.gameObject);
					print ("Right Mag = " + RightMag);
				}

				if (DownMag == Closest) {
					//print (DownMag + " vs " + Closest);
					currNode.SetOccupiedNULL();
					currNode = GridSystem.Instance.GetNode (currNode.GetXIndex(), currNode.GetZIndex() - 1);
					currNode.SetOccupied (this.gameObject);
					print ("Down Mag = " + DownMag);
				}

				if (LeftMag == Closest) {
					//print (LeftMag + " vs " + Closest);
					currNode.SetOccupiedNULL();
					currNode = GridSystem.Instance.GetNode (currNode.GetXIndex() - 1, currNode.GetZIndex());
					currNode.SetOccupied (this.gameObject);
					print ("Left Mag = " + LeftMag);
				}
			}
			this.Stats.AP--;
		}
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
