using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;
using System.IO;
using System;

[System.Serializable]
public class ItemDatabase : GenericSingleton<ItemDatabase>
{
    [SerializeField]
    public static Item[] itemDatabase;
    private JsonData itemData;

    IEnumerator loadStreamingAsset(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        string result;
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            result = www.text;
            Debug.Log(result);
            itemDatabase = JsonHelper.FromJson<Item>(result);
        }
        else
        {
            result = System.IO.File.ReadAllText(filePath);
            Debug.Log(result);
            itemDatabase = JsonHelper.FromJson<Item>(result);
        }
    }

    // Use this for initialization
    void Start () {
        //itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        StartCoroutine(loadStreamingAsset("Items.json"));
        ConstructItemDatabase();

	}

    public Item FetchItemByName(string name)
    {
        for (int i = 0; i < itemDatabase.Length; i++)
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
        for(int i = 0; i < itemDatabase.Length; i++)
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
        for (int i = 0; i < itemDatabase.Length; i++)
        {
            itemDatabase[i].ID = itemDatabase[i].id;
            itemDatabase[i].Title = itemDatabase[i].title;
            itemDatabase[i].Value = itemDatabase[i].value;
            itemDatabase[i].Type = itemDatabase[i].type;
            itemDatabase[i].Modifier = itemDatabase[i].modifier;
            itemDatabase[i].ModifierValue = itemDatabase[i].modifier_value;
            itemDatabase[i].Description = itemDatabase[i].description;
            itemDatabase[i].Stackable = itemDatabase[i].stackable;
            itemDatabase[i].Rarity = itemDatabase[i].rarity;
            itemDatabase[i].Icon = itemDatabase[i].icon;
            itemDatabase[i].Sprite = Resources.Load<Sprite>("Sprite/Items/" + itemDatabase[i].Icon);
        }
    }
}

[Serializable]
public class Item
{
    public int id;
    public string title;
    public int value;
    public string type;
    public string modifier;
    public int modifier_value;
    public string description;
    public bool stackable;
    public string rarity;
    public string icon;

    public int ID;
    public string Title;
    public int Value;
    public string Type;
    public string Modifier;
    public int ModifierValue;
    public string Description;
    public bool Stackable;
    public string Rarity;
    public string Icon;
    public Sprite Sprite;

    public Item()
    {
       
        //this.Sprite = Resources.Load<Sprite>("Sprite/Items/" + icon);
    }

    //public Item()
    //{
    //    this.ID = -1;
    //}
}