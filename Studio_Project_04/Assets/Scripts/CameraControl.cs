using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    bool freecam = false;
	float Timer;

	//References to Player & Turn Manager
    private TurnManager turnManager;
	private PlayerManager playerManager;

	//Variable to move the camera towards the game object
	private GameObject currFocus = null;

    // Use this for initialization
    void Start () {
		turnManager = TurnManager.Instance;
		playerManager = PlayerManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {

		Timer -= Time.deltaTime;

		// If it is currently the player's turn
		if (turnManager.IsPlayerTurn ()) {
			if (currFocus != null) {
				Vector3 TempPos = currFocus.transform.position;

				TempPos.y = this.transform.position.y;

				// Regular WASD Movement
				if (Input.GetKey("w"))
				{
					transform.position += transform.up * 10 * Time.deltaTime;
					Timer = 1;
				}
				if (Input.GetKey("a"))
				{
					transform.position -= transform.right * 10 * Time.deltaTime;
					Timer = 1;
				}
				if (Input.GetKey("s"))
				{
					transform.position -= transform.up * 10 * Time.deltaTime;
					Timer = 1;
				}
				if (Input.GetKey("d"))
				{
					transform.position += transform.right * 10 * Time.deltaTime;
					Timer = 1;
				}

				// If the current Focused target isn't the same as the player manager one
				if (playerManager.GetSelectedUnit() != null &&
					currFocus != playerManager.GetSelectedUnit ().gameObject) {
					currFocus = playerManager.GetSelectedUnit ().gameObject;
				}

				if (Timer <= 0) {
					if ((this.transform.position - TempPos).magnitude > 5) {
						if ((this.transform.position - TempPos).magnitude > 0.5f) {
							this.transform.position += (TempPos - this.transform.position).normalized * 0.5f;
						}
					} else {
						if ((this.transform.position - TempPos).magnitude > 0.1f) {
							this.transform.position += (TempPos - this.transform.position).normalized * 0.1f;
						}
					}
				}
			} else {
				if (playerManager.GetSelectedUnit () != null) {
					currFocus = playerManager.GetSelectedUnit ().gameObject;
				}
			}
		} else {
			if (currFocus != null) {
				Vector3 TempPos = currFocus.transform.position;

				TempPos.y = this.transform.position.y;

				// If the current Focused target isn't the same as the turn manager one
				if (turnManager.GetCurrUnit () != null &&
					currFocus != turnManager.GetCurrUnit ().gameObject) {
					currFocus = turnManager.GetCurrUnit ().gameObject;
				}

				if ((this.transform.position - TempPos).magnitude > 5) {
					if ((this.transform.position - TempPos).magnitude > 0.5f) {
						this.transform.position += (TempPos - this.transform.position).normalized * 0.5f;
					}
				} else {
					if ((this.transform.position - TempPos).magnitude > 0.1f) {
						this.transform.position += (TempPos - this.transform.position).normalized * 0.1f;
					}
				}
			} else {
				if (turnManager.GetCurrUnit () != null) {
					currFocus = turnManager.GetCurrUnit ().gameObject;
				}
			}
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
        
    }

	public void setFocus(GameObject new_Focus)
	{
		currFocus = new_Focus;
	}

	public GameObject getFocus()
	{
		return currFocus;
	}
}
