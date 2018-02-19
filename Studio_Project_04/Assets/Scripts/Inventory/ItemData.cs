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

    private Inventory inv;
    private InfoPanel infoPanel;
    private Transform originalParent;
    private Vector2 offset;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        infoPanel = inv.GetComponent<InfoPanel>();
    }

    private void Update()
    {
        transform.GetChild(0).GetComponent<Text>().text = amount.ToString();
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
        this.transform.SetParent(inv.slots[slot].transform);
        this.transform.position = inv.slots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        infoPanel.Activate(this.gameObject.GetComponent<ItemData>());
    }
}
