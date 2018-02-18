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
	private TurnManager turnManager;

    // Nodes
    public int nodeX;
    public int nodeZ;
    private Nodes currNode;
    private Nodes nextNode;
    // Use this for initialization
    void Start () {
		turnManager = TurnManager.Instance;
        currNode = GridSystem.Instance.GetNode(nodeX, nodeZ);
        transform.position = new Vector3(currNode.transform.position.x, transform.position.y, currNode.transform.position.z);
    }

   void OnMouseDown()
    {
        //placeholder destroy script
		if(turnManager.GetAbleToAttack() && !turnManager.GetAbleToChangeUnit())
        {
            Destroy(this.gameObject);
            turnManager.SetAbleToAttack(false);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
