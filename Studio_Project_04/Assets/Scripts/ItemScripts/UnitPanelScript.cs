using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitPanelScript : MonoBehaviour, IDragHandler
{

    public ItemData data;

    // Use this for initialization
    void Start () {
        this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 4; i++)
        {
            this.transform.Find("Unit " + i).GetChild(0).GetChild(0).GetComponent<Text>().text = "";
            this.transform.Find("Unit " + i).gameObject.SetActive(false);
        }
        for (int i = 0; i < StatusMenu.Instance.players.Count; i++)
        {
            this.transform.Find("Unit " + i).gameObject.SetActive(true);
            this.transform.Find("Unit " + i).GetChild(0).GetComponent<UnitSelectScript>().unit = i;
            this.transform.Find("Unit " + i).GetChild(0).GetChild(0).GetComponent<Text>().text = "\nHP : " + StatusMenu.Instance.players[i].GetComponent<UnitVariables>().HP.ToString() + " / " + StatusMenu.Instance.players[i].GetComponent<UnitVariables>().startAP.ToString() + "\nAP : " + StatusMenu.Instance.players[i].GetComponent<UnitVariables>().AP.ToString();
            this.transform.Find("Unit " + i).GetChild(0).GetComponent<UnitSelectScript>().data = data;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }
}

