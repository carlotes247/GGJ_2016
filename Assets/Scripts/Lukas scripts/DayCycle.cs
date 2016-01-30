using UnityEngine;
using System.Collections;

public class DayCycle : MonoBehaviour {

	public float daySpeed;
	public GameObject sun;
	public Vector3 rotationDirection;
	public float sunriseAngleLightIntensifier;
	public float sunriseIntensitySpeed;
	Light sunLight;

	float initialLightIntensity;

	//	public float lightIntensity; 			darkness intensifier
	//	public Light[] environmentLights;

	void Awake(){
		sunLight = sun.GetComponent<Light> ();
		initialLightIntensity = sunLight.intensity;
	}
	// Update is called once per frame
	void Update () {
		sun.transform.Rotate (rotationDirection.normalized * daySpeed/100);
		//		if (environmentLights.Length > 0) {
		//			foreach (Light light in environmentLights) {
		//				light.intensity = lightIntensity;
		//			}
		//		}
		if(sun.transform.rotation.eulerAngles.x < sunriseAngleLightIntensifier){
			sunLight.intensity = Mathf.Lerp (sunLight.intensity, 1, Time.deltaTime * sunriseIntensitySpeed/10);
		}
		if(sun.transform.rotation.eulerAngles.x >= 180){
			print("DOWN");
			//Night, start the next day
			NextDay();
		}
	}
	public void NextDay(){
		
	}
}
