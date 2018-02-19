using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenControl : MonoBehaviour {

	Camera CameraRef;

	Vector3 TargetPosition;

	[SerializeField]
	GameObject Cursor;

	[SerializeField]
	GameObject MovePoint;

	GameObject MovePointRef;

	// Use this for initialization
	void Start () {
		CameraRef = FindObjectOfType<Camera> ().GetComponent<Camera>();
		TargetPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			ClickOnSpot ();
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

		Debug.Log (ray.direction);
	}
}
