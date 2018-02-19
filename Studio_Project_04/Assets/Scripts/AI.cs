﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStrategy{
	AGGRESSIVE,
	RANDOM,
	DEFENSIVE,
	STRATEGIC
};

public enum Faction{
	FRIENDLY,
	NEUTRAL,
	ENEMY,
	HATEFUL_NEUTRAL,
};

public class AI : MonoBehaviour {

	[SerializeField]
	public string Name;

	public Unit _unit;

	[SerializeField]
	public EnemyStrategy Personality;

	// TO BE CHANGED TO PRIVATE LATER
	[SerializeField]
	int HP;

	// TO BE CHANGED TO PRIVATE LATER
	[SerializeField]
	int AP;

	[SerializeField]
	public Faction FactionType;

	Vector3 TargetMovement;

	List<Nodes> m_visited = new List<Nodes>();
	List<Nodes> m_path = new List<Nodes>();
	Nodes PrevNode;
	Nodes CurrNode;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < UnitDatabase.Database.Count; ++i) {
			if (Name == UnitDatabase.Database [i].getName ()) {
				_unit = UnitDatabase.Database [i];
				break;
			}
		}

		if (_unit == null) {
			Destroy (this.gameObject);
		} else {
			HP = _unit.getHP ();
			AP = _unit.getAP ();
		}

		CurrNode = FindObjectOfType<GridSystem> ().GetNode (Random.Range (0, FindObjectOfType<GridSystem> ().GetRows ()),
															Random.Range (0, FindObjectOfType<GridSystem> ().GetColumn ()));
		m_visited.Add (CurrNode);
		CurrNode.SetOccupied (_unit);

		this.transform.position = CurrNode.transform.position;
		TargetMovement = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (AP != 0) {
			if ((this.transform.position - TargetMovement).magnitude < 0.01f) {
				// Determines which is the next area to go to
				switch (Personality) {
				case EnemyStrategy.AGGRESSIVE:
					AggressiveAction ();
					break;
				case EnemyStrategy.DEFENSIVE:
					DefensiveAction ();
					break;
				case EnemyStrategy.RANDOM:
					RandomAction ();
					break;
				case EnemyStrategy.STRATEGIC:
					StrategicAction ();
					break;
				}

				TargetMovement = CurrNode.transform.position;
			}
		} else {
			m_visited.Clear ();
			m_visited.Add (CurrNode);
		}
		this.transform.position += (TargetMovement - this.transform.position).normalized * 0.05f;
	}

	Faction getFactionType (){
		return FactionType;
	}

	void AggressiveAction()
	{

	}

	void DefensiveAction()
	{

	}

	void RandomAction()
	{
		int Choice = Random.Range (1, 5);
		switch (Choice) {
		case(1): // Up
			if (CurrNode.GetZIndex () == FindObjectOfType<GridSystem> ().GetColumn () - 1
				|| CurrNode.GetOccupied() == null) {
				return;
			}
			PrevNode = CurrNode;
			CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex (), CurrNode.GetZIndex () + 1);

			for (int i = 0; i < m_visited.Count; i++) {
				//Checks if the current random node was visited before.
				if (CurrNode.GetXIndex () == m_visited [i].GetXIndex () &&
				    CurrNode.GetZIndex () == m_visited [i].GetZIndex ()) {
					CurrNode = PrevNode;
					return;
				}
			}

			m_visited.Add (CurrNode);
			PrevNode.SetOccupiedNULL ();
			CurrNode.SetOccupied (_unit);
			break;
		case(2): // Right
			if (CurrNode.GetXIndex () == FindObjectOfType<GridSystem> ().GetRows () - 1
				|| CurrNode.GetOccupied() == null) {
				return;
			}

			PrevNode = CurrNode;
			CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex () + 1, CurrNode.GetZIndex ());

			for (int i = 0; i < m_visited.Count; i++) {
				//Checks if the current random node was visited before.
				if (CurrNode.GetXIndex () == m_visited [i].GetXIndex () &&
					CurrNode.GetZIndex () == m_visited [i].GetZIndex ()) {
					CurrNode = PrevNode;
					return;
				}
			}

			m_visited.Add (CurrNode);
			PrevNode.SetOccupiedNULL ();
			CurrNode.SetOccupied (_unit);
			break;
		case(3): // Down
			if (CurrNode.GetZIndex () == 0
				|| CurrNode.GetOccupied() == null) {
				return;
			}

			PrevNode = CurrNode;
			CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex (), CurrNode.GetZIndex () - 1);

			for (int i = 0; i < m_visited.Count; i++) {
				//Checks if the current random node was visited before.
				if (CurrNode.GetXIndex () == m_visited [i].GetXIndex () &&
				    CurrNode.GetZIndex () == m_visited [i].GetZIndex ()) {
					CurrNode = PrevNode;
					return;
				}
			}

			m_visited.Add (CurrNode);
			PrevNode.SetOccupiedNULL ();
			CurrNode.SetOccupied (_unit);
			break;
		case(4): // Left
			if (CurrNode.GetXIndex () == 0
				|| CurrNode.GetOccupied() == null) {
				return;
			}

			PrevNode = CurrNode;
			CurrNode = FindObjectOfType<GridSystem> ().GetNode (CurrNode.GetXIndex () - 1, CurrNode.GetZIndex ());

			for (int i = 0; i < m_visited.Count; i++) {
				//Checks if the current random node was visited before.
				if (CurrNode.GetXIndex () == m_visited [i].GetXIndex () &&
				    CurrNode.GetZIndex () == m_visited [i].GetZIndex ()) {
					CurrNode = PrevNode;
					return;
				}
			}

			m_visited.Add (CurrNode);
			PrevNode.SetOccupiedNULL ();
			CurrNode.SetOccupied (_unit);
			break;
		}
		AP--;
	}

	void StrategicAction()
	{

	}

	void BFS(Nodes Start, Nodes End)
	{
		
	}

	void DFS()
	{
		
	}
}
