using UnityEngine;
using System.Collections;
using Colorful;

public class Darkness : MonoBehaviour {
	public float darknessVariable = 0;
	public float darknessSpeed;
	bool decreasing = true;
	public GameObject theCamera;

	public float maxSSAOIntensity;
	public float maxSSAORadius;
	float initSSAOIntensity;
	float initSSAORadius;
	public float maxVignetteIntensity;
	float initVignetteIntensity;

	public AudioClip[] ambientSounds;
	AudioSource audioSrc;

	public Light[] environmentLights;

	// Use this for initialization
	void Start () {
		initSSAOIntensity = theCamera.GetComponent<SSAOPro> ().Intensity;
		initSSAORadius = theCamera.GetComponent<SSAOPro> ().Radius;
		initVignetteIntensity = theCamera.GetComponent<FastVignette> ().Darkness;
		theCamera = GameObject.Find ("MainCamera");
		audioSrc = GetComponent <AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (decreasing && darknessVariable < 100) {
			print ("dcresin");
			darknessVariable += darknessSpeed / 10 * Time.deltaTime;
			theCamera.GetComponent<SSAOPro>().Intensity = initSSAOIntensity + maxSSAOIntensity * darknessVariable/100;
			theCamera.GetComponent<SSAOPro>().Radius = initSSAORadius + maxSSAORadius * darknessVariable/100;
			theCamera.GetComponent<FastVignette>().Darkness = initVignetteIntensity + maxVignetteIntensity * darknessVariable/100;
		}
		if (darknessVariable < 20 && audioSrc.clip != ambientSounds [0]) {
			audioSrc.clip = ambientSounds [0];
			audioSrc.Play ();
		} else if (darknessVariable > 20 && darknessVariable < 50 && audioSrc.clip != ambientSounds [1]) {
			audioSrc.clip = ambientSounds [1];
			audioSrc.Play ();
		} else if (darknessVariable > 50 && darknessVariable < 70 && audioSrc.clip != ambientSounds [2]) {
			audioSrc.clip = ambientSounds [2];
			audioSrc.Play ();
		} else if(darknessVariable > 70 && audioSrc.clip != ambientSounds [3]){
			audioSrc.clip = ambientSounds [3];
			audioSrc.Play ();
		}

		if (environmentLights.Length > 0) {
			foreach (Light light in environmentLights) {
				light.intensity = darknessVariable;
			}
		}
	}
	public void ResetDarkness(){
		darknessVariable = 0;
	}
}
