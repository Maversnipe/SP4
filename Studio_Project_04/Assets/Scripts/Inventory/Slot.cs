using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    public int id;

    // Use this for initialization
    void Start () {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

        if (Inventory.Instance.items[id].isEmpty)
        {
            if (droppedItem.equipped)
            {
                droppedItem.dropped = true;
                droppedItem.equipped = false;
                Inventory.Instance.items[id] = StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot).GetComponent<EquipmentSlot>().temp;
                StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot).GetComponent<EquipmentSlot>().isEmpty = true;
                StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot).GetComponent<EquipmentSlot>().temp = new InventoryObject();
                droppedItem.slot = id;
            }
            else
            {
                Inventory.Instance.items[id] = Inventory.Instance.items[droppedItem.slot];
                Inventory.Instance.items[droppedItem.slot] = new InventoryObject();
                droppedItem.slot = id;
            } 
        }
        else
        {
            if (droppedItem.equipped)
            {
                Transform item = this.transform.GetChild(0);

                if (droppedItem.equipSlot == "Weapon Slot")
                {
                    if(item.GetComponent<ItemData>().weapon != null)
                    {
                        droppedItem.dropped = true;
                        droppedItem.equipped = false;
                        droppedItem.slot = id;

                        item.GetComponent<ItemData>().equipped = true;
                        item.transform.SetParent(StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot));
                        item.transform.position = StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot).position;
                        InventoryObject temp2 = Inventory.Instance.items[id];
                        Inventory.Instance.items[id] = StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot).GetComponent<EquipmentSlot>().temp;
                        StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot).GetComponent<EquipmentSlot>().temp = new InventoryObject();
                        StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot).GetComponent<EquipmentSlot>().temp = temp2;
                    }
                }
                if (droppedItem.equipSlot == "Armor Slot")
                {
                    droppedItem.dropped = true;
                    droppedItem.equipped = false;
                    droppedItem.slot = id;

                    if (item.GetComponent<ItemData>().armor != null)
                    {
                        item.GetComponent<ItemData>().equipped = true;
                        item.transform.SetParent(StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot));
                        item.transform.position = StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot).position;
                        InventoryObject temp2 = Inventory.Instance.items[id];
                        Inventory.Instance.items[id] = StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot).GetComponent<EquipmentSlot>().temp;
                        StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot).GetComponent<EquipmentSlot>().temp = new InventoryObject();
                        StatusMenu.Instance.transform.Find("Equipment Slot Panel " + StatusMenu.Instance.currPlayerUnit).Find(droppedItem.equipSlot).GetComponent<EquipmentSlot>().temp = temp2;
                    }
                }
               
            }
            else
            {
                InventoryObject temp = Inventory.Instance.items[id];
                Inventory.Instance.items[id] = Inventory.Instance.items[droppedItem.slot];
                Inventory.Instance.items[droppedItem.slot] = new InventoryObject();
                Inventory.Instance.items[droppedItem.slot] = temp;

                Transform item = this.transform.GetChild(0);
                item.GetComponent<ItemData>().slot = droppedItem.slot;
                item.transform.SetParent(Inventory.Instance.slots[droppedItem.slot].transform);
                item.transform.position = Inventory.Instance.slots[droppedItem.slot].transform.position;
                droppedItem.slot = id;
                droppedItem.transform.SetParent(this.transform);
                droppedItem.transform.position = this.transform.position;
            }
        }
    }
}
