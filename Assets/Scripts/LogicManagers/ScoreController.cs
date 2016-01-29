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

/*! This script will take charge of controlling the global score of the game */
using UnityEngine;
using System.Collections;

/// <summary>
/// The controller of the score, in charge of adding and removing points
/// </summary>
public class ScoreController : MonoBehaviour {

    /// The global gameScore for the game
    // Field
    private float gameScore;
    // Property
    public float GameScore { get { return this.gameScore; } set { this.gameScore = value; } }

	// Use this for initialization
	void Start () {
        GameScore = 0;
        Toolbox.Instance.GameManager.HudController.UpdateHUDScore(GameScore.ToString());
    }

    // Update is called once per frame
    void Update () {
        //Toolbox.Instance.GameManager.HudController.UpdateHUDScore(GameScore.ToString());
        //Toolbox.Instance.GameManager.HudController.TestAlive();
        
    }

    /// The function in charge of adding a certain amount of points to the global score
    void AddPoints (float amount)
    {
        GameScore += amount;
    }

    /// The public functions that updates the score
    public void UpdateScore (float amount)
    {
        AddPoints(amount);
        Toolbox.Instance.GameManager.HudController.UpdateHUDScore(GameScore.ToString());
    }

    public void TestAlive()
    {
        if (Toolbox.Instance.GameManager.AllowDebugCode)
        {
            Debug.Log("ScoreController Alive!!" + GameScore.ToString()); 
        }
    }

    public void DebugScore()
    {
        if (Toolbox.Instance.GameManager.AllowDebugCode)
        {
            Debug.Log("Score is: " + GameScore.ToString());
        }
    }
}
