using UnityEngine;
using System.Collections;

public class PuzzleManager : MonoBehaviour {

	public bool alarmPuzzle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		// ADD ALL THE OTHER PUZZLE BOOLS HERE, WHEN THEY ALL ARE TRUE HE WINS
		if (alarmPuzzle == true) {
			Debug.Log ("WIN");
		}
	}
}
