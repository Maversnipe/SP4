using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTypes{
	SLASH,
	PIERCE,
	BLUNT
};

public enum WeaponType{
	RANGED,
	MELEE
}

public class Weapon {
	public Weapon (string n_name, int n_Damage, DamageTypes n_Type, WeaponType n_Range, int n_AttackRange)
	{
		Name = n_name;
		Damage = n_Damage;
		Type = n_Type;
		Range = n_Range;
		AttackRange = n_AttackRange;
	}
	string Name;
	DamageTypes Type;
	WeaponType Range;
	int Damage;
	int AttackRange;

	public string getName() {
		return Name;
	}

	public DamageTypes getDamageType() {
		return Type;
	}
	public WeaponType getRange() {
		return Range;
	}
	public int getDamage() {
		return Damage;
	}
	public int getAttackRange() {
		return AttackRange;
	}
};

// Has to be singleton to prevent more than one database from existing
public class WeaponDatabase : GenericSingleton<WeaponDatabase> {

	[SerializeField]
	public static List<Weapon> Database = new List<Weapon>();

	[SerializeField]
	public static List<string> StringData = new List<string>();

	// Use this for initialization
	void Start () {

		// ----- Melee Weapon Type ----- 

		// Slash Damage Type
		Database.Add (new Weapon ("Wooden Sword", 5, DamageTypes.SLASH, WeaponType.MELEE, 1));
		Database.Add (new Weapon ("Iron Sword", 6, DamageTypes.SLASH, WeaponType.MELEE, 1));
		Database.Add (new Weapon ("Steel Sword", 7, DamageTypes.SLASH, WeaponType.MELEE, 1));
		Database.Add (new Weapon ("Silver Sword", 8, DamageTypes.SLASH, WeaponType.MELEE, 1));

		// Pierce Damage Type
		Database.Add (new Weapon ("Lance", 9, DamageTypes.PIERCE, WeaponType.MELEE, 1));
		Database.Add (new Weapon ("Iron Lance", 10, DamageTypes.PIERCE, WeaponType.MELEE, 1));
		Database.Add (new Weapon ("Steel Lance", 11, DamageTypes.PIERCE, WeaponType.MELEE, 1));
		Database.Add (new Weapon ("Silver Lance", 12, DamageTypes.PIERCE, WeaponType.MELEE, 1));

		// Blunt Damage Type
		Database.Add (new Weapon ("Wooden Hammer", 13, DamageTypes.BLUNT, WeaponType.MELEE, 1));
		Database.Add (new Weapon ("Iron Hammer", 14, DamageTypes.BLUNT, WeaponType.MELEE, 1));
		Database.Add (new Weapon ("Steel Hammer", 15, DamageTypes.BLUNT, WeaponType.MELEE, 1));
		Database.Add (new Weapon ("Silver Hammer", 16, DamageTypes.BLUNT, WeaponType.MELEE, 1));

		// -----  Ranged Weapon Type ----- 

		// Slash Damage Type
		Database.Add (new Weapon ("Hachet", 1, DamageTypes.SLASH, WeaponType.RANGED, 2));
		Database.Add (new Weapon ("Hand Axe", 2, DamageTypes.SLASH, WeaponType.RANGED, 2));

		// Pierce Damage Type
		Database.Add (new Weapon ("Bow", 4, DamageTypes.PIERCE, WeaponType.RANGED, 4));
		Database.Add (new Weapon ("Cross Bow", 5, DamageTypes.PIERCE, WeaponType.RANGED, 4));

		// Blunt Damage Type
		Database.Add (new Weapon ("Sling Shot", 7, DamageTypes.BLUNT, WeaponType.RANGED, 2));
		Database.Add (new Weapon ("Boomerang", 8, DamageTypes.BLUNT, WeaponType.RANGED, 2)); // Have to move back to player


		for (int i = 0; i < Database.Count; i++) {
			StringData.Add (Database [i].getName ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
