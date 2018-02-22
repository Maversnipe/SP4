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

    public GameObject amountField;

	// Use this for initialization
	void Start () {
        amountField = GameObject.Find("AmountField");
    }
	
    public void closePanel()
    {
        this.transform.parent.gameObject.SetActive(false);
    }

    public void useItem()
    {
        Inventory.Instance.slots[itemData.slot].transform.GetChild(0).GetComponent<ConsumableItem>().Use();
        itemData.amount--;
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
            this.transform.parent.gameObject.SetActive(false);
        }
        else if (sellItemData.weapon != null)
        {
            int itemID = sellItemData.weapon.ID;
            Inventory.Instance.RemoveWeapon(itemID, amount);
            amount = 0;
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
