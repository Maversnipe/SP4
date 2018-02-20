using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit{
    public Unit(string n_Name, int n_HP, int n_AP, int n_Initiative, Weapon n_weapon)
    {
        Name = n_Name;
        HP = n_HP;
        AP = n_AP;
        Initiative = n_Initiative;
        _weapon = n_weapon;
    }

    string Name;
    int HP;
    int AP;
    int Initiative;
    Weapon _weapon;

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
}

public class UnitDatabase : GenericSingleton<UnitDatabase>
{

    public static List<Unit> Database = new List<Unit>();



    // Use this for initialization
    void Start()
    {

        // Slashing weapon + Medium Armor
        // HP normal, AP and Initiative fast

        // Melee
        Database.Add(new Unit("Trainee", 20, 7, 9, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("Mercenary", 30, 9, 11, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("Myrmidon", 40, 11, 13, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("SwordMaster", 50, 13, 15, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));

        // Ranged
        Database.Add(new Unit("Fighter", 20, 7, 9, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("Warrior", 30, 9, 11, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));

        // Blunt weapon + Heavy Armor
        // HP high, AP and Initiative slow

        // Melee
        Database.Add(new Unit("Brigand", 30, 3, 5, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("Pirate", 40, 5, 7, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("Corsair", 50, 7, 9, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("Berserker", 60, 9, 11, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));

        // Ranged
        Database.Add(new Unit("Brigand Trainee", 30, 3, 5, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("Joker", 40, 5, 7, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));

        // Piercing weapon + Light Armor
        // HP low, AP and Initiative normal

        // Melee
        Database.Add(new Unit("Novice", 10, 5, 7, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("Squire", 20, 7, 9, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("Adventurer", 30, 9, 11, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("Hero", 40, 11, 13, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));

        // Ranged
        Database.Add(new Unit("Archer", 10, 5, 7, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
        Database.Add(new Unit("Sniper", 20, 7, 9, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
