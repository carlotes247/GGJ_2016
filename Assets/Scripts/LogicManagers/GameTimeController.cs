using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the timeScale of the game
/// </summary>
public class GameTimeController : MonoBehaviour {

    [SerializeField]
    private float currentTimeScale;
    /// <summary>
    /// (Property) The current time scale
    /// </summary>
    public float CurrentTimeScale { get { return this.currentTimeScale; } set { this.currentTimeScale = value; } }

    [SerializeField]
    private float previousTimeScale;
    /// <summary>
    /// (Property) The timeScale previous to a change
    /// </summary>
    public float PreviousTimeScale { get { return this.previousTimeScale; } set { this.previousTimeScale = value; } }
	
    // The start function, executes when the component is loaded and started
    void Start()
    {
        // We load the initial timeScale into CurrentTimeScale
        SetTimeScale(Time.timeScale);
    }

    /// <summary>
    /// Sets the timeScale to the value passed in
    /// </summary>
    /// <param name="timeToSet"> The time to set </param>
    public void SetTimeScale (float timeToSet)
    {
        // We check that the timeToSet is not the same as the current one
        if (timeToSet != CurrentTimeScale)
        {
            // We save the current timeScale as the previous one
            PreviousTimeScale = CurrentTimeScale;
            // We save the timeToSet into currentTimeScale
            CurrentTimeScale = timeToSet;
            // We set the timeScale
            Time.timeScale = CurrentTimeScale;
        }        
    }

    /// <summary>
    /// Pauses the timeScale or resumes it depending on the bool passed in
    /// </summary>
    /// <param name="option">True for pause, false to resume</param>
    public void PauseTime(bool option)
    {
        // If the option is true...
        if (option)
        {            
            //... we pause the game
            // We set the game timeScale to 0
            this.SetTimeScale(0f);

            if (Toolbox.Instance.GameManager.AllowDebugCode)
            {
                Debug.Log("Time Paused!");
            } 
        }
        // If the option is false...
        else
        {
            //... we resume the game
            // We get back the timeScale that was working before the change            
            this.SetTimeScale(PreviousTimeScale);

            if (Toolbox.Instance.GameManager.AllowDebugCode)
            {
                Debug.Log("Time Resumed!");
            }
        }
    }

    
}
