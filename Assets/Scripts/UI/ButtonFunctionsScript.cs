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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script will store a lot of functions
/// </summary>
public class ButtonFunctionsScript : MonoBehaviour {

	/// The result controller of the scene (if any)
	GameObject resultController;
	

	/// Start is called just before any of the
	/// Update methods is called the first time.
	void Start () {
		//FindResulController();
	}
	
	/// Update is called every frame, if the
	/// MonoBehaviour is enabled.
	void Update () {
		
	}

	/// Loads a level depending on a name
	public void LoadLevel (string levelName) {
		Application.LoadLevel(levelName);
	}

	/// Exits the game
	public void ExitGame () {
		Application.Quit();
	}

	/// This function communicates with result controller and set the minigame to pass
	public void PassMinigame () {
		resultController.SendMessage("PassMinigame");
	}

	/// This function communicates with result controller and set the minigame to fail
	public void FailMinigame () {
		resultController.SendMessage("FailMinigame");
	}

	/// Finds the result controller in the scene
	void FindResulController () {
		GameObject aux = GameObject.FindWithTag("ResultController");
		// We check if aux is null. If it is, there is no ResultController in the scene
		if (aux!= null) {
			this.resultController = aux;
		}
	}
	
}
