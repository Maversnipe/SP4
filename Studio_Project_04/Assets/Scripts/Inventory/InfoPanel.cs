﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {

    private Item item;
    private Weapon weapon;
    private Armor armor;

    private string title;
    private string info;
    private GameObject infoPanel;
    private GameObject itemName;
    private GameObject itemInfo;
    private GameObject useButton;
    private GameObject equipButton;

    void Start()
    {
        infoPanel = this.gameObject.transform.GetChild(0).gameObject;
        itemName = infoPanel.transform.GetChild(1).gameObject;
        itemInfo = infoPanel.transform.GetChild(2).gameObject;
        useButton = GameObject.Find("Use");
        equipButton = GameObject.Find("Equip");
        useButton.SetActive(false);
        equipButton.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void Activate(ItemData data)
    {
        if(data.item != null)
        {
            this.item = data.item;
            ConstructItemDataString();
            infoPanel.SetActive(true);
            equipButton.SetActive(false);
            useButton.SetActive(true);
            useButton.GetComponent<ButtonScript>().itemData = data;
            infoPanel.transform.GetChild(0).GetComponent<Image>().sprite = item.Sprite;
        }
        if (data.weapon != null)
        {
            this.weapon = data.weapon;
            ConstructWeaponDataString();
            infoPanel.SetActive(true);
            useButton.SetActive(false);
            equipButton.SetActive(true);
            equipButton.GetComponent<ButtonScript>().itemData = data;
            infoPanel.transform.GetChild(0).GetComponent<Image>().sprite = weapon.Sprite;
        }
        if (data.armor != null)
        {
            this.armor = data.armor;
            ConstructArmorDataString();
            infoPanel.SetActive(true);
            useButton.SetActive(false);
            equipButton.SetActive(true);
            equipButton.GetComponent<ButtonScript>().itemData = data;
            infoPanel.transform.GetChild(0).GetComponent<Image>().sprite = armor.Sprite;
        }
    }

    public void Deactivate()
    {
        infoPanel.SetActive(false);
        useButton.SetActive(false);
        equipButton.SetActive(false);
    }

    public void ConstructItemDataString()
    {
        title = item.Title;
        info = item.Description + "\n" + "\nRarity : " + item.Rarity;
        itemName.GetComponent<Text>().text = title;
        itemInfo.GetComponent<Text>().text = info;
    }

    public void ConstructWeaponDataString()
    {
        title = weapon.Title;
        info = weapon.Description + "\n" + "\nATK : " + weapon.Attack + "\nRarity : " + weapon.Rarity + "\nValue : " + weapon.Value;
        itemName.GetComponent<Text>().text = title;
        itemInfo.GetComponent<Text>().text = info;
    }

    public void ConstructArmorDataString()
    {
        title = armor.Title;
        info = armor.Description + "\n" + "\nDEF : " + armor.Defence + "\nRarity : " + armor.Rarity + "\nValue : " + armor.Value;
        itemName.GetComponent<Text>().text = title; 
        itemName.GetComponent<Text>().text = title;
        itemInfo.GetComponent<Text>().text = info;
    }
}
