using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UnitSelectScript : EventTrigger {

    public int unit;
    public ItemData data;

	// Use this for initialization
	void Start () {
		
	}

    public override void OnPointerEnter(PointerEventData eventData)
    {
        var tempColor = this.GetComponent<Image>().color;
        tempColor.r = 192f;
        tempColor.g = 234f;
        tempColor.b = 255f;
        this.gameObject.GetComponent<Image>().color = tempColor;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        var tempColor = this.GetComponent<Image>().color;
        tempColor.r = 255f;
        tempColor.g = 255f;
        tempColor.b = 255f;
        this.GetComponent<Image>().color = tempColor;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Inventory.Instance.slots[data.slot].transform.GetChild(0).GetComponent<ConsumableItem>().Use(unit);
        this.transform.parent.parent.gameObject.SetActive(false);
    }
}
