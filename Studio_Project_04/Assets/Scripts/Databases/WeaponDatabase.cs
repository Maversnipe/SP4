using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;

using System.IO;
using System;

[System.Serializable]
public class WeaponDatabase : GenericSingleton<WeaponDatabase>
{
    [SerializeField]
    public static Weapon[] weaponDatabase;
    private string weaponData;

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
            weaponDatabase = JsonHelper.FromJson<Weapon>(result);
        }
        else
        {
            result = System.IO.File.ReadAllText(filePath);
            Debug.Log(result);
            weaponDatabase = JsonHelper.FromJson<Weapon>(result);
        }
    }

    // Use this for initialization
    void Start () {
        //weaponData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Weapons.json"));
        StartCoroutine(loadStreamingAsset("Weapons.json"));

        ConstructWeaponDatabase();
        Debug.Log(weaponDatabase[0].Description);

    }

    public Weapon FetchWeaponByName(string name)
    {
        for (int i = 0; i < weaponDatabase.Length; i++)
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
        for(int i = 0; i < weaponDatabase.Length; i++)
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
        for (int i = 0; i < weaponDatabase.Length; i++)
        {
            weaponDatabase[i].ID = weaponDatabase[i].id;
            weaponDatabase[i].Title = weaponDatabase[i].title;
            weaponDatabase[i].Value = weaponDatabase[i].value;
            weaponDatabase[i].Type = weaponDatabase[i].type;
            weaponDatabase[i].Attack = weaponDatabase[i].attack;
            weaponDatabase[i].AP = weaponDatabase[i].ap;
            weaponDatabase[i].Range = weaponDatabase[i].range;
            weaponDatabase[i].Description = weaponDatabase[i].description;
            weaponDatabase[i].Stackable = weaponDatabase[i].stackable;
            weaponDatabase[i].Rarity = weaponDatabase[i].rarity;
            weaponDatabase[i].Icon = weaponDatabase[i].icon;
            weaponDatabase[i].Sprite = Resources.Load<Sprite>("Sprite/Items/Weapons/" + weaponDatabase[i].Icon);
        }
    }
}

[Serializable]
public class Weapon
{
    public int id;
    public string title;
    public int value;
    public string type;
    public int attack;
    public int ap;
    public int range;
    public string description;
    public bool stackable;
    public string rarity;
    public string icon;

    public int ID;
    public string Title;
    public int Value;
    public string Type;
    public int Attack;
    public int AP;
    public int Range;
    public string Description;
    public bool Stackable;
    public string Rarity;
    public string Icon;
    public Sprite Sprite;

	public Weapon()
    {
        //this.Sprite = Resources.Load<Sprite>("Sprite/Items/Weapons/" + Icon);
    }

    //public Weapon()
    //{
    //    this.ID = -1;
    //}
}