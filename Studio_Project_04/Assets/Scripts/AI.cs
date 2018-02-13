using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStrategy{
	AGGRESSIVE,
	RANDOM,
	DEFENSIVE
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

	// Use this for initialization
	void Start () {
		TargetMovement = new Vector3(10,0,10);
	}
	
	// Update is called once per frame
	void Update () {
		if ((this.transform.position - TargetMovement).magnitude < 1) {
			// Determines which is the next area to go to
			switch (Personality) {
			case EnemyStrategy.AGGRESSIVE:
				break;
			case EnemyStrategy.DEFENSIVE:
				break;
			case EnemyStrategy.RANDOM:
				break;
			}
		} else {
			this.transform.position += (TargetMovement - this.transform.position).normalized * 0.01f;
		}
	}

	Faction getFactionType (){
		return FactionType;
	}
}
