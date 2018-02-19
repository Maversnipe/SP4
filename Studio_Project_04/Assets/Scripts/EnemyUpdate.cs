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

    // Reference to the PlayerManager's instance
	private PlayerManager playerManager;

    // Nodes
    public int nodeX;
    public int nodeZ;
    private Nodes currNode;
    private Nodes nextNode;
    // Use this for initialization
    void Start () {
		playerManager = PlayerManager.Instance;
        currNode = GridSystem.Instance.GetNode(nodeX, nodeZ);
        transform.position = new Vector3(currNode.transform.position.x, transform.position.y, currNode.transform.position.z);
    }

   void OnMouseDown()
    {
//        //placeholder destroy script
//		if(playerManager.GetAbleToAttack() && !playerManager.GetAbleToChangeUnit())
//        {
//            Destroy(this.gameObject);
//            playerManager.SetAbleToAttack(false);
//        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
