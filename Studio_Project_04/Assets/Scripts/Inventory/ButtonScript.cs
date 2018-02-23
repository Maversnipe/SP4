using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

    //public Inventory inv;

    public int amount;
    public ItemData sellItemData;
    public ItemData itemData;
    public ShopItemData shopItemData;

    public GameObject statusMenu;
    public GameObject equipmentSlotPanel;

    public GameObject amountField;

	// Use this for initialization
	void Start () {
        amountField = GameObject.Find("AmountField");
        statusMenu = GameObject.Find("StatusMenu");
        equipmentSlotPanel = statusMenu.transform.Find("Equipment Slot Panel " + statusMenu.GetComponent<StatusMenu>().currPlayerUnit).gameObject;
    }

    private void Update()
    {
        equipmentSlotPanel = statusMenu.transform.Find("Equipment Slot Panel " + statusMenu.GetComponent<StatusMenu>().currPlayerUnit).gameObject;
    }
	
    public void closePanel()
    {
        this.transform.parent.gameObject.SetActive(false);
    }

    public void equipItem()
    {
        if(itemData.weapon != null)
        {
            if (equipmentSlotPanel.transform.Find("Weapon Slot").GetComponent<EquipmentSlot>().isEmpty)
            {
                itemData.equipped = true;
                itemData.transform.SetParent(equipmentSlotPanel.transform.Find("Weapon Slot").transform);
                itemData.transform.position = equipmentSlotPanel.transform.Find("Weapon Slot").transform.position;
                equipmentSlotPanel.transform.Find("Weapon Slot").GetComponent<EquipmentSlot>().temp = Inventory.Instance.items[itemData.slot];
                Inventory.Instance.items[itemData.slot] = new InventoryObject();
                equipmentSlotPanel.transform.Find("Weapon Slot").GetComponent<EquipmentSlot>().isEmpty = false;
            }
        }
    }

    public void unequipItem()
    {
        if(itemData.weapon != null)
        {
            if (!equipmentSlotPanel.transform.Find("Weapon Slot").GetComponent<EquipmentSlot>().isEmpty)
            {
                itemData.equipped = false;

                for (int i = 0; i < Inventory.Instance.items.Count; i++)
                {
                    if (Inventory.Instance.items[i].isEmpty)
                    {
                        itemData.slot = i;
                        itemData.equipped = false;
                        Inventory.Instance.items[i] = equipmentSlotPanel.transform.Find("Weapon Slot").GetComponent<EquipmentSlot>().temp;
                        equipmentSlotPanel.transform.Find("Weapon Slot").GetComponent<EquipmentSlot>().isEmpty = true;
                        itemData.transform.SetParent(Inventory.Instance.slots[i].transform);
                        itemData.transform.position = Inventory.Instance.slots[i].transform.position;
                        break;
                    
                    }
                }
            }
        }
        
    }

    public void useItem()
    {
        Inventory.Instance.slots[itemData.slot].transform.GetChild(0).GetComponent<ConsumableItem>().Use();
        int itemID = itemData.item.ID;
        Inventory.Instance.RemoveItem(itemID, 1);
    }

    public void buyItem()
    {
        if(shopItemData.item != null)
        {
            int itemID = shopItemData.item.ID;
            Inventory.Instance.AddItem(itemID, 1);
        }
        else if (shopItemData.weapon != null)
        {
            int itemID = shopItemData.weapon.ID;
            Inventory.Instance.AddWeapon(itemID, 1);
        }
        //if (shopItemData.armor != null)
        //{
        //    int itemID = shopItemData.item.ID;
        //    inv.GetComponent<Inventory>().AddItem(itemID);
        //}
    }

    public void sellItem()
    {
        if (sellItemData.item != null)
        {
            int itemID = sellItemData.item.ID;
            Inventory.Instance.RemoveItem(itemID, amount);
            amount = 0;
            amountField.GetComponent<InputField>().text = "";
            this.transform.parent.gameObject.SetActive(false);
        }
        else if (sellItemData.weapon != null)
        {
            int itemID = sellItemData.weapon.ID;
            if(sellItemData.weapon.Stackable)
            {
                Inventory.Instance.RemoveWeapon(itemID, amount);
            }
            else
            {
                Inventory.Instance.RemoveWeapon(itemID, amount, sellItemData.slot);
            }
            amount = 0;
            amountField.GetComponent<InputField>().text = "";
            this.transform.parent.gameObject.SetActive(false);
        }
        //if (shopItemData.armor != null)
        //{
        //    int itemID = shopItemData.item.ID;
        //    inv.GetComponent<Inventory>().AddItem(itemID);
        //}
    }

    public void setAmount()
    {
        amount = int.Parse(amountField.GetComponent<InputField>().text);
    }
}
