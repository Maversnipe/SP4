using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

	// All the variables needed to allow NPC interaction
	[TextArea]
	[SerializeField]
	private string[] Dialogue = null;

	private bool Interacted;

	private Camera CameraRef;

	private string TextToShow;

	private Rect windowRect = new Rect(150, 150, 200, 200);

	private bool Seen = false;

	// All the variables needed to allow basic patrolling Movement
	[SerializeField]
	private GameObject[] Waypoints = null;

	private Vector3 TargetMovement;

	[SerializeField]
	private float Speed;

	private bool isMoving;

	// Use this for initialization
	void Start () {
		Interacted = false;
		CameraRef = FindObjectOfType<Camera> ();

		TextToShow = Dialogue[0];

		if (Waypoints.Length != 0) {
			TargetMovement = Waypoints [0].transform.position;
		}

		isMoving = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Waypoints == null || isMoving == false) {
			if (Interacted == false) {
				if (FindObjectOfType<OpenControl> ().getTarget() != this.gameObject.transform.position) {
					isMoving = true;
				}
			}
			return;
		}

		if ((this.transform.position - TargetMovement).magnitude < 0.1f) {
			for (int i = 0; i < Waypoints.Length; ++i) {
				if (TargetMovement == Waypoints [i].transform.position) {
					if (i == Waypoints.Length - 1) {
						TargetMovement = Waypoints [0].transform.position;
					} else {
						int temp = i + 1;
						TargetMovement = Waypoints [temp].transform.position;
					}
					return;
				}
			}
		}

		this.transform.position += (TargetMovement - this.transform.position).normalized * Speed;
	}

	void OnGUI() {
		GUI.skin.window.wordWrap = true;
		GUI.skin.window.alignment = TextAnchor.UpperLeft;
		GUI.changed = false;

		if (Interacted) {
			windowRect = GUI.Window (0, windowRect, DoMyWindow, TextToShow);
			windowRect.width = Screen.width * 0.8f;
			windowRect.height = TextToShow.Length * 50;
			windowRect.height = Screen.height * 0.3f;
			windowRect.x = (int)(Screen.width * 0.5f - windowRect.width * 0.5f);
			windowRect.y = (int)(Screen.height * 0.9f - windowRect.height * 0.9f);

			GUI.Window (0, windowRect, DoMyWindow, TextToShow);
		}
	}

	void DoMyWindow(int windowID){
		GUI.skin.label.fontSize = 10;

		GUI.Label (new Rect ( windowRect.width - 150
							, windowRect.height * 0.6f + 10
							, 140
							, 20), "Press Spacebar to Continue");
		if (Input.GetKeyDown(KeyCode.Space) && !Seen) {
			for (int i = 0; i < Dialogue.Length; i++) {
				// Everything before the current text index is not needed
				if (Dialogue [i].Equals(TextToShow)) {
					// Checks if this is the last bit of text before restoring functionality
					if (i == Dialogue.Length - 1) {
						Interacted = false;
						FindObjectOfType<OpenControl> ().setAbleToMove (true);
						TextToShow = Dialogue [0];
						isMoving = true;
					} else { // Seen is needed to ensure the code doesn't blitz past to the end
						int temp = i + 1;
						TextToShow = Dialogue [temp];
						Seen = true;
					}
					return;
				}
			}
		}

		if (Input.GetKeyUp (KeyCode.Space) && Seen) {
			Seen = false;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.name == "Player") {
			if (other.GetComponent<OpenControl> ().getTarget () == this.gameObject.transform.position) {
				other.GetComponent<OpenControl> ().StopMoving ();
				other.GetComponent<OpenControl> ().setAbleToMove (false);
				Interacted = true;
			}
		}
	}

	public void SetMoving(bool n_new)
	{
		isMoving = n_new;
	}
}
