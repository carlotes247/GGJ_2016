using UnityEngine;
using System.Collections;
using ReusableMethods;

/// <summary>
/// The controller in charge on handling the weapon's flashlight
/// </summary>
public class FlashLightController : MonoBehaviour {

    [SerializeField]
    private GameObject flashLight;
    /// <summary>
    /// The flashlight to control
    /// </summary>
    public GameObject FlashLight { get { return this.flashLight; } set { this.flashLight = value; } }

    ///// <summary>
    ///// The timer for the flashlight
    ///// </summary>
    //private TimerController timerController;

    ///// <summary>
    ///// The seconds to wait the countdown
    ///// </summary>
    //private float secondsToWait;

	// Use this for initialization
	void Start () {
        //timerController = new TimerController();
        //secondsToWait = Random.Range(0.5f, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
        // We update the position of the flashlight to the world position of the pointerPos
        FlashLight.transform.position = Camera.main.ScreenToWorldPoint(Toolbox.Instance.GameManager.InputController.ScreenPointerPos + (Vector3.forward));
        //Debug.Log("Updating Flashlight to " + Toolbox.Instance.GameManager.InputController.ScreenPointerPos.ToString());

        //// We check regularly if the flashlight needs to turn off
        //if (timerController.GenericCountDown(secondsToWait))
        //{
        //    if (Probabilities.GetResultProbability(0.1f))
        //    {
        //        FlashLight.SetActive(false);
        //    }
        //}
        //else
        //{
        //    FlashLight.SetActive(true);
        //}

    }
}
