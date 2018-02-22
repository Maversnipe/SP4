using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{


    public Item item;
    public Weapon weapon;
    public Armor armor;

    public int amount;

    public int slot;

    private Shop shop;
    private ShopPanel infoPanel;
    private Transform originalParent;
    private Vector2 offset;

    void Start()
    {
        shop = GameObject.Find("Shop").GetComponent<Shop>();
        infoPanel = shop.GetComponent<ShopPanel>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null || weapon != null || armor != null)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            originalParent = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
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
        this.transform.SetParent(Inventory.Instance.slots[slot].transform);
        this.transform.position = Inventory.Instance.slots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        infoPanel.Activate(this.gameObject.GetComponent<ShopItemData>());
    }
}
