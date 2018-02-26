using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

using System.IO;

[System.Serializable]
public class WeaponDatabase : GenericSingleton<WeaponDatabase>
{
    [SerializeField]
    public static List<Weapon> weaponDatabase = new List<Weapon>();
    private JsonData weaponData;

	// Use this for initialization
	void Start () {
        weaponData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Weapons.json"));

        ConstructWeaponDatabase();

	}

    public Weapon FetchWeaponByName(string name)
    {
        for (int i = 0; i < weaponDatabase.Count; i++)
        {
            if (weaponDatabase[i].Title == name)
            {
                return weaponDatabase[i];
            }

        }

        return null;
    }

    public Weapon FetchWeaponByID(int id)
    {
        for(int i = 0; i < weaponDatabase.Count; i++)
        {
            if (weaponDatabase[i].ID == id)
            {
                return weaponDatabase[i];
            }
            
        }

        return null;
    }

    void ConstructWeaponDatabase()
    {
        for(int i = 0; i < weaponData.Count; i++)
        {
			Weapon newWeapon = new Weapon ((int)weaponData[i]["id"], weaponData[i]["title"].ToString(), (int)weaponData[i]["value"],
				weaponData[i]["type"].ToString(), (int)weaponData[i]["stats"]["attack"], (int)weaponData[i]["stats"]["ap"], (int)weaponData[i]["stats"]["range"],
				(int)weaponData[i]["stats"]["strength"], (int)weaponData[i]["stats"]["vitality"], (int)weaponData[i]["stats"]["intelligence"],
				(int)weaponData[i]["stats"]["dexterity"], weaponData[i]["description"].ToString(), (bool)weaponData[i]["stackable"],
				weaponData[i]["rarity"].ToString(), weaponData[i]["icon"].ToString());

			weaponDatabase.Add(newWeapon);
        }
    }
}

public class Weapon
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public string Type { get; set; }
    public int Attack { get; set; }
	public int AP { get; set; }
	public int Range { get; set; }
    public int Strength { get; set; }
    public int Vitality { get; set; }
    public int Intelligence { get; set; }
    public int Dexterity { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public string Rarity { get; set; }
    public string Icon { get; set; }
    public Sprite Sprite { get; set; }

	public Weapon(int id, string title, int value,
		string type, int attack, int ap, int range,
		int strength, int vitality, int intelligence,
		int dexterity, string description, bool stackable,
		string rarity, string icon)
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
        this.Type = type;
        this.Attack = attack;
		this.AP = ap;
		this.Range = range;
        this.Strength = strength;
        this.Vitality = vitality;
        this.Intelligence = intelligence;
        this.Dexterity = dexterity;
        this.Description = description;
        this.Stackable = stackable;
        this.Rarity = rarity;
        this.Icon = icon;
        this.Sprite = Resources.Load<Sprite>("Sprite/Items/" + icon);
    }

    public Weapon()
    {
        this.ID = -1;
    }
}