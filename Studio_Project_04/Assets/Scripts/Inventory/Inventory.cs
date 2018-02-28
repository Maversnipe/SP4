using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class InventoryObject
{
    //Class to represent objects in inventory, stores a reference to a item, weapon or armor
    public Item item;
    public Weapon weapon;
    public Armor armor;

    public string itemType;

    //Boolean to check if the slot its in is empty
    public bool isEmpty;

    public InventoryObject()
    {
        isEmpty = true;
    }
}

[System.Serializable]
public class Inventory : GenericSingleton<Inventory>, IDragHandler
{
    GameObject buyConfirmDialog;
    GameObject inventoryPanel;
    GameObject slotPanel;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    private int slotAmount;
    public int emptySlots;
    public int gold;
    public List<InventoryObject> items = new List<InventoryObject>();
    public List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        buyConfirmDialog = GameObject.FindGameObjectWithTag("BuyConfirmDialog");
        buyConfirmDialog.SetActive(false);
        slotAmount = 20;
        emptySlots = slotAmount;
        inventoryPanel = GameObject.Find("Inventory");
        slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;

        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new InventoryObject());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform, false);
        }

        gold += 500;

        AddItem(0, 3);
        AddWeapon(0, 1);
        AddArmor(1, 1);

    }

    void Update()
    {
        int no = 0;

        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].isEmpty)
            {
                no++;
            }
        }

        emptySlots = no;

        this.transform.Find("Gold Panel").Find("Gold").GetComponent<Text>().text = gold.ToString();
    }

    public void AddItem(int id, int no)
    {
        Item itemToAdd = ItemDatabase.Instance.FetchItemByID(id);

        if (itemToAdd.Stackable && checkForItem(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].isEmpty && items[i].item != null)
                {
                    if (items[i].item.ID == id)
                    {
                        ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                        data.amount += no;
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
                    itemObj.transform.localPosition = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Title;
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    if(itemToAdd.Stackable)
                    {
                        data.amount += no;
                        data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    }
                    else
                    {
                        data.amount++;
                        data.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    if (itemToAdd.Type == "Consumable")
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

    public void RemoveItem(int id, int no)
    {
        Item itemToRemove = ItemDatabase.Instance.FetchItemByID(id);

        bool removeItem = false;

        if (itemToRemove.Stackable && checkForItem(itemToRemove))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].isEmpty && items[i].item != null)
                {
                    if (items[i].item.ID == id)
                    {
                        ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                        if ((data.amount - no) <= 0)
                        {
                            removeItem = true;
                            break;
                        }
                        data.amount -= no;
                        data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                        break;
                    }
                }
            }
        }
        else
        {
            if (checkForItem(itemToRemove))
            {
                GameObject.Destroy(slots[FindItemToRemove(itemToRemove)].transform.GetChild(0).gameObject);
                items[FindItemToRemove(itemToRemove)] = new InventoryObject();
            }
        }

        if(removeItem)
        {
            if (checkForItem(itemToRemove))
            {
                GameObject.Destroy(slots[FindItemToRemove(itemToRemove)].transform.GetChild(0).gameObject);
                items[FindItemToRemove(itemToRemove)] = new InventoryObject();
            }
        }

    }

    public void AddWeapon(int id, int no)
    {
        Weapon weaponToAdd = WeaponDatabase.Instance.FetchWeaponByID(id);

        if (weaponToAdd.Stackable && checkForItem(weaponToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].isEmpty && items[i].weapon != null)
                {
                    if (items[i].weapon.ID == id)
                    {
                        ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                        data.amount += no;
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
                    itemObj.transform.localPosition = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = weaponToAdd.Sprite;
                    itemObj.name = weaponToAdd.Title;
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    if (weaponToAdd.Stackable)
                    {
                        data.amount += no;
                        data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    }
                    else
                    {
                        data.amount++;
                        data.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    items[i].isEmpty = false;
                    items[i].itemType = "Weapon";
                    break;
                }
            }
        }
    }

    public void RemoveWeapon(int id, int no)
    {
        Weapon weaponToRemove = WeaponDatabase.Instance.FetchWeaponByID(id);

        bool removeItem = false;

        if (weaponToRemove.Stackable && checkForItem(weaponToRemove))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].isEmpty && items[i].weapon != null)
                {
                    if (items[i].weapon.ID == id)
                    {
                        ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                        if((data.amount - no) <= 0)
                        {
                            removeItem = true;
                            break;
                        }
                        data.amount -= no;
                        data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                        break;
                    }
                }
            }
        }
    }

    public void RemoveWeapon(int id, int no, int slot)
    {
        Weapon weaponToRemove = WeaponDatabase.Instance.FetchWeaponByID(id);

        if (checkForItem(weaponToRemove))
        {
            items[slot] = new InventoryObject();
            GameObject.Destroy(slots[slot].transform.GetChild(0).gameObject);
        }
    }

    public void AddArmor(int id, int no)
    {
        Armor armorToAdd = ArmorDatabase.Instance.FetchArmorByID(id);

        if (armorToAdd.Stackable && checkForItem(armorToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].isEmpty && items[i].armor != null)
                {
                    if (items[i].weapon.ID == id)
                    {
                        ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                        data.amount += no;
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
                    items[i].armor = armorToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().armor = armorToAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.localPosition = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = armorToAdd.Sprite;
                    itemObj.name = armorToAdd.Title;
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    if (armorToAdd.Stackable)
                    {
                        data.amount += no;
                        data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    }
                    else
                    {
                        data.amount++;
                        data.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    items[i].isEmpty = false;
                    items[i].itemType = "Armor";
                    break;
                }
            }
        }
    }

    public void RemoveArmor(int id, int no)
    {
        Armor armorToRemove = ArmorDatabase.Instance.FetchArmorByID(id);

        bool removeItem = false;

        if (armorToRemove.Stackable && checkForItem(armorToRemove))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].isEmpty && items[i].armor != null)
                {
                    if (items[i].weapon.ID == id)
                    {
                        ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                        if ((data.amount - no) <= 0)
                        {
                            removeItem = true;
                            break;
                        }
                        data.amount -= no;
                        data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                        break;
                    }
                }
            }
        }
    }

    public void RemoveArmor(int id, int no, int slot)
    {
        Armor armorToRemove = ArmorDatabase.Instance.FetchArmorByID(id);

        if (checkForItem(armorToRemove))
        {
            items[slot] = new InventoryObject();
            GameObject.Destroy(slots[slot].transform.GetChild(0).gameObject);
        }
    }

    bool checkForItem(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item != null)
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
            if (items[i].weapon != null)
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
            if (items[i].armor != null)
            {
                if (items[i].armor.ID == armor.ID)
                {
                    return true;
                }
            }
        }
        return false;
    }

    int FindItemToRemove(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item != null)
            {
                if (items[i].item.ID == item.ID)
                {
                    return i;
                }
            }
        }
        return 0;
    }

    int FindItemToRemove(Weapon weapon)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].weapon != null)
            {
                if (items[i].weapon.ID == weapon.ID)
                {
                    return i;
                }
            }
        }
        return 0;
    }

    int FindItemToRemove(Armor armor)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].armor != null)
            {
                if (items[i].armor.ID == armor.ID)
                {
                    return i;
                }
            }
        }
        return 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }
}
