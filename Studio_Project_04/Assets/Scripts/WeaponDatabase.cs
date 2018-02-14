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
	public Weapon (string n_name, int n_Damage, DamageTypes n_Type, WeaponType n_Range)
	{
		Name = n_name;
		Damage = n_Damage;
		Type = n_Type;
		Range = n_Range;
	}
	string Name;
	DamageTypes Type;
	WeaponType Range;
	int Damage;

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
		Database.Add (new Weapon ("Wooden Sword", 2, DamageTypes.SLASH, WeaponType.MELEE));
		Database.Add (new Weapon ("Iron Sword", 3, DamageTypes.SLASH, WeaponType.MELEE));
		Database.Add (new Weapon ("Steel Sword", 4, DamageTypes.SLASH, WeaponType.MELEE));
		Database.Add (new Weapon ("Diamond Sword", 5, DamageTypes.SLASH, WeaponType.MELEE));

		// Pierce Damage Type
		Database.Add (new Weapon ("Rapier", 6, DamageTypes.PIERCE, WeaponType.MELEE));

		// Blunt Damage Type
		Database.Add (new Weapon ("Hammer", 7, DamageTypes.BLUNT, WeaponType.MELEE));

		// -----  Ranged Weapon Type ----- 


		for (int i = 0; i < Database.Count; i++) {
			StringData.Add (Database [i].getName ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
