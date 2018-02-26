using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenControl : MonoBehaviour {

	Camera CameraRef;

	Vector3 TargetPosition;

	[SerializeField]
	float Speed;

	[SerializeField]
	GameObject MovePoint;

	GameObject MovePointRef;

	bool CanMove;

	// Use this for initialization
	void Start () {
		CameraRef = FindObjectOfType<Camera> ().GetComponent<Camera>();
		TargetPosition = this.transform.position;

		CanMove = true;
	}
	
	// Update is called once per frame
	void Update () {
		// Moves the unit towards the TargetPosition
		if ((this.transform.position - TargetPosition).magnitude > 0.1f) {
			this.transform.position += (TargetPosition - this.transform.position).normalized * Speed;
		}

		// Mouse Control
		if (CanMove && Input.GetMouseButtonDown (0)) {
			ClickOnSpot ();
		}

		// Removes the MovePointRef object IF it has not reached it's alpha limit and the unit has reached the targetposition
		if (MovePointRef != null) {
			if ((this.transform.position - MovePointRef.transform.position).magnitude < 0.1f) {
				Destroy (MovePointRef);
			}
		}
	}

	void ClickOnSpot() {
		RaycastHit collide;
		Ray ray = CameraRef.ScreenPointToRay(Input.mousePosition);

		// Check if mouseclick has collided with any buildings
		if (Physics.Raycast (ray, out collide)) {

			if (collide.collider.tag == "Player") {
				return;
			}

			if (collide.collider.tag == "Building") {
				TargetPosition = collide.transform.GetChild (0).transform.position;
			} else if (collide.collider.tag == "NPC") {
				TargetPosition = collide.transform.position;
				collide.collider.GetComponent<NPC> ().SetMoving (false);
			} else {
				TargetPosition.x = this.transform.position.x + ray.direction.x * CameraRef.transform.position.y;
				TargetPosition.z = this.transform.position.z + ray.direction.z * CameraRef.transform.position.y;
			}
		}

		Quaternion Temp = Quaternion.Euler (90, 0, 0);
		if (MovePointRef != null) {
			Destroy (MovePointRef);
		}

		MovePointRef = (Instantiate (MovePoint, TargetPosition, Temp));
	}

	// Instantly stops the player from moving but does not prevent the player from moving further
	public void StopMoving() {
		TargetPosition = this.transform.position;
	}

	// Gets the target position
	public Vector3 getTarget() {
		return TargetPosition;
	}

	// To Prevent player from moving altogether
	public void setAbleToMove(bool Setter)
	{
		CanMove = Setter;
	}
}
