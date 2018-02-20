using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenControl : MonoBehaviour {

	Camera CameraRef;

	Vector3 TargetPosition;

	[SerializeField]
	GameObject MovePoint;

	[SerializeField]
	GameObject Dungeon1;

	[SerializeField]
	GameObject Dungeon2;

	GameObject MovePointRef;

	Vector3 Dungeon1Pos;
	Vector3 Dungeon2Pos;

	// Use this for initialization
	void Start () {
		CameraRef = FindObjectOfType<Camera> ().GetComponent<Camera>();
		TargetPosition = this.transform.position;

		Dungeon1Pos = new Vector3 (Dungeon1.transform.position.x, this.transform.position.y, Dungeon1.transform.position.z);
		Dungeon2Pos = new Vector3 (Dungeon2.transform.position.x, this.transform.position.y, Dungeon2.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			ClickOnSpot ();
		}

		// Check collision of player and "Dungeons"
		if ((this.transform.position - Dungeon1Pos).magnitude < 3.5f) {
			//Debug.Log ("Reached Dungeon 1");
			SceneManager.LoadScene ("SceneBase");
		}
		if ((this.transform.position - Dungeon2Pos).magnitude < 3.5f) {
			Debug.Log ("Reached Dungeon 2");
		}

		if ((this.transform.position - TargetPosition).magnitude > 0.1f) {
			this.transform.position += (TargetPosition - this.transform.position).normalized * 0.05f;
		}

		if (MovePointRef != null) {
			if ((this.transform.position - MovePointRef.transform.position).magnitude < 0.1f) {
				Destroy (MovePointRef);
			}
		}
	}

	void ClickOnSpot() {
		Ray ray = CameraRef.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
		TargetPosition.x = this.transform.position.x + ray.direction.x * 40;
		TargetPosition.z = this.transform.position.z + ray.direction.z * 40;

		Quaternion Temp = Quaternion.Euler (90, 0, 0);
		if (MovePointRef != null) {
			MovePointRef.transform.position = TargetPosition;
		} else {
			MovePointRef = (Instantiate (MovePoint, TargetPosition, Temp));
		}

		//Debug.Log (ray.direction);
	}
}
