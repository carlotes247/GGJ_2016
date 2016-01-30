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

	public Light[] environmentLights;

	// Use this for initialization
	void Start () {
		initSSAOIntensity = theCamera.GetComponent<SSAOPro> ().Intensity;
		initSSAORadius = theCamera.GetComponent<SSAOPro> ().Radius;
		initVignetteIntensity = theCamera.GetComponent<FastVignette> ().Darkness;
		theCamera = GameObject.Find ("MainCamera");
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


		if (environmentLights.Length > 0) {
			foreach (Light light in environmentLights) {
				light.intensity = darknessVariable;
			}
		}
	}
}
