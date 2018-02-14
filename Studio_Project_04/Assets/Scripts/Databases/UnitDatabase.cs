using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit{
	public Unit (string n_Name, int n_HP, int n_AP, int n_Initiative, Weapon n_weapon, Armor n_armor)
	{
		Name = n_Name;
		HP = n_HP;
		AP = n_AP;
		Initiative = n_Initiative;
		_weapon = n_weapon;
		_armor = n_armor;
	}

	string Name;
	int HP;
	int AP;
	int Initiative;
	Weapon _weapon;
	Armor _armor;

	public string getName()
	{
		return Name;
	}

	public int getHP()
	{
		return HP;
	}

	public int getAP()
	{
		return AP;
	}

	public int getInitiative()
	{
		return Initiative;
	}

	public Weapon getWeapon()
	{
		return _weapon;
	}

	Armor getArmor()
	{
		return _armor;
	}
}

public class UnitDatabase : GenericSingleton<UnitDatabase> {

	public static List<Unit> Database = new List<Unit> ();

	// Use this for initialization
	void Start () {
		Database.Add(new Unit("Squire",5,3,3,WeaponDatabase.getWeapon("Wooden Sword"),ArmorDatabase.getArmor("Nothing")));
	}
	
	// Update is called once per frame
	void Update () {
	}
}
