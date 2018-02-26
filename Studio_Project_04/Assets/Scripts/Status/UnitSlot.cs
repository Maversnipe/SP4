using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSlot : MonoBehaviour, IPointerClickHandler
{

    public int unitNo;

    // Use this for initialization
    void Start () {
        unitNo = 0;
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        StatusMenu.Instance.currPlayerUnit = unitNo;
    }

}
