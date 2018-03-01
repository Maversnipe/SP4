using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemData : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler{

    public Item item;
    public Weapon weapon;
    public Armor armor;

    public int amount;

    public int slot;

    public bool equipped;
    public string equipSlot;
    public bool dropped;
    private GameObject shopOverlay;

    private GameObject DragItemHolder;
    private InfoPanel infoPanel;
    private EquipmentInfoPanel equipmentInfoPanel;
    public Transform originalParent;
    private Vector2 offset;

    void Awake()
    {

        item = null;
        weapon = null;
        armor = null;
        equipped = false;
        infoPanel = Inventory.Instance.GetComponent<InfoPanel>();
        equipmentInfoPanel = StatusMenu.Instance.GetComponent<EquipmentInfoPanel>();
        shopOverlay = GameObject.Find("Shop Overlay");
        DragItemHolder = GameObject.Find("DragItemHolder");
    }

    private void Update()
    {
        shopOverlay = GameObject.Find("Shop Overlay");
        transform.GetChild(0).GetComponent<Text>().text = amount.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null || weapon != null || armor != null)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            originalParent = this.transform.parent;
            this.transform.SetParent(DragItemHolder.transform);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            shopOverlay.GetComponent<Image>().raycastTarget = true;
            dropped = false;
            infoPanel.Deactivate();
            equipmentInfoPanel.Deactivate();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null || weapon != null || armor != null)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!equipped)
        {
            this.transform.SetParent(Inventory.Instance.slots[slot].transform);
            this.transform.position = Inventory.Instance.slots[slot].transform.position;   
        }
        else
        {
            if(dropped)
            {
                this.transform.SetParent(Inventory.Instance.slots[slot].transform);
                this.transform.position = Inventory.Instance.slots[slot].transform.position;
            }
            else
            {
                this.transform.SetParent(originalParent);
                this.transform.position = this.transform.parent.position;
            }
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        shopOverlay.GetComponent<Image>().raycastTarget = false;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!equipped)
        {
            infoPanel.Activate(this.gameObject.GetComponent<ItemData>());
        }
        else
        {
            equipmentInfoPanel.Activate(this.gameObject.GetComponent<ItemData>());
        }
    }
}
