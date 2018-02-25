using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class StatusMenu : GenericSingleton<StatusMenu>, IDragHandler
{

    private GameObject[] players;

    public int currPlayerUnit;

    GameObject equipmentPanel;

    GameObject unitName;

    public GameObject weapon;
    public GameObject armor;

    // Use this for initialization
    void Start()
    {
        currPlayerUnit = 0;

        players = GameObject.FindGameObjectsWithTag("PlayerUnit");
        equipmentPanel = GameObject.Find("Equipment Slot Panel " + currPlayerUnit);

        weapon = GameObject.Find("Weapon Slot");
        weapon.GetComponent<EquipmentSlot>().slotType = "Weapon";
        armor = GameObject.Find("Armor Slot");
        armor.GetComponent<EquipmentSlot>().slotType = "Armor";

    }

    void OnEnable()
    {
        for (int i = 0; i < players.Length; i++)
        {

        }
    }

    void Update()
    {
        this.transform.Find("Status Panel").GetChild(0).GetComponent<Text>().text = "\nHP : " + players[currPlayerUnit].GetComponent<UnitVariables>().HP.ToString() + "\nAP : " + players[currPlayerUnit].GetComponent<UnitVariables>().AP.ToString()
                                                                                    + "\nInitiative : " + players[currPlayerUnit].GetComponent<UnitVariables>().Initiative.ToString();

        this.transform.Find("Unit Name").GetChild(0).GetComponent<Text>().text = players[currPlayerUnit].name;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

}
