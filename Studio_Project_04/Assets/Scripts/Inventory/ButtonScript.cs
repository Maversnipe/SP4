using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    private GameObject inv;

    public ItemData itemData;

	// Use this for initialization
	void Start () {
        inv = GameObject.Find("Inventory");
    }
	
    public void closePanel()
    {
        this.transform.parent.gameObject.SetActive(false);
    }

    public void useItem()
    {
        inv.GetComponent<Inventory>().slots[itemData.slot].transform.GetChild(0).GetComponent<ConsumableItem>().Use();
        itemData.amount--;
    }
}
