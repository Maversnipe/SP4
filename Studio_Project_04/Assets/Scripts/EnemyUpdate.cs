using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUpdate : MonoBehaviour {

    // Serializable private variable defining Unit
    [SerializeField]
    Color HoverColor;

    // Reference to the unit's Components
    private Renderer rend;
    private Color DefaultColor;

    // Reference to the UnitManager's instance
    private UnitManager unitmanager;

    // Nodes
    public int nodeX;
    public int nodeZ;
    private Nodes currNode;
    private Nodes nextNode;
    // Use this for initialization
    void Start () {
        unitmanager = UnitManager.instance;
        currNode = GridSystem._instance.GetNode(nodeX, nodeZ);
        transform.position = new Vector3(currNode.transform.position.x, transform.position.y, currNode.transform.position.z);
    }

   void OnMouseDown()
    {
        //placeholder destroy script
        if(unitmanager.AbleToAttack && !unitmanager.AbleToChangeUnit)
        {
            Destroy(this.gameObject);
            unitmanager.AbleToAttack = false;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
