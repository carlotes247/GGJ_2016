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

	public GameObject ocdRenderer;

	public Light[] environmentLights;

	// Use this for initialization
	public void InitiateCamera () {
//		theCamera = GameObject.Find ("MainCamera");
		theCamera = Camera.main.gameObject;
		initSSAOIntensity = theCamera.GetComponent<SSAOPro> ().Intensity;
		initSSAORadius = theCamera.GetComponent<SSAOPro> ().Radius;
		initVignetteIntensity = theCamera.GetComponent<FastVignette> ().Darkness;
		audioSrc = GetComponent <AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.O)) {
			ResetDarkness ();
		}
		if (decreasing && darknessVariable < 100) {
			darknessVariable += darknessSpeed / 10 * Time.deltaTime;
			theCamera.GetComponent<FastVignette>().Darkness = initVignetteIntensity + maxVignetteIntensity * darknessVariable/100;
		}
		if (darknessVariable > 20) {
			theCamera.GetComponent<SSAOPro> ().Intensity = initSSAOIntensity + maxSSAOIntensity * darknessVariable / 100;
			theCamera.GetComponent<SSAOPro> ().Radius = initSSAORadius + maxSSAORadius * darknessVariable / 100;
		}
		ProceduralMaterial substance = ocdRenderer.GetComponent<Renderer> ().sharedMaterial as ProceduralMaterial;
//		ocdRenderer.GetComponent<Renderer>().material.SetFloat ("OCD_mask", 1f);

		//Audio switching
		if (darknessVariable < 20 && audioSrc.clip != ambientSounds [0]) {
			substance.SetProceduralFloat("OCD_mask", 0.1f);
			substance.RebuildTextures ();
			audioSrc.clip = ambientSounds [0];
			audioSrc.Play ();
		} else if (darknessVariable > 20 && darknessVariable < 50 && audioSrc.clip != ambientSounds [1]) {
			audioSrc.clip = ambientSounds [1];
			audioSrc.Play ();
		} else if (darknessVariable > 50 && darknessVariable < 70 && audioSrc.clip != ambientSounds [2]) {
			audioSrc.clip = ambientSounds [2];
			audioSrc.Play ();
//			ocdMat.SetFloat ("OCD_Mask", 1f);
		} else if(darknessVariable > 70 && audioSrc.clip != ambientSounds [3]){
			audioSrc.clip = ambientSounds [3];
			theCamera.GetComponent<Wiggle> ().enabled = true;
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
		theCamera.GetComponent<SSAOPro> ().Intensity = initSSAOIntensity;
		theCamera.GetComponent<SSAOPro> ().Radius = initSSAORadius;
		theCamera.GetComponent<FastVignette> ().Darkness = initVignetteIntensity;
	}
}
