using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellScript : MonoBehaviour, IDropHandler
{
    private GameObject sellConfirmDialog;

    // Use this for initialization
    void Start () {
        sellConfirmDialog = GameObject.FindGameObjectWithTag("SellConfirmDialog");
        sellConfirmDialog.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnDrop(PointerEventData eventData)
    {
        if(!eventData.pointerDrag.GetComponent<ItemData>().equipped)
        {
            ItemData soldItem = eventData.pointerDrag.GetComponent<ItemData>();
            sellConfirmDialog.SetActive(true);
            sellConfirmDialog.transform.GetChild(0).GetComponent<ButtonScript>().sellItemData = soldItem;
        }
    }
}
