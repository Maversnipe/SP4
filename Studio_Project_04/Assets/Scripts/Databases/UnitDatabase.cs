using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

[System.Serializable]
public class UnitDatabase : GenericSingleton<UnitDatabase>
{

	public static List<UnitVariables> unitDatabase = new List<UnitVariables>();

	private JsonData unitData;

	// Use this for initialization
	void Start () {
		unitData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/UnitVariables.json"));

		ConstructUnitDatabase();

	}

//    // Use this for initialization
//    void Start()
//    {
//
//        // Slashing weapon + Medium Armor
//        // HP normal, AP and Initiative fast
//
//        // Melee
//		Database.Add(new UnitVariables("Trainee", 20, 7, 9, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//		Database.Add(new UnitVariables("Mercenary", 30, 9, 11, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//		Database.Add(new UnitVariables("Myrmidon", 40, 11, 13, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//		Database.Add(new UnitVariables("SwordMaster", 50, 13, 15, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//
//        // Ranged
//        Database.Add(new Unit("Fighter", 20, 7, 9, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//        Database.Add(new Unit("Warrior", 30, 9, 11, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//
//        // Blunt weapon + Heavy Armor
//        // HP high, AP and Initiative slow
//
//        // Melee
//        Database.Add(new Unit("Brigand", 30, 3, 5, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//        Database.Add(new Unit("Pirate", 40, 5, 7, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//        Database.Add(new Unit("Corsair", 50, 7, 9, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//        Database.Add(new Unit("Berserker", 60, 9, 11, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//
//        // Ranged
//        Database.Add(new Unit("Brigand Trainee", 30, 3, 5, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//        Database.Add(new Unit("Joker", 40, 5, 7, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//
//        // Piercing weapon + Light Armor
//        // HP low, AP and Initiative normal
//
//        // Melee
//        Database.Add(new Unit("Novice", 10, 5, 7, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//        Database.Add(new Unit("Squire", 20, 7, 9, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//        Database.Add(new Unit("Adventurer", 30, 9, 11, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//        Database.Add(new Unit("Hero", 40, 11, 13, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//
//        // Ranged
//        Database.Add(new Unit("Archer", 10, 5, 7, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//        Database.Add(new Unit("Sniper", 20, 7, 9, WeaponDatabase.Instance.FetchWeaponByName("The Hax Sword")));
//    }

    // Update is called once per frame

	public UnitVariables FetchUnitByName(string _name)
	{
		for(int i = 0; i < unitDatabase.Count; i++)
		{
			if (unitDatabase[i].Name == _name)
			{
				return unitDatabase[i];
			}
		}

		return null;
	}

	void ConstructUnitDatabase()
	{
		for(int i = 0; i < unitData.Count; i++)
		{
			UnitVariables newUnit = new UnitVariables(unitData[i]["name"].ToString(), (int)unitData[i]["hp"], (int)unitData[i]["ap"],
				(int)unitData[i]["initiative"], (int)unitData[i]["id"], unitData[i]["weapon"].ToString(),
				unitData[i]["armor"].ToString());

			unitDatabase.Add(newUnit);
		}
	}
}
