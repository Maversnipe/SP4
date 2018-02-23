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
    public GameObject offhand;
    public GameObject head;
    public GameObject body;
    public GameObject arms;
    public GameObject legs;
    public GameObject feet;
    public GameObject accessory;

    // Use this for initialization
    void Start()
    {
        currPlayerUnit = 0;

        players = GameObject.FindGameObjectsWithTag("PlayerUnit");
        equipmentPanel = GameObject.Find("Equipment Slot Panel " + currPlayerUnit);

        weapon = GameObject.Find("Weapon Slot");
        weapon.GetComponent<EquipmentSlot>().slotType = "Weapon";
        offhand = GameObject.Find("Offhand Slot");
        offhand.GetComponent<EquipmentSlot>().slotType = "Offhand";
        head = GameObject.Find("Head Slot");
        head.GetComponent<EquipmentSlot>().slotType = "Head";
        body = GameObject.Find("Body Slot");
        body.GetComponent<EquipmentSlot>().slotType = "Body";
        arms = GameObject.Find("Arms Slot");
        arms.GetComponent<EquipmentSlot>().slotType = "Arms";
        legs = GameObject.Find("Legs Slot");
        legs.GetComponent<EquipmentSlot>().slotType = "Legs";
        feet = GameObject.Find("Feet Slot");
        feet.GetComponent<EquipmentSlot>().slotType = "Feet";
        accessory = GameObject.Find("Accessory Slot");
        accessory.GetComponent<EquipmentSlot>().slotType = "Accessory";

    }

    void Update()
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

}
