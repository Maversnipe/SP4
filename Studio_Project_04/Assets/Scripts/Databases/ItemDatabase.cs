using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

using System.IO;

[System.Serializable]
public class ItemDatabase : GenericSingleton<ItemDatabase>
{
    [SerializeField]
    public static List<Item> itemDatabase = new List<Item>();
    private JsonData itemData;

	// Use this for initialization
	void Start () {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        Debug.Log("1");
        ConstructItemDatabase();

	}

    public Item FetchItemByName(string name)
    {
        for (int i = 0; i < itemDatabase.Count; i++)
        {
            if (itemDatabase[i].Title == name)
            {
                return itemDatabase[i];
            }

        }

        return null;
    }

    public Item FetchItemByID(int id)
    {
        for(int i = 0; i < itemDatabase.Count; i++)
        {
            if (itemDatabase[i].ID == id)
            {
                return itemDatabase[i];
            }
            
        }

        return null;
    }

    void ConstructItemDatabase()
    {
        for(int i = 0; i < itemData.Count; i++)
        {
            itemDatabase.Add(new Item((int)itemData[i]["id"], itemData[i]["title"].ToString(), (int)itemData[i]["value"], itemData[i]["type"].ToString(), itemData[i]["modifier"].ToString(), (int)itemData[i]["modifier_value"]
                , itemData[i]["description"].ToString(), (bool)itemData[i]["stackable"], itemData[i]["rarity"].ToString(), itemData[i]["icon"].ToString()));
        }
    }
}

public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public string Type { get; set; }
    public string Modifier { get; set; }
    public int ModifierValue { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public string Rarity { get; set; }
    public string Icon { get; set; }
    public Sprite Sprite { get; set; }

    public Item(int id, string title, int value, string type, string modifier, int modifierValue, string description, bool stackable, string rarity, string icon)
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
        this.Type = type;
        this.Modifier = modifier;
        this.ModifierValue = modifierValue;
        this.Description = description;
        this.Stackable = stackable;
        this.Rarity = rarity;
        this.Icon = icon;
        this.Sprite = Resources.Load<Sprite>("Sprite/Items/" + icon);
    }

    public Item()
    {
        this.ID = -1;
    }
}