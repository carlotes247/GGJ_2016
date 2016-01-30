using UnityEngine;
using System.Collections;

public class PickUpObjects : MonoBehaviour {

	Rigidbody rb;
	//BoxCollider coll;

	public float pickUpLength; // The Length away from the player the object is going to be when its picked up
	public float pickUpSpeed; // The Speed from its position to the players hand when it gets picked up

	bool objectPosUpdate = true; // When the object is not picked up, this makes sure objectPosition keeps track of the objects position before being picked up
	public bool objectInHand; // When the object is picked up
	public bool playerInRange; // If the object is within range, gets set to true by the PickUpRange script that should be on the player. The code in that script can be moved to another script on the player

	public Transform objectPosition; // The objects current position when not in hand
	public Transform targetPosition; // Where the object is going to lerp to, which is the object Hand, under the Players>Camera

	Vector3 mousePosition; // Vector 3 for the camera

	void Start () {
		rb = GetComponent<Rigidbody> ();
		//coll = GetComponent<BoxCollider> ();
	}

	void Update() {
		// USE THIS WITH WIIMOTE
		//mousePosition = new Vector3(Toolbox.Instance.GameManager.InputController.ScreenPointerPos.x, Toolbox.Instance.GameManager.InputController.ScreenPointerPos.y, pickUpLength); // Śets the mousePosition vector 3 to the mouse input with pickUpLength as its Z value, the distance from the camera

		// USE THIS WITH MOUSE
		mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, pickUpLength); // Śets the mousePosition vector 3 to the mouse input with pickUpLength as its Z value, the distance from the camera
		mousePosition = Camera.main.ScreenToWorldPoint (mousePosition); 
	}
		
	void FixedUpdate () {
		
		if (objectPosUpdate == true) // If the object is not in hand its storing the objects transform.
			objectPosition = transform;

		if (objectInHand == true) {
			objectPosUpdate = false; // If the object is picked up, turns off the bool so the 
			rb.useGravity = false; // If the object is picked up, its gravity gets turned off

			// USE THIS WITH WIIMOTE
			//transform.position = Vector3.Lerp (objectPosition.position, Toolbox.Instance.GameManager.InputController.ScreenPointerPos, pickUpSpeed);

			// USE THIS WITH MOUSE
			transform.position = Vector3.Lerp (objectPosition.position, mousePosition, pickUpSpeed);


			// IF YOUR GOING TO BE ABLE TO ROTATE THE OBJECT WITH THE WII REMOVE, COMMENTED!
			transform.rotation = targetPosition.rotation; // Setting the objects rotation to be the same as the player

			// BACK UP
			//transform.rotation = targetPosition.rotation; // Setting the objects rotation to be the same as the player

			// THIS IS BEST SUITED FOR 360 CONTOLLER
			/*transform.position = Vector3.Lerp (objectPosition.position, targetPosition.position, speed);  THE OBJECT GET LERPED TO THE CENTER OF THE SCREEN WITH THIS, NOT THE MOUSE/WII CURSOR POSITION. MAYBE THIS IS BEST IF WE HAVE 360 CONTOLLER
																											When the player click on the object, it lerps from the position it was, to the child object Hand*/

			//coll.enabled = false;
		}
	}

	void OnMouseDown() {
		if (playerInRange == true)
			objectInHand = true;
	}

	void OnMouseUp() {
		objectPosUpdate = true;
		objectInHand = false; 
		rb.useGravity = true; // If you drop the object, the gravity is activated again. 

		//coll.enabled = true;
	}
}
