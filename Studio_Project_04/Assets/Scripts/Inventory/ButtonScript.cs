using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

    //public Inventory inv;

    public int amount;
    public ItemData sellItemData;
    public ItemData itemData;
    public ShopItemData boughtItemData;
    public ShopItemData shopItemData;

    public GameObject equipmentSlotPanel;
    private GameObject unitSelectPanel;
    public EquipmentInfoPanel equipmentInfoPanel;
    private GameObject buyConfirmDialog;
    public GameObject sellAmountField;
    public GameObject buyAmountField;

    // Use this for initialization
    void Start () {
        buyConfirmDialog = GameObject.FindWithTag("BuyConfirmDialog");
        Debug.Log(buyConfirmDialog);
        unitSelectPanel = GameObject.FindWithTag("UnitSelectPanel");
        sellAmountField = GameObject.Find("SellAmountField");
        buyAmountField = GameObject.Find("BuyAmountField");
        equipmentInfoPanel = StatusMenu.Instance.GetComponent<EquipmentInfoPanel>();
        equipmentSlotPanel = StatusMenu.Instance.gameObject.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).gameObject;
    }

    private void Update()
    {
        equipmentSlotPanel = StatusMenu.Instance.gameObject.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).gameObject;
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
            else if (!equipmentSlotPanel.transform.Find("Weapon Slot").GetComponent<EquipmentSlot>().isEmpty)
            {
                Transform item = equipmentSlotPanel.transform.Find("Weapon Slot").transform.GetChild(0);

                item.GetComponent<ItemData>().equipped = false;
                item.GetComponent<ItemData>().slot = itemData.slot;
                InventoryObject temp2 = equipmentSlotPanel.transform.Find("Weapon Slot").GetComponent<EquipmentSlot>().temp;

                equipmentSlotPanel.transform.Find("Weapon Slot").GetComponent<EquipmentSlot>().temp = Inventory.Instance.items[itemData.slot];
                Inventory.Instance.items[itemData.slot] = new InventoryObject();
                Inventory.Instance.items[itemData.slot] = temp2;


                item.transform.SetParent(Inventory.Instance.slots[itemData.slot].transform);
                item.transform.position = Inventory.Instance.slots[itemData.slot].transform.position;

                itemData.equipped = true;
                itemData.transform.SetParent(equipmentSlotPanel.transform.Find("Weapon Slot").transform);
                itemData.transform.position = equipmentSlotPanel.transform.Find("Weapon Slot").transform.position;
            }
        }
        if(itemData.armor != null)
        {
            if (equipmentSlotPanel.transform.Find("Armor Slot").GetComponent<EquipmentSlot>().isEmpty)
            {
                itemData.equipped = true;
                itemData.transform.SetParent(equipmentSlotPanel.transform.Find("Armor Slot").transform);
                itemData.transform.position = equipmentSlotPanel.transform.Find("Armor Slot").transform.position;
                equipmentSlotPanel.transform.Find("Armor Slot").GetComponent<EquipmentSlot>().temp = Inventory.Instance.items[itemData.slot];
                Inventory.Instance.items[itemData.slot] = new InventoryObject();
                equipmentSlotPanel.transform.Find("Armor Slot").GetComponent<EquipmentSlot>().isEmpty = false;
            }
            else if (!equipmentSlotPanel.transform.Find("Armor Slot").GetComponent<EquipmentSlot>().isEmpty)
            {
                Transform item = equipmentSlotPanel.transform.Find("Armor Slot").transform.GetChild(0);

                item.GetComponent<ItemData>().equipped = false;
                item.GetComponent<ItemData>().slot = itemData.slot;
                InventoryObject temp2 = equipmentSlotPanel.transform.Find("Armor Slot").GetComponent<EquipmentSlot>().temp;

                equipmentSlotPanel.transform.Find("Armor Slot").GetComponent<EquipmentSlot>().temp = Inventory.Instance.items[itemData.slot];
                Inventory.Instance.items[itemData.slot] = new InventoryObject();
                Inventory.Instance.items[itemData.slot] = temp2;


                item.transform.SetParent(Inventory.Instance.slots[itemData.slot].transform);
                item.transform.position = Inventory.Instance.slots[itemData.slot].transform.position;

                itemData.equipped = true;
                itemData.transform.SetParent(equipmentSlotPanel.transform.Find("Armor Slot").transform);
                itemData.transform.position = equipmentSlotPanel.transform.Find("Armor Slot").transform.position;
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
                equipmentInfoPanel.Deactivate();
                for (int i = 0; i < Inventory.Instance.items.Count; i++)
                {
                    if (Inventory.Instance.items[i].isEmpty)
                    {
                        itemData.slot = i;
                        itemData.equipped = false;
                        StatusMenu.Instance.players[StatusMenu.Instance.currPlayerUnit].GetComponent<UnitVariables>()._weapon = null;
                        Inventory.Instance.items[i] = equipmentSlotPanel.transform.Find("Weapon Slot").GetComponent<EquipmentSlot>().temp;
                        equipmentSlotPanel.transform.Find("Weapon Slot").GetComponent<EquipmentSlot>().isEmpty = true;
                        itemData.transform.SetParent(Inventory.Instance.slots[i].transform);
                        itemData.transform.position = Inventory.Instance.slots[i].transform.position;
                        break;
                    
                    }
                }
            }
        }
        if (itemData.armor != null)
        {
            if (!equipmentSlotPanel.transform.Find("Armor Slot").GetComponent<EquipmentSlot>().isEmpty)
            {
                itemData.equipped = false;
                equipmentInfoPanel.Deactivate();
                for (int i = 0; i < Inventory.Instance.items.Count; i++)
                {
                    if (Inventory.Instance.items[i].isEmpty)
                    {
                        itemData.slot = i;
                        itemData.equipped = false;
                        StatusMenu.Instance.players[StatusMenu.Instance.currPlayerUnit].GetComponent<UnitVariables>()._armor = null;
                        Inventory.Instance.items[i] = equipmentSlotPanel.transform.Find("Armor Slot").GetComponent<EquipmentSlot>().temp;
                        equipmentSlotPanel.transform.Find("Armor Slot").GetComponent<EquipmentSlot>().isEmpty = true;
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
        unitSelectPanel.SetActive(true);
        unitSelectPanel.GetComponent<UnitPanelScript>().data = itemData;
    }

    public void activateConfirmDialog()
    {
        buyConfirmDialog.SetActive(true);
        buyConfirmDialog.transform.GetChild(0).GetComponent<ButtonScript>().boughtItemData = shopItemData;
    }

    public void buyItem()
    {
        if(boughtItemData.item != null)
        {
            if (boughtItemData.GetComponent<ShopItemData>().item.Value * amount <= Inventory.Instance.gold)
            {
                int itemID = boughtItemData.item.ID;
                Inventory.Instance.AddItem(itemID, amount);
                Inventory.Instance.gold -= boughtItemData.GetComponent<ShopItemData>().item.Value * amount;
                amount = 0;
                buyAmountField.GetComponent<InputField>().text = "";
                this.transform.parent.gameObject.SetActive(false);
            }
        }
        else if (boughtItemData.weapon != null)
        {
            if (boughtItemData.GetComponent<ShopItemData>().weapon.Value * amount <= Inventory.Instance.gold)
            {
                int itemID = boughtItemData.weapon.ID;

                if (boughtItemData.weapon.Stackable)
                {
                    Inventory.Instance.AddWeapon(itemID, amount);
                    Inventory.Instance.gold -= boughtItemData.GetComponent<ShopItemData>().weapon.Value * amount;
                    amount = 0;
                    buyAmountField.GetComponent<InputField>().text = "";
                    this.transform.parent.gameObject.SetActive(false);
                }
                else if (!boughtItemData.weapon.Stackable)
                {
                    if (amount <= Inventory.Instance.emptySlots)
                    {
                        for (int i = 0; i < amount; i++)
                        {
                            Inventory.Instance.AddWeapon(itemID, 1);

                        }
                        buyAmountField.GetComponent<InputField>().text = "";
                        Inventory.Instance.gold -= boughtItemData.GetComponent<ShopItemData>().weapon.Value * amount;
                        this.transform.parent.gameObject.SetActive(false);
                        amount = 0;
                    }
                }
            }  
        }
        else if (boughtItemData.armor != null)
        {
            if (boughtItemData.GetComponent<ShopItemData>().armor.Value * amount <= Inventory.Instance.gold)
            {
                int itemID = boughtItemData.armor.ID;

                if (boughtItemData.weapon.Stackable)
                {
                    Inventory.Instance.AddArmor(itemID, amount);
                    Inventory.Instance.gold -= boughtItemData.GetComponent<ShopItemData>().armor.Value * amount;
                    amount = 0;
                    buyAmountField.GetComponent<InputField>().text = "";
                    this.transform.parent.gameObject.SetActive(false);
                }
                else if (!boughtItemData.armor.Stackable)
                {
                    if (amount <= Inventory.Instance.emptySlots)
                    {
                        for (int i = 0; i < amount; i++)
                        {
                            Inventory.Instance.AddArmor(itemID, 1);

                        }
                        buyAmountField.GetComponent<InputField>().text = "";
                        Inventory.Instance.gold -= boughtItemData.GetComponent<ShopItemData>().armor.Value * amount;
                        this.transform.parent.gameObject.SetActive(false);
                        amount = 0;
                    }
                }
            }
        }
    }

    public void sellItem()
    {
        if (sellItemData.item != null)
        {
            int itemID = sellItemData.item.ID;
            Inventory.Instance.RemoveItem(itemID, amount);
            Inventory.Instance.gold += sellItemData.GetComponent<ItemData>().item.Value * amount;
            amount = 0;
            sellAmountField.GetComponent<InputField>().text = "";
            
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
            Inventory.Instance.gold += sellItemData.GetComponent<ItemData>().weapon.Value * amount;
            amount = 0;
            sellAmountField.GetComponent<InputField>().text = "";
      

            this.transform.parent.gameObject.SetActive(false);
        }
        else if (sellItemData.armor != null)
        {
            int itemID = sellItemData.armor.ID;
            if (sellItemData.armor.Stackable)
            {
                Inventory.Instance.RemoveArmor(itemID, amount);
            }
            else
            {
                Inventory.Instance.RemoveArmor(itemID, amount, sellItemData.slot);
            }
            Inventory.Instance.gold += sellItemData.GetComponent<ItemData>().armor.Value * amount;
            amount = 0;
            sellAmountField.GetComponent<InputField>().text = "";

            this.transform.parent.gameObject.SetActive(false);
        }
    }

    public void setSellAmount()
    {
        amount = int.Parse(sellAmountField.GetComponent<InputField>().text);
    }

    public void setBuyAmount()
    {
        amount = int.Parse(buyAmountField.GetComponent<InputField>().text);
    }
}
