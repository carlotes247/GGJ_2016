/* 
* Copyright 2007 Carlos González Díaz
*  
* Licensed under the EUPL, Version 1.1 or – as soon they
will be approved by the European Commission - subsequent
versions of the EUPL (the "Licence");
* You may not use this work except in compliance with the
Licence.
* You may obtain a copy of the Licence at:
*  
*
https://joinup.ec.europa.eu/software/page/eupl
*  
* Unless required by applicable law or agreed to in
writing, software distributed under the Licence is
distributed on an "AS IS" basis,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
express or implied.
* See the Licence for the specific language governing
permissions and limitations under the Licence.
*/ 

/*!  This script is ins charge of managing the HUD of the game*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// The controller for the whole HUD. Right now, it controls the InGameCursor and the ScoreText
/// </summary>
public class HUDController : MonoBehaviour
{

    #region Attributes
    /// The text of the score
    // Field
    [SerializeField]
    private Text scoreText;
    // Property
    public Text ScoreText { get { return this.scoreText; } set { this.scoreText = value; } }

    /// The inGame cursor to move
    // Field
    [SerializeField]
    private Image inGameCursor;
    // Property
    public Image InGameCursor { get { return this.inGameCursor; } set { this.inGameCursor = value; } }

    [SerializeField]
    private Text storyText;
    /// <summary>
    /// (Property) The ingame story text to show on the hud
    /// </summary>
    public Text StoryText { get { return this.storyText; } set { this.storyText = value; } }

    #endregion

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
         
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// The function to update the score on the text
    public void UpdateHUDScore(string textToWrite)
    {
        ScoreText.text = textToWrite;
    }

    /// <summary>
    /// Updates the content of the StoryText in HUD
    /// </summary>
    /// <param name="textToWrite"> The string containing the text to show on the HUD </param>
    public void UpdateStoryText(string textToWrite)
    {
        StoryText.text = textToWrite;
    }

    public void TestAlive()
    {
        Debug.Log("HUDScoreAlive!");
    }

    public void DebugHUDScore ()
    {
        if (Toolbox.Instance.GameManager.AllowDebugCode)
        {
            Debug.Log("Score on HUD: " + ScoreText.text);
        }
    }
}
