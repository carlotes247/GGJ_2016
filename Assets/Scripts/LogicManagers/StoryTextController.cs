using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the storyTexts ingame
/// </summary>
public class StoryTextController : MonoBehaviour {

    [SerializeField]
    private string[] storyTextArray;
    /// <summary>
    /// (Property) The array of storyText strings
    /// </summary>
    public string[] StoryTextArray { get { return this.storyTextArray; } }

    [SerializeField]
    private int currentStoryText;
    /// <summary>
    /// (Property) The current piece of story
    /// </summary>
    public int CurrentStoryText { get { return this.currentStoryText; } set { this.currentStoryText = value; } }	

    /// <summary>
    /// Public function to set the storyText and update it on the HUD
    /// </summary>
    /// <param name="indexStoryText"> The index of the array of storyTexts we want to read from</param>
    public void SetStoryText (int indexStoryText)
    {
        // We check if the indexToGo is not out of the range of storyTexts
        if (indexStoryText >= StoryTextArray.Length || indexStoryText < 0)
        {
            //Debug.LogError("AAAAA");
            // If it is, we throw an exception
            throw new UnityException("The story " + indexStoryText.ToString() + " is out of the range of StoryTextArray!");
        }

        // We update the current storyText
        CurrentStoryText = indexStoryText;
        // We draw the story
        DrawStoryText(StoryTextArray[CurrentStoryText]);
    }

    /// <summary>
    /// Private function to draw on the HUD the story we passed in
    /// </summary>
    /// <param name="textToDraw"> The string contaiing the storyText to draw</param>
    private void DrawStoryText (string textToDraw)
    {
        // We call the HUDController and draw the storyText
        Toolbox.Instance.GameManager.HudController.UpdateStoryText(textToDraw);
    }

    /// <summary>
    /// Sets the next story text to the current
    /// </summary>
    public void NextStoryText()
    {      
        // We add 1 to the current Story text and we set it
        SetStoryText(CurrentStoryText + 1);
    }

    /// <summary>
    /// Sets the previous story text to the current
    /// </summary>
    public void PreviousStoryText()
    {
        // We remove 1 to the current Story text and we set it
        SetStoryText(CurrentStoryText - 1);
    }
}
