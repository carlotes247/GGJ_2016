using UnityEngine;
using System.Collections;

public class PickUpRange : MonoBehaviour {

	SphereCollider coll;

	public float pickUpRange;

	void Start() {
		coll = GetComponent<SphereCollider> ();
	}

	void Update() {
		Cursor.visible = true; 

		coll.radius = pickUpRange;
	}
	
	void OnTriggerEnter (Collider Enter) {
		if (Enter.gameObject.layer == 8) { // ALl the objects that should be interactable should have a certain tag, put that tag in here. 
			Enter.GetComponent<PickUpObjects> ().playerInRange = true;
		}


		//if (Enter.gameObject.tag == "PickUp")  // ALl the objects that should be interactable should have a certain tag, put that tag in here. 
		//	Enter.GetComponent<PickUpObjects> ().playerInRange = true;
	}

	void OnTriggerStay (Collider Stay) {
		if (Stay.gameObject.layer == 8) { // ALl the objects that should be interactable should have a certain tag, put that tag in here. 
			Stay.GetComponent<PickUpObjects> ().playerInRange = true;
		}


		//if (Stay.gameObject.tag == "PickUp")
		//	Stay.GetComponent<PickUpObjects>().playerInRange = true;
	}

	void OnTriggerExit (Collider Exit) {
		if (Exit.gameObject.layer == 8) { // ALl the objects that should be interactable should have a certain tag, put that tag in here. 
			Exit.GetComponent<PickUpObjects> ().playerInRange = false;
		}


		//if (Exit.gameObject.tag == "PickUp")
		//	Exit.GetComponent<PickUpObjects>().playerInRange = false;
	}
}
