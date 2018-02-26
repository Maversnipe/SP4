using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    bool freecam = false;
    bool isPanning = false;
    int cameraMode = 0;
    Vector3 anchorPos;
    private TurnManager turnManager;
	private PlayerManager playerManager;

	private GameObject currFocus;

    // Use this for initialization
    void Start () {
		turnManager = TurnManager.Instance;
		playerManager = PlayerManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {

		if (turnManager.IsPlayerTurn()) {

			if (Input.GetKey("w"))
			{
				transform.position += transform.up * 10 * Time.deltaTime;
			}
			if (Input.GetKey("a"))
			{
				transform.position -= transform.right * 10 * Time.deltaTime;
			}
			if (Input.GetKey("s"))
			{
				transform.position -= transform.up * 10 * Time.deltaTime;
			}
			if (Input.GetKey("d"))
			{
				transform.position += transform.right * 10 * Time.deltaTime;
			}

			if (playerManager.GetSelectedUnit () != null) {
				currFocus = playerManager.GetSelectedUnit ().gameObject;
			} else {
				currFocus = null;
			}
		} else {
			if (turnManager.GetCurrUnit () != null) {
				currFocus = turnManager.GetCurrUnit ().gameObject;
			}
		}

		if (currFocus != null) {
			Vector3 TempPos = this.transform.position;

			TempPos.x = currFocus.transform.position.x;
			TempPos.z = currFocus.transform.position.z;
			this.transform.position = TempPos;
		}

        if (Input.GetKeyDown("b"))
        {
            cameraMode = 0;
        }
        if (Input.GetKeyDown("n"))
        {
            cameraMode = 1;
        }

        if (!freecam)
        {
            if (Input.GetKeyDown("c"))
            {
                freecam = true;
            }

        }
        if (freecam)
        {
            if (Input.GetKeyDown("v"))
            {
                freecam = false;
				transform.position = new Vector3(turnManager.GetCurrUnit ().transform.position.x, transform.position.y, turnManager.GetCurrUnit ().transform.position.z);
            }

            if (cameraMode == 0)
            {
                if (Input.GetKey("w"))
                {
                    transform.position += transform.up * 10 * Time.deltaTime;
                }
                if (Input.GetKey("a"))
                {
                    transform.position -= transform.right * 10 * Time.deltaTime;
                }
                if (Input.GetKey("s"))
                {
                    transform.position -= transform.up * 10 * Time.deltaTime;
                }
                if (Input.GetKey("d"))
                {
                    transform.position += transform.right * 10 * Time.deltaTime;
                }
            }

            if(cameraMode == 1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isPanning = true;
                    anchorPos = Input.mousePosition;

                }

                if(!Input.GetMouseButton(0))
                {
                    isPanning = false;
                }

                if(isPanning)
                {
                    Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - anchorPos);

                    Vector3 move = new Vector3(pos.x * 2, pos.y * 2, 0);
                    transform.Translate(move, Space.Self);
                }
  
            }

        }
        
    }
}
