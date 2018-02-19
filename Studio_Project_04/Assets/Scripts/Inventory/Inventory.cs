using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class InventoryObject
{
    public Item item;
    public Weapon weapon;
    public Armor armor;

    public string itemType;

    public bool isEmpty;

    public InventoryObject()
    {
        isEmpty = true;
    }
}

public class Inventory : MonoBehaviour, IDragHandler
{

    ItemDatabase itemDatabase;
    WeaponDatabase weaponDatabase;
    ArmorDatabase armorDatabase;
    GameObject inventoryPanel;
    GameObject slotPanel;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    private int slotAmount;

    public List<InventoryObject> items = new List<InventoryObject>();
    public List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        itemDatabase = GetComponent<ItemDatabase>();
        weaponDatabase = GetComponent<WeaponDatabase>();
        armorDatabase = GetComponent<ArmorDatabase>();

        slotAmount = 20;
        inventoryPanel = GameObject.Find("Inventory");
        slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;

        for(int i = 0; i < slotAmount; i++)
        {
            items.Add(new InventoryObject());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }

        AddItem(0);
        AddItem(0);
        AddItem(0);
        AddWeapon(0);
    }

    public void AddItem(int id)
    {
        Item itemToAdd = itemDatabase.FetchItemByID(id);

        if(itemToAdd.Stackable && checkForItem(itemToAdd))
        {
            for(int i = 0; i < items.Count; i++)
            {
                if (!items[i].isEmpty && items[i].item.ID == id)
                {
                    if (items[i].item.ID == id)
                    {
                        ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                        data.amount++;
                        data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].isEmpty)
                {
                    items[i].item = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemToAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Title;
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    if(itemToAdd.Type == "Consumable")
                    {
                        slots[data.slot].transform.GetChild(0).gameObject.AddComponent<ConsumableItem>();
                    }
                    items[i].isEmpty = false;
                    items[i].itemType = "Item";
                    break;
                }
            }
        }
    }

    public void AddWeapon(int id)
    {
        Weapon weaponToAdd = weaponDatabase.FetchWeaponByID(id);

        if (weaponToAdd.Stackable && checkForItem(weaponToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].isEmpty && items[i].weapon.ID == id)
                {
                    if (items[i].weapon.ID == id)
                    {
                        ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                        data.amount++;
                        data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].isEmpty)
                {
                    items[i].weapon = weaponToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().weapon = weaponToAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = weaponToAdd.Sprite;
                    itemObj.name = weaponToAdd.Title;
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    items[i].isEmpty = false;
                    items[i].itemType = "Weapon";
                    break;
                }
            }
        }
    }

    bool checkForItem(Item item)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].item != null)
            {
                if (items[i].item.ID == item.ID)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool checkForItem(Weapon weapon)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].weapon != null)
            {
                if (items[i].weapon.ID == weapon.ID)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool checkForItem(Armor armor)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].armor != null)
            {
                if (items[i].armor.ID == armor.ID)
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
