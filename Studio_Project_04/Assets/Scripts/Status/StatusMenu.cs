using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class StatusMenu : GenericSingleton<StatusMenu>, IDragHandler
{

    public List<UnitVariables> players;

    public int currPlayerUnit;

    GameObject equipmentPanel;

    GameObject unitName;

    public GameObject weapon;
    public GameObject armor;

    // Use this for initialization
    void Start()
    {
        currPlayerUnit = 0;

        players = PlayerUnitVariables.Instance.ListOfUnitVariables;
        equipmentPanel = GameObject.Find("Equipment Slot Panel " + currPlayerUnit);

        for (int i = 0; i < 4; i++)
        {
            this.transform.Find("Equipment Slot Panel " + i).Find("Weapon Slot").GetComponent<EquipmentSlot>().slotType = "Weapon";
            this.transform.Find("Equipment Slot Panel " + i).Find("Armor Slot").GetComponent<EquipmentSlot>().slotType = "Armor";
        }
    }
    void Update()
    {
        players = PlayerUnitVariables.Instance.ListOfUnitVariables;
        for (int i = 0; i < players.Count; i++)
        {
            this.transform.Find("Unit Panel").Find("Unit Slot " + i).GetComponent<UnitSlot>().unitNo = i;
        }

        for (int i = 0; i < 4; i++)
        {
            this.transform.Find("Equipment Slot Panel " + i).gameObject.SetActive(false);
        }

        this.transform.Find("Equipment Slot Panel " + currPlayerUnit).gameObject.SetActive(true);
        this.transform.Find("Equipment Slot Panel " + currPlayerUnit).SetAsLastSibling();

        this.transform.Find("Status Panel").GetChild(0).GetComponent<Text>().text = "\nHP : " + players[currPlayerUnit].HP.ToString() + "\nAP : " + players[currPlayerUnit].GetComponent<UnitVariables>().AP.ToString()
                                                                                    + "\nInitiative : " + players[currPlayerUnit].Initiative.ToString();

        this.transform.Find("Unit Name").GetChild(0).GetComponent<Text>().text = players[currPlayerUnit].name;

        if(this.transform.Find("Equipment Slot Panel " + currPlayerUnit).Find("Weapon Slot").childCount > 0)
        {
            players[currPlayerUnit]._weapon = this.transform.Find("Equipment Slot Panel " + currPlayerUnit).Find("Weapon Slot").GetChild(0).GetComponent<ItemData>().weapon;
        }
        else
        {
            players[currPlayerUnit]._weapon = null;
        }
        if (this.transform.Find("Equipment Slot Panel " + currPlayerUnit).Find("Armor Slot").childCount > 0)
        {
            players[currPlayerUnit]._armor = this.transform.Find("Equipment Slot Panel " + currPlayerUnit).Find("Armor Slot").GetChild(0).GetComponent<ItemData>().armor;
        }
        else
        {
            players[currPlayerUnit]._armor = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

}
