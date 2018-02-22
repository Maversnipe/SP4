using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellScript : MonoBehaviour, IDropHandler
{
    private GameObject confirmDialog;

    // Use this for initialization
    void Start () {
        confirmDialog = GameObject.Find("ConfirmDialog");
        confirmDialog.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnDrop(PointerEventData eventData)
    {
        ItemData soldItem = eventData.pointerDrag.GetComponent<ItemData>();
        confirmDialog.SetActive(true);
        confirmDialog.transform.GetChild(0).GetComponent<ButtonScript>().sellItemData = soldItem;

    }
}
