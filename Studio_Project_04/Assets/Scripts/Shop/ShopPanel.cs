﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{

    private Item item;
    private Weapon weapon;
    private Armor armor;

    private string title;
    private string info;
    private GameObject infoPanel;
    private GameObject itemName;
    private GameObject itemInfo;
    private GameObject buyButton;

    void Start()
    {
        infoPanel = this.gameObject.transform.GetChild(0).gameObject;
        itemName = infoPanel.transform.GetChild(1).gameObject;
        itemInfo = infoPanel.transform.GetChild(2).gameObject;
        buyButton = GameObject.Find("Buy");
        buyButton.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void Activate(ShopItemData data)
    {
        if (data.item != null)
        {
            this.item = data.item;
            ConstructItemDataString();
            infoPanel.SetActive(true);
            buyButton.SetActive(true);
            buyButton.GetComponent<ButtonScript>().shopItemData = data;
            infoPanel.transform.GetChild(0).GetComponent<Image>().sprite = item.Sprite;
        }
        if (data.weapon != null)
        {
            this.weapon = data.weapon;
            ConstructWeaponDataString();
            infoPanel.SetActive(true);
            buyButton.SetActive(true);
            buyButton.GetComponent<ButtonScript>().shopItemData = data;
            infoPanel.transform.GetChild(0).GetComponent<Image>().sprite = weapon.Sprite;
        }
        if (data.armor != null)
        {
            this.armor = data.armor;
            ConstructWeaponDataString();
            infoPanel.SetActive(true);
            buyButton.SetActive(true);
            buyButton.GetComponent<ButtonScript>().shopItemData = data;
            infoPanel.transform.GetChild(0).GetComponent<Image>().sprite = armor.Sprite;
        }
    }

    public void Deactivate()
    {
        infoPanel.SetActive(false);
        buyButton.SetActive(false);
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
