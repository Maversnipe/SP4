using System.Collections;
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
	public Armor _Armor;

	[SerializeField]
	public Weapon _Weapon;

	[SerializeField]
	public EnemyStrategy Personality;

	[SerializeField]
	int HP;

	[SerializeField]
	int AP;

	[SerializeField]
	public Faction FactionType;

	Vector3 TargetMovement;

	List<Nodes> m_visited = new List<Nodes>();
	Nodes PrevNode;
	Nodes CurrNode;

	// Use this for initialization
	void Start () {
		CurrNode = FindObjectOfType<GridSystem> ().GetNode (Random.Range (0, FindObjectOfType<GridSystem> ().getRows ()),
															Random.Range (0, FindObjectOfType<GridSystem> ().getColumn ()));
		m_visited.Add (CurrNode);

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
			if (CurrNode.GetZIndex () == FindObjectOfType<GridSystem> ().getColumn () - 1) {
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
			break;
		case(2): // Right
			if (CurrNode.GetXIndex () == FindObjectOfType<GridSystem> ().getRows () - 1) {
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
			break;
		case(3): // Down
			if (CurrNode.GetZIndex () == 0) {
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
			break;
		case(4): // Left
			if (CurrNode.GetXIndex () == 0) {
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
			break;
		}
		AP--;
	}

	void StrategicAction()
	{

	}
}
