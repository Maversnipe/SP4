using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    public int id;
    private Inventory inv;

    // Use this for initialization
    void Start () {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
	}

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

        if (inv.items[id].isEmpty)
        {
            inv.items[id] = inv.items[droppedItem.slot];
            inv.items[droppedItem.slot] = new InventoryObject();
            droppedItem.slot = id;
        }
        else
        {
            InventoryObject temp = inv.items[id];
            inv.items[id] = inv.items[droppedItem.slot];
            inv.items[droppedItem.slot] = new InventoryObject();
            inv.items[droppedItem.slot] = temp;

            Transform item = this.transform.GetChild(0);
            item.GetComponent<ItemData>().slot = droppedItem.slot;
            item.transform.SetParent(inv.slots[droppedItem.slot].transform);
            item.transform.position = inv.slots[droppedItem.slot].transform.position;
            droppedItem.slot = id;
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;
        }
    }
}
