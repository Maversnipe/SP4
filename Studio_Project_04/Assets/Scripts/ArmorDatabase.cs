using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorTypes{
	LIGHT,
	MEDIUM,
	HEAVY
};

public class Armor{
	public Armor(string n_Name, int n_Defense, ArmorTypes n_Type)
	{
		Name = n_Name;
		DefenseValue = n_Defense;
		Type = n_Type;
	}
	string Name;
	int DefenseValue;
	ArmorTypes Type;

	public string getName()
	{
		return Name;
	}

	public int getDefense()
	{
		return DefenseValue;
	}

	public ArmorTypes getType()
	{
		return Type;
	}
}

// Has to be singleton to prevent more than one database from existing
[System.Serializable]
public class ArmorDatabase : GenericSingleton<ArmorDatabase> {

	[SerializeField]
	public static List<Armor> Database = new List<Armor>();
	[SerializeField]
	public static List<string> StringData = new List<string>();

	// Use this for initialization
	void Start () {
		
		// Light Armor Type
		Database.Add (new Armor ("Nothing", 0, ArmorTypes.LIGHT));
		Database.Add (new Armor ("Cloth", 1, ArmorTypes.LIGHT));
		Database.Add (new Armor ("Upgraded Cloth", 2, ArmorTypes.LIGHT));
		Database.Add (new Armor ("Leather", 3, ArmorTypes.LIGHT));
		Database.Add (new Armor ("Upgraded Leather", 4, ArmorTypes.LIGHT));

		// Medium Armor Type
		Database.Add (new Armor ("Wooden", 5, ArmorTypes.MEDIUM));
		Database.Add (new Armor ("Upgraded Wooden", 6, ArmorTypes.MEDIUM));
		Database.Add (new Armor ("Iron", 7, ArmorTypes.MEDIUM));
		Database.Add (new Armor ("Upgraded Iron", 8, ArmorTypes.MEDIUM));

		// Heavy Armor Type
		Database.Add (new Armor ("Steel", 11, ArmorTypes.HEAVY));
		Database.Add (new Armor ("Upgraded Steel", 12, ArmorTypes.HEAVY));
		Database.Add (new Armor ("Silver", 13, ArmorTypes.HEAVY));
		Database.Add (new Armor ("Upgraded Silver", 14, ArmorTypes.HEAVY));

		for (int i = 0; i < Database.Count; i++) {
			StringData.Add (Database [i].getName ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<Armor> getList () {
		return Database;
	}
}
