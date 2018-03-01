using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageScript : GenericSingleton<StorageScript>
{
    public List<InventoryObject> s_items = new List<InventoryObject>();
    public int s_gold;

	// Use this for initialization
	void Start () {
        s_gold = 500;
        for (int i = 0; i < 20; i++)
        {
            s_items.Add(new InventoryObject());
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
