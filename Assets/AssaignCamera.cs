using UnityEngine;
using System.Collections;

public class AssaignCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("Darkness").GetComponent<Darkness> ().theCamera = gameObject;
	}

}
