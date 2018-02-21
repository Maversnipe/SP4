using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

	[TextArea]
	[SerializeField]
	private string[] Dialogue;

	private bool Interacted;

	private Camera CameraRef;

	private string TextToShow;

	private Rect windowRect = new Rect(150, 150, 200, 200);

	private bool Seen = false;

	// Use this for initialization
	void Start () {
		Interacted = false;
		CameraRef = FindObjectOfType<Camera> ();

		TextToShow = Dialogue[0];
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
