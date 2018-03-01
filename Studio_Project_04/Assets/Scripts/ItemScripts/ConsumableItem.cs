using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : MonoBehaviour {

	void Start () {
		
	}

    public void Use(int unit)
    {
        if(this.gameObject.GetComponent<ItemData>().item.Modifier == "HP")
        {
            StatusMenu.Instance.players[unit].GetComponent<UnitVariables>().HP += this.gameObject.GetComponent<ItemData>().item.ModifierValue;
            Inventory.Instance.RemoveItem(this.gameObject.GetComponent<ItemData>().item.ID, 1);
            Debug.Log("Item used");
            if (StatusMenu.Instance.players[unit].GetComponent<UnitVariables>().HP > StatusMenu.Instance.players[unit].GetComponent<UnitVariables>().startAP)
            {
                StatusMenu.Instance.players[unit].GetComponent<UnitVariables>().HP = StatusMenu.Instance.players[unit].GetComponent<UnitVariables>().startAP;
            }
        }
        
    }
}
