using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;
using System.IO;
using System;

[System.Serializable]
public class ArmorDatabase : GenericSingleton<ArmorDatabase>
{
    [SerializeField]
    public static Armor[] armorDatabase;
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
            Debug.Log(result);
            armorDatabase = JsonHelper.FromJson<Armor>(result);
        }
        else
        {
            result = System.IO.File.ReadAllText(filePath);
            Debug.Log(result);
            armorDatabase = JsonHelper.FromJson<Armor>(result);
        }
    }

    // Use this for initialization
    void Start () {
        //armorData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Armors.json"));
        StartCoroutine(loadStreamingAsset("Armors.json"));
        ConstructArmorDatabase();
        Debug.Log(armorDatabase[5].Title);
    }

    public Armor FetchArmorByName(string name)
    {
        for (int i = 0; i < armorDatabase.Length; i++)
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
        for(int i = 0; i < armorDatabase.Length; i++)
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
        for(int i = 0; i < armorDatabase.Length; i++)
        {
            armorDatabase[i].ID = armorDatabase[i].id;
            armorDatabase[i].Title = armorDatabase[i].title;
            armorDatabase[i].Value = armorDatabase[i].value;
            armorDatabase[i].Type = armorDatabase[i].type;
            armorDatabase[i].Defence = armorDatabase[i].defence;
            armorDatabase[i].Description = armorDatabase[i].description;
            armorDatabase[i].Stackable = armorDatabase[i].stackable;
            armorDatabase[i].Rarity = armorDatabase[i].rarity;
            armorDatabase[i].Icon = armorDatabase[i].icon;
            armorDatabase[i].Sprite = Resources.Load<Sprite>("Sprite/Items/Armors/" + armorDatabase[i].Icon);
        }
    }
}

[Serializable]
public class Armor
{
    public int id;
    public string title;
    public int value;
    public string type;
    public int defence;
    public string description;
    public bool stackable;
    public string rarity;
    public string icon;

    public int ID;
    public string Title;
    public int Value;
    public string Type;
    public int Defence;
    public string Description;
    public bool Stackable;
    public string Rarity;
    public string Icon;
    public Sprite Sprite;

    public Armor()
    {
        
        //this.Sprite = Resources.Load<Sprite>("Sprite/Items/Armors/" + icon);
    }

    //public Armor()
    //{
    //    this.ID = -1;
    //}
}