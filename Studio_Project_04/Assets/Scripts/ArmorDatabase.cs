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
		Database.Add (new Armor ("Leather", 2, ArmorTypes.LIGHT));
		Database.Add (new Armor ("Chainmail", 5, ArmorTypes.MEDIUM));
		Database.Add (new Armor ("Plate", 9, ArmorTypes.HEAVY));
		Database.Add (new Armor ("Really shitty stuff", 20, ArmorTypes.LIGHT));

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
