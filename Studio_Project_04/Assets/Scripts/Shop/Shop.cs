using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopObject
{
    //Class to represent objects in inventory, stores a reference to a item, weapon or armor
    public Item item;
    public Weapon weapon;
    public Armor armor;

    public string itemType;

    //Boolean to check if the slot its in is empty
    public bool isEmpty;

    public ShopObject()
    {
        isEmpty = true;
    }
}

public class Shop : MonoBehaviour, IDragHandler {

    private int slotAmount;

    
    GameObject shopPanel;
    GameObject slotPanel;
    public GameObject shopSlot;
    public GameObject shopItem;

    //List of items
    public List<ShopObject> shopItems = new List<ShopObject>();
    //List of slots
    public List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        
        slotAmount = 20;
        shopPanel = GameObject.Find("Shop");
        slotPanel = shopPanel.transform.Find("Shop Slot Panel").gameObject;

        for (int i = 0; i < slotAmount; i++)
        {
            shopItems.Add(new ShopObject());
            slots.Add(Instantiate(shopSlot));
            slots[i].GetComponent<ShopSlot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }

        AddShopItem(0);
        AddShopWeapon(0);

    }

    //Add item by id
    public void AddShopItem(int id)
    {
        Item itemToAdd = ItemDatabase.Instance.FetchItemByID(id);

        for (int i = 0; i < shopItems.Count; i++)
        {
            if (shopItems[i].isEmpty)
            {
                shopItems[i].item = itemToAdd;
                GameObject itemObj = Instantiate(shopItem);
                itemObj.GetComponent<ShopItemData>().item = itemToAdd;
                itemObj.GetComponent<ShopItemData>().slot = i;
                itemObj.transform.SetParent(slots[i].transform);
                itemObj.transform.localPosition = Vector2.zero;
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                itemObj.name = itemToAdd.Title;
                ShopItemData data = slots[i].transform.GetChild(0).GetComponent<ShopItemData>();
                shopItems[i].isEmpty = false;
                shopItems[i].itemType = "Item";
                break;
            }
        }
    }

    //Add weapon by id
    public void AddShopWeapon(int id)
    {
        Weapon weaponToAdd = WeaponDatabase.Instance.FetchWeaponByID(id);

        for (int i = 0; i < shopItems.Count; i++)
        {
            if (shopItems[i].isEmpty)
            {
                shopItems[i].weapon = weaponToAdd;
                GameObject itemObj = Instantiate(shopItem);
                itemObj.GetComponent<ShopItemData>().weapon = weaponToAdd;
                itemObj.GetComponent<ShopItemData>().slot = i;
                itemObj.transform.SetParent(slots[i].transform);
                itemObj.transform.localPosition = Vector2.zero;
                itemObj.GetComponent<Image>().sprite = weaponToAdd.Sprite;
                itemObj.name = weaponToAdd.Title;
                ShopItemData data = slots[i].transform.GetChild(0).GetComponent<ShopItemData>();
                shopItems[i].isEmpty = false;
                shopItems[i].itemType = "Weapon";
                break;
            }
        }
    }

    bool checkForItem(Item item)
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            if (shopItems[i].item != null)
            {
                if (shopItems[i].item.ID == item.ID)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool checkForItem(Weapon weapon)
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            if (shopItems[i].weapon != null)
            {
                if (shopItems[i].weapon.ID == weapon.ID)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool checkForItem(Armor armor)
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            if (shopItems[i].armor != null)
            {
                if (shopItems[i].armor.ID == armor.ID)
                {
                    return true;
                }
            }

        }
        return false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

}
