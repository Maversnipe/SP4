using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusScript : MonoBehaviour {

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

        players = GameObject.FindGameObjectsWithTag("PlayerUnit");
        equipmentPanel = GameObject.Find("Equipment Slot Panel");

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
}
