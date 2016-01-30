using UnityEngine;
using System.Collections;

public class PickUpObjects : MonoBehaviour {

	Rigidbody rb;
	PuzzleManager puzzleManager;
	AudioSource audioSource;

	public float objectPlacementValue;
	public float pickUpLength = 1f; // The Length away from the player the object is going to be when its picked up
	public float pickUpSpeed = 0.5f; // The Speed from its position to the players hand when it gets picked up

	bool objectPosUpdate = true; // When the object is not picked up, this makes sure objectPosition keeps track of the objects position before being picked up
	public bool objectInHand; // When the object is picked up
	public bool playerInRange; // If the object is within range, gets set to true by the PickUpRange script that should be on the player. The code in that script can be moved to another script on the player
	public bool notPlaced = true;

	public bool alarmInHand;
	public bool bookInHand;

	public Transform objectPosition; // The objects current position when not in hand

	// USE THIS IF YOU WANT MAKE THE OBJECT NOT ROTATE WHEN PICKED UPP
	//public Transform targetPosition; // Where the object is going to lerp to, which is the object Hand, under the Players>Camera

	Vector3 mousePosition; // Vector 3 for the camera

	void Start () {
		rb = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();
		puzzleManager = GameObject.FindGameObjectWithTag("PuzzleManager").GetComponent<PuzzleManager> ();
	
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

			if (this.gameObject.tag == "AlarmClock")
				alarmInHand = true;

			/*if (this.gameObject.tag == "Book")
				bookInHand = true;*/
			

			// USE THIS WITH WIIMOTE
			//transform.position = Vector3.Lerp (objectPosition.position, Toolbox.Instance.GameManager.InputController.ScreenPointerPos, pickUpSpeed);

			// USE THIS WITH MOUSE
			transform.position = Vector3.Lerp (objectPosition.position, mousePosition, pickUpSpeed);


			// THIS NEEDS TO BE CHANGED WITH THE WII MORE ROTATION INPUT
			//transform.rotation = targetPosition.rotation; // Setting the objects rotation to be the same as the player


			//THIS ROTATION IS BEST SUITED FOR MOUSE
			//transform.rotation = targetPosition.rotation; // Setting the objects rotation to be the same as the player
			// THIS IS BEST SUITED FOR 360 CONTOLLER
			/*transform.position = Vector3.Lerp (objectPosition.position, targetPosition.position, speed);  THE OBJECT GET LERPED TO THE CENTER OF THE SCREEN WITH THIS, NOT THE MOUSE/WII CURSOR POSITION. MAYBE THIS IS BEST IF WE HAVE 360 CONTOLLER
																											When the player click on the object, it lerps from the position it was, to the child object Hand*/
		}
	}

	public void OnMouseDown() {
		PickUpLogic ();

		// OLD, CAN BE USED WITH MOUSE
		/*if (playerInRange == true && notPlaced == true) // If the player is within range and the object has not been placed. 
			objectInHand = true;*/
	}

	public void PickUpLogic() {
		if (objectInHand == false) {
			if (playerInRange == true && notPlaced == true) // If the player is within range and the object has not been placed. 
			objectInHand = true;
		} else {
			objectPosUpdate = true;
			objectInHand = false; 
			rb.useGravity = true; // If you drop the object, the gravity is activated again. 
		}
	}

	public void OnMouseUp() {
		PickUpLogic ();

		// OLD, CAN BE USED WITH MOUSE
		/*objectPosUpdate = true;
		objectInHand = false; 
		rb.useGravity = true; // If you drop the object, the gravity is activated again. */

	}

	void OnTriggerEnter(Collider Enter) {
		if (Enter.gameObject.tag == "AlarmClockPlacement" && alarmInHand == true) { // If the alarm is in hand and it collides with the AlarmClockPlacement object
			notPlaced = false; // Object has been placed
			objectInHand = false; // The object is no longer in hand
			alarmInHand = false; // The AlarmClock is not in hand
			rb.useGravity = false; // // Turns off gravity
			rb.isKinematic = true; // Makes it kinematic so it stays there
			audioSource.enabled = false; // Turns off its sound FX
			puzzleManager.alarmPuzzle = true; // Tells the puzzle manager that the alarm puzzle is finished

			// Transform the object to where it supposed to be placed, minus the Y value so its correct. The Y value can be set in the inspector for each object
			objectPosition.position = new Vector3 (Enter.gameObject.transform.position.x, Enter.gameObject.transform.position.y - objectPlacementValue, Enter.gameObject.transform.position.z);
			transform.rotation = Enter.gameObject.transform.rotation; // Sets the rotation to the placement gameobjects rotation

			Debug.Log ("Alarm Puzzel CLear.");
		}
	}
}
