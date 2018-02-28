using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;
using System.IO;

[System.Serializable]
public class ArmorDatabase : GenericSingleton<ArmorDatabase>
{
    [SerializeField]
    public static List<Armor> armorDatabase = new List<Armor>();
    private JsonData armorData;

    IEnumerator loadStreamingAsset(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        string result;
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            result = www.text;
            armorData = JsonMapper.ToObject(File.ReadAllText(result));
        }
        else
        {
            result = System.IO.File.ReadAllText(filePath);
            armorData = JsonMapper.ToObject(result);
        }
    }

    // Use this for initialization
    void Start () {
        //armorData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Armors.json"));
        StartCoroutine(loadStreamingAsset("Armors.json"));
        ConstructArmorDatabase();
	}

    public Armor FetchArmorByName(string name)
    {
        for (int i = 0; i < armorDatabase.Count; i++)
        {
            if (armorDatabase[i].Title == name)
            {
                return armorDatabase[i];
            }

        }

        return null;
    }

    public Armor FetchArmorByID(int id)
    {
        for(int i = 0; i < armorDatabase.Count; i++)
        {
            if (armorDatabase[i].ID == id)
            {
                return armorDatabase[i];
            }
            
        }

        return null;
    }

    void ConstructArmorDatabase()
    {
        for(int i = 0; i < armorData.Count; i++)
        {
			Armor newArmor = new Armor((int)armorData[i]["id"], armorData[i]["title"].ToString(), (int)armorData[i]["value"],
				armorData[i]["type"].ToString(), (int)armorData[i]["stats"]["defence"], (int)armorData[i]["stats"]["strength"],
				(int)armorData[i]["stats"]["vitality"], (int)armorData[i]["stats"]["intelligence"], (int)armorData[i]["stats"]["dexterity"],
				armorData[i]["description"].ToString(), (bool)armorData[i]["stackable"], armorData[i]["rarity"].ToString(),
				armorData[i]["icon"].ToString());
			
			armorDatabase.Add(newArmor);
        }
    }
}

public class Armor
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public string Type { get; set; }
    public int Defence { get; set; }
    public int Strength { get; set; }
    public int Vitality { get; set; }
    public int Intelligence { get; set; }
    public int Dexterity { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public string Rarity { get; set; }
    public string Icon { get; set; }
    public Sprite Sprite { get; set; }

    public Armor(int id, string title, int value,
		string type, int defence, int strength,
		int vitality, int intelligence, int dexterity,
		string description, bool stackable, string rarity,
		string icon)
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
        this.Type = type;
        this.Defence = defence;
        this.Strength = strength;
        this.Vitality = vitality;
        this.Intelligence = intelligence;
        this.Dexterity = dexterity;
        this.Description = description;
        this.Stackable = stackable;
        this.Rarity = rarity;
        this.Icon = icon;
        this.Sprite = Resources.Load<Sprite>("Sprite/Items/Armors/" + icon);
    }

    public Armor()
    {
        this.ID = -1;
    }
}