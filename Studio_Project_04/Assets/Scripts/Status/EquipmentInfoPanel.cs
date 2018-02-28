using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInfoPanel : MonoBehaviour {

    private Weapon weapon;
    private Armor armor;

    private string title;
    private string info;
    private GameObject infoPanel;
    private GameObject itemName;
    private GameObject itemInfo;
    private GameObject unequipButton;

    void Start()
    {
        infoPanel = this.gameObject.transform.GetChild(0).gameObject;
        itemName = infoPanel.transform.GetChild(1).gameObject;
        itemInfo = infoPanel.transform.GetChild(2).gameObject;
        unequipButton = GameObject.Find("Unequip");
        unequipButton.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void Activate(ItemData data)
    {
        if (data.weapon != null)
        {
            this.weapon = data.weapon;
            ConstructWeaponDataString();
            infoPanel.SetActive(true);
            unequipButton.SetActive(true);
            unequipButton.GetComponent<ButtonScript>().itemData = data;
            infoPanel.transform.GetChild(0).GetComponent<Image>().sprite = weapon.Sprite;
        }
        if (data.armor != null)
        {
            this.armor = data.armor;
            ConstructArmorDataString();
            infoPanel.SetActive(true);
            unequipButton.SetActive(true);
            unequipButton.GetComponent<ButtonScript>().itemData = data;
            infoPanel.transform.GetChild(0).GetComponent<Image>().sprite = armor.Sprite;
        }

    }

    public void Deactivate()
    {
        infoPanel.SetActive(false);
        unequipButton.SetActive(false);
    }

    public void ConstructWeaponDataString()
    {
        title = weapon.Title;
        info = weapon.Description + "\n" + "\nATK : " + weapon.Attack + "\nSTR : " + weapon.Strength + "\nVIT : " + weapon.Vitality + "\nINT : " + weapon.Intelligence + "\nDEX : " + weapon.Dexterity + "\nRarity : " + weapon.Rarity + "\nValue : " + weapon.Value;
        itemName.GetComponent<Text>().text = title;
        itemInfo.GetComponent<Text>().text = info;
    }

    public void ConstructArmorDataString()
    {
        title = armor.Title;
        info = armor.Description + "\n" + "\nDEF : " + armor.Defence + "\nSTR : " + armor.Strength + "\nVIT : " + armor.Vitality + "\nINT : " + armor.Intelligence + "\nDEX : " + armor.Dexterity + "\nRarity : " + armor.Rarity + "\nValue : " + armor.Value;
        itemName.GetComponent<Text>().text = title;
        itemName.GetComponent<Text>().text = title;
        itemInfo.GetComponent<Text>().text = info;
    }
}
