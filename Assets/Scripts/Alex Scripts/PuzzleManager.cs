using UnityEngine;
using System.Collections;

public class PuzzleManager : MonoBehaviour {

	public bool alarmPuzzle;
	public bool greenBookPuzzle;
	public bool blueBookPuzzle;
	public bool brownBookPuzzle;

	AudioSource audioSource;
	public AudioClip achievementSFX;

	public bool SFX;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (SFX == true) {
			AchievSFX ();
			SFX = false;
		}
	
		// ADD ALL THE OTHER PUZZLE BOOLS HERE, WHEN THEY ALL ARE TRUE HE WINS
		if (alarmPuzzle == true && greenBookPuzzle == true && blueBookPuzzle == true) {
			Debug.Log ("WIN");
		}
	}

	void AchievSFX() {
		audioSource.PlayOneShot (achievementSFX, 0.75f);
	}
}
