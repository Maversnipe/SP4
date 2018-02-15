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

		// Slashing weapon + Medium Armor
		// HP normal, AP and Initiative fast

		// Melee
		Database.Add(new Unit("Trainee",20,7,9,WeaponDatabase.getWeapon("Wooden Sword"),ArmorDatabase.getArmor("Wooden")));
		Database.Add(new Unit("Mercenary",30,9,11,WeaponDatabase.getWeapon("Iron Sword"),ArmorDatabase.getArmor("Upgraded Wooden")));
		Database.Add(new Unit("Myrmidon",40,11,13,WeaponDatabase.getWeapon("Steel Sword"),ArmorDatabase.getArmor("Iron")));
		Database.Add(new Unit("SwordMaster",50,13,15,WeaponDatabase.getWeapon("Silver Sword"),ArmorDatabase.getArmor("Upgraded Iron")));

		// Ranged
		Database.Add(new Unit("Fighter",20,7,9,WeaponDatabase.getWeapon("Hachet"),ArmorDatabase.getArmor("Wooden")));
		Database.Add(new Unit("Warrior",30,9,11,WeaponDatabase.getWeapon("Hand Axe"),ArmorDatabase.getArmor("Iron")));

		// Blunt weapon + Heavy Armor
		// HP high, AP and Initiative slow

		// Melee
		Database.Add(new Unit("Brigand",30,3,5,WeaponDatabase.getWeapon("Wooden Hammer"),ArmorDatabase.getArmor("Steel")));
		Database.Add(new Unit("Pirate",40,5,7,WeaponDatabase.getWeapon("Iron Hammer"),ArmorDatabase.getArmor("Upgraded Steel")));
		Database.Add(new Unit("Corsair",50,7,9,WeaponDatabase.getWeapon("Steel Hammer"),ArmorDatabase.getArmor("Silver")));
		Database.Add(new Unit("Berserker",60,9,11,WeaponDatabase.getWeapon("Silver Hammer"),ArmorDatabase.getArmor("Upgraded Silver")));

		// Ranged
		Database.Add(new Unit("Brigand Trainee",30,3,5,WeaponDatabase.getWeapon("Sling Shot"),ArmorDatabase.getArmor("Steel")));
		Database.Add(new Unit("Joker",40,5,7,WeaponDatabase.getWeapon("Boomerang"),ArmorDatabase.getArmor("Silver")));

		// Piercing weapon + Light Armor
		// HP low, AP and Initiative normal

		// Melee
		Database.Add(new Unit("Novice",10,5,7,WeaponDatabase.getWeapon("Wooden Lance"),ArmorDatabase.getArmor("Cloth")));
		Database.Add(new Unit("Squire",20,7,9,WeaponDatabase.getWeapon("Iron Lance"),ArmorDatabase.getArmor("Upgraded Cloth")));
		Database.Add(new Unit("Adventurer",30,9,11,WeaponDatabase.getWeapon("Steel Lance"),ArmorDatabase.getArmor("Leather")));
		Database.Add(new Unit("Hero",40,11,13,WeaponDatabase.getWeapon("Silver Lance"),ArmorDatabase.getArmor("Upgraded Leather")));

		// Ranged
		Database.Add(new Unit("Archer",10,5,7,WeaponDatabase.getWeapon("Sling Shot"),ArmorDatabase.getArmor("Nothing")));
		Database.Add(new Unit("Sniper",20,7,9,WeaponDatabase.getWeapon("Boomerang"),ArmorDatabase.getArmor("Nothing")));
	}
	
	// Update is called once per frame
	void Update () {
	}
}
