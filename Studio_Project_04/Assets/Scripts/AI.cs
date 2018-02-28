using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum EnemyStrategy{
	AGGRESSIVE,
	RANDOM,
	DEFENSIVE,
	STRATEGIC
};

public class AI : MonoBehaviour {
	[SerializeField]
	Color HoverColor;

	// Reference to the Node's Components
	private Renderer rend;

	[SerializeField]
	public EnemyStrategy Personality;

	Vector3 TargetMovement;

	private UnitVariables Stats;
	private int MaxAP;

	// AI Unit's ID
	private int ID;
	private static int AICount = 0;
	private float counter = 0.0f;

	Nodes currNode;
	Nodes nextNode;
	private Queue<Nodes> m_path = new Queue<Nodes>();

	Nodes EnemyTarget;

	private bool Path_Set;
	private bool isAttacking;

	// Reference to the UnitManager's instance
	private TurnManager turnManager;

	// Reference to GridSystem's Instance
	private GridSystem GridRef;

	// Use this for initialization
	void Awake () {
		rend = GetComponent<Renderer> ();

		//Gathers the name from the Unit Variable
		Stats = this.gameObject.GetComponent<UnitVariables> ();
		//Gathers the stats from the Json File
		Stats.Copy(UnitDatabase.Instance.FetchUnitByName (Stats.Name));

		MaxAP = Stats.AP;

		// Set AI Unit's ID
		ID = AICount;
		++AICount;

		// Set the AI starting Position & occupies the current node
		currNode = GridSystem.Instance.GetNode(Random.Range(0,GridSystem.Instance.GetRows()),Random.Range(0,GridSystem.Instance.GetColumn()));
		//currNode = GridSystem.Instance.GetNode(0,0);
		currNode.SetOccupied (this.gameObject);
		this.transform.position = currNode.transform.position;
		TargetMovement = this.transform.position;

		nextNode = currNode;

		turnManager = TurnManager.Instance;
		GridRef = GridSystem.Instance;

		Path_Set = false;
		isAttacking = false;
	}

	// Run only when Mouse click on the unit
	void OnMouseDown()
	{
		// Return if AI is blocked by UI elements
		if (EventSystem.current.IsPointerOverGameObject ())
			return;
		
		// if unit can attack
		if (PlayerManager.Instance.GetAbleToAttack ())
		{
			// Get information from Units class
			Players selectedUnitObject = PlayerManager.Instance.GetSelectedUnit ().GetComponent<Players> ();
			Debug.Log ("Rangeeee " + selectedUnitObject.GetStats ()._weapon.Range);
			for (int i = 1; i <= selectedUnitObject.GetStats ()._weapon.Range; ++i)
			{
				if ((selectedUnitObject.GetCurrNode ().GetXIndex () + i == currNode.GetXIndex ()
					&& selectedUnitObject.GetCurrNode ().GetZIndex () == currNode.GetZIndex ()) ||
					(selectedUnitObject.GetCurrNode ().GetXIndex () - i == currNode.GetXIndex ()
						&& selectedUnitObject.GetCurrNode ().GetZIndex () == currNode.GetZIndex ()) ||
					(selectedUnitObject.GetCurrNode ().GetZIndex () + i == currNode.GetZIndex ()
						&& selectedUnitObject.GetCurrNode ().GetXIndex () == currNode.GetXIndex ()) ||
					(selectedUnitObject.GetCurrNode ().GetZIndex () - i == currNode.GetZIndex ()
						&& selectedUnitObject.GetCurrNode ().GetXIndex () == currNode.GetXIndex ()))
				{
					// Update Opponent Unit Info
					selectedUnitObject.GetStats ().UpdateOpponentUnitInfo (this.Stats);

					int damageDeal = turnManager.CalculateDamage (selectedUnitObject.gameObject, this.gameObject);

					Stats.HP -= damageDeal;
					if (Stats.HP <= 0)
					{
						Destroy (this);
						PlayerManager.Instance.SetPlayerCount (0);
						SceneManager.LoadScene ("SceneCleared");
					}
					PlayerManager.Instance.SetAbleToAttack (false);
					// End the turn of the player node
					selectedUnitObject.TurnEnd ();
					// Change unit back to no unit
					PlayerManager.Instance.ChangeUnit (null);

					// De-Spawn Opponent Unit Info Window
					selectedUnitObject.GetStats ().SetOpponentUnitInfoWindow (false);

					break;
				}
			}
		}
	}

	// Run only when Mouse cursor move into the node collision box
	// Visual feedback for player, show that he/she can clicked on these nodes
	void OnMouseEnter()
	{
		// Return if AI is blocked by UI elements
		if (EventSystem.current.IsPointerOverGameObject ())
			return;
		
		// if unit can attack
		if (PlayerManager.Instance.GetAbleToAttack ())
		{
			// Get information from Units class
			Players selectedUnitClass = PlayerManager.Instance.GetSelectedUnit ().GetComponent<Players> ();

			// Spawn Opponent Unit Info Window
			selectedUnitClass.GetStats ().SetOpponentUnitInfoWindow (true);

			// Update Opponent Unit Info
			selectedUnitClass.GetStats ().UpdateOpponentUnitInfo (this.Stats);

			Nodes unitCurrNode = selectedUnitClass.GetCurrNode ();

			for(int i = 1; i <= selectedUnitClass.GetStats ()._weapon.Range; ++i)
			{
				if ((selectedUnitClass.GetCurrNode ().GetXIndex () + i == currNode.GetXIndex ()
					&& selectedUnitClass.GetCurrNode ().GetZIndex () == currNode.GetZIndex ()) ||
					(selectedUnitClass.GetCurrNode ().GetXIndex () - i == currNode.GetXIndex ()
						&& selectedUnitClass.GetCurrNode ().GetZIndex () == currNode.GetZIndex ()) ||
					(selectedUnitClass.GetCurrNode ().GetZIndex () + i == currNode.GetZIndex ()
						&& selectedUnitClass.GetCurrNode ().GetXIndex () == currNode.GetXIndex ()) ||
					(selectedUnitClass.GetCurrNode ().GetZIndex () - i == currNode.GetZIndex ()
						&& selectedUnitClass.GetCurrNode ().GetXIndex () == currNode.GetXIndex ())) 
				{
					// Change Visibility of Node to opague
					rend.material.color = HoverColor;
					break;
				}
			}
		}
		else
		{
			// Spawn Unit Info Window
			Stats.SetUnitInfoWindow(true);
		}
			
	}

	// Run only when Mouse cursor move out of the node collision box
	void OnMouseExit()
	{
		if (PlayerManager.Instance.GetAbleToAttack ())
		{
			// Get information from Units class
			Players selectedUnitClass = PlayerManager.Instance.GetSelectedUnit ().GetComponent<Players> ();

			// De-Spawn Opponent Unit Info Window
			selectedUnitClass.GetStats ().SetOpponentUnitInfoWindow (false);
		}
		else
		{
			// De-Spawn Unit Info Window
			Stats.SetUnitInfoWindow(false);
		}


		rend.material.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.GetComponent<UnitVariables> ().Copy (Stats);
		this.gameObject.GetComponent<UnitVariables> ().UpdateHealthBar ();

		// Update Unit Info Window only if its Active
		if (!PlayerManager.Instance.GetAbleToAttack ())
			this.gameObject.GetComponent<UnitVariables> ().UpdateUnitInfo ();

		// Checks if it is this particular's AI turn
		if (turnManager.GetCurrUnit () == null || turnManager.GetCurrUnit ().GetID () != this.gameObject.GetComponent<AI> ().GetID ()) {
			return;
		}


		if (Stats.HP == 0) {
			Destroy (this.gameObject);
		}

		// Acts only until the AP has reached 0
		if (Stats.AP > 0) {
			// print (this.gameObject.name + " " + Stats.AP);

			if ((this.transform.position - TargetMovement).magnitude < 0.1f) {

				// Spawn Unit Info Window
				Stats.SetUnitInfoWindow(true);
				currNode.SetSelectable (false);
				currNode.ChangeColour ();
				currNode.SetOccupiedNULL ();
				switch (Personality) {
				case(EnemyStrategy.AGGRESSIVE):
					AggressiveAction ();
					break;
				case(EnemyStrategy.DEFENSIVE):
					DefensiveAction ();
					break;
				case(EnemyStrategy.RANDOM):
					RandomAction ();
					break;
				case(EnemyStrategy.STRATEGIC):
					StrategicAction ();
					break;
				}

				TargetMovement = currNode.transform.position;
			}
		} else {
			if ((this.transform.position - TargetMovement).magnitude < 0.1f) {
				this.transform.position = TargetMovement;
				
				// De-Spawn Unit Info Window
				Stats.SetUnitInfoWindow(false);
				currNode.SetSelectable (false);
				currNode.ChangeColour ();
				currNode.SetOccupied (this.gameObject);
				TurnEnd ();
			}
		}

		this.transform.position += (TargetMovement - this.transform.position).normalized * 0.05f;
	}

	void AggressiveAction()
	{
		if (EnemyTarget == null) {

			Nodes Temp = null;

			for (int x = 0; x < GridRef.GetRows (); ++x) {
				for (int z = 0; z < GridRef.GetColumn (); ++z) {
					if (GridRef.GetNode (x, z).GetOccupied () != null && GridRef.GetNode (x, z).GetOccupied ().tag == "PlayerUnit") {
						if (Temp != null) { // Temp Magnitude is more than current grid magnitude, change Temp altogether
							if ((Temp.GetOccupied ().transform.position - currNode.transform.position).magnitude > (GridRef.GetNode (x, z).GetOccupied ().transform.position - currNode.transform.position).magnitude) {
								Temp = GridRef.GetNode (x, z);
							}
						} else { // Sets the first reference of Temp
							Temp = GridRef.GetNode (x, z);
						}
					}
				}
			}

			EnemyTarget = Temp;
		} else {
			if (!Path_Set) {
				SetPath (currNode, EnemyTarget);
				m_path.Dequeue ();
				Path_Set = true;
			} else {
				if (!isAttacking) {
					// Assigns a node while also removing the assigned node from the path
					Nodes TempMove = currNode;
					if (m_path.Count != 0) {
						TempMove = m_path.Dequeue ();
					}

					// If the next area is or isn't the enemy target, act accordingly
					if (TempMove != EnemyTarget) {
						currNode.SetOccupiedNULL ();
						currNode = TempMove;
						currNode.SetOccupied (this.gameObject);
					} else {
						isAttacking = true;
						TempMove.SetSelectable (false);
						int damageDeal = turnManager.CalculateDamage (this.gameObject, EnemyTarget.GetOccupied());
						EnemyTarget.GetOccupied ().GetComponent<UnitVariables> ().HP -= damageDeal;
						if (EnemyTarget.GetOccupied ().GetComponent<UnitVariables> ().HP <= 0)
							EnemyTarget = null;
					}
				} else {
					// prevent error - if player died
					if (EnemyTarget.GetOccupied () == null) {
						EnemyTarget = null;
						return;
					}
					
					UnitVariables Temp = EnemyTarget.GetOccupied ().GetComponent<Players> ().GetStats ();
					int damageDeal = turnManager.CalculateDamage (this.gameObject, EnemyTarget.GetOccupied());
					Temp.HP -= damageDeal;
					EnemyTarget.GetOccupied ().GetComponent<Players> ().SetStats (Temp);
					EnemyTarget.GetOccupied ().GetComponent<UnitVariables> ().UpdateHealthBar ();
					EnemyTarget.GetOccupied ().GetComponent<UnitVariables> ().UpdateUnitInfo ();
					if (EnemyTarget.GetOccupied ().GetComponent<UnitVariables> ().HP <= 0)
						EnemyTarget = null;
				}
				this.Stats.AP--;
			}
		}
	}

	void DefensiveAction()
	{
		
	}

	void RandomAction()
	{
		
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
		Camera.main.GetComponent<CameraControl> ().setFocus (this.gameObject);
		Stats.AP = Stats.startAP;
	}

	// The reset for the end of each Unit's turn
	public void TurnEnd()
	{
		EnemyTarget = null;
		if (m_path.Count > 0)
			m_path.Clear ();
		Path_Set = false;
		isAttacking = false;
		// If nextNode is not null, update currNode to be nextNode
		// And null nextNode
		if (nextNode)
		{
			currNode = nextNode;
			nextNode = null;
		}
		turnManager.NextTurn ();

		Stats.AP = MaxAP;
	}

	public void SetPath(Nodes m_start, Nodes m_end)
	{
		m_path.Enqueue (m_start);
		Nodes Temp = m_start;
		int AP_Ref = Stats.AP;

		while (!m_path.Contains (m_end) && AP_Ref > 0) {
			float UpMag = (Temp.transform.position - m_end.transform.position).magnitude,
			DownMag = (Temp.transform.position - m_end.transform.position).magnitude,
			LeftMag = (Temp.transform.position - m_end.transform.position).magnitude,
			RightMag = (Temp.transform.position - m_end.transform.position).magnitude,
			Closest;

			if (Temp.GetZIndex () != GridRef.GetColumn () - 1) {
				UpMag = (GridRef.GetNode(Temp.GetXIndex(), Temp.GetZIndex() + 1).transform.position - m_end.transform.position).magnitude;
			}
			if (Temp.GetZIndex () != 0) {
				DownMag = (GridRef.GetNode(Temp.GetXIndex(), Temp.GetZIndex() - 1).transform.position - m_end.transform.position).magnitude;
			}
			if (Temp.GetXIndex () != GridRef.GetRows () - 1) {
				LeftMag = (GridRef.GetNode(Temp.GetXIndex() + 1, Temp.GetZIndex()).transform.position - m_end.transform.position).magnitude;
			}
			if (Temp.GetXIndex () != 0) {
				RightMag = (GridRef.GetNode(Temp.GetXIndex() - 1, Temp.GetZIndex()).transform.position - m_end.transform.position).magnitude;
			}
			Closest = Mathf.Min (UpMag, DownMag, LeftMag, RightMag);

			if (Closest == UpMag) {
				Temp = GridRef.GetNode (Temp.GetXIndex (), Temp.GetZIndex () + 1);
			} else if (Closest == DownMag) {
				Temp = GridRef.GetNode (Temp.GetXIndex (), Temp.GetZIndex () - 1);
			} else if (Closest == LeftMag) {
				Temp = GridRef.GetNode (Temp.GetXIndex () + 1, Temp.GetZIndex ());
			} else if (Closest == RightMag) {
				Temp = GridRef.GetNode (Temp.GetXIndex () - 1, Temp.GetZIndex ());
			}

			if (Temp != EnemyTarget) {
				Temp.SetSelectable (true);
			}
			Temp.ChangeColour ();
			m_path.Enqueue (Temp);
			AP_Ref--;
		}
	}

	// Get & Set Unit's ID
	public int GetID() {return ID;}
	public void SetID(int _id) {ID = _id;}

	// Get Unit's Stats
	public UnitVariables GetStats() {return Stats;}
}
