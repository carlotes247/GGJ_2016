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

/*! \file CanvasFindMainCamera.cs
 *  \brief This script finds the main camera and assign it to the canvas to draw in world space.
 * */
using UnityEngine;
using System.Collections;

public class CanvasFindMainCamera : MonoBehaviour {

    public Canvas canvas;
    private bool cameraFound;

	// Use this for initialization
	void Start () {
        canvas.renderMode = RenderMode.WorldSpace;
        cameraFound = false;
	}
	
	// Update is called once per frame
	void Update () {
        FindMainCamera();
	}

    // The function for finding the camera
    void FindMainCamera() {
        if (canvas.worldCamera == null)
        {
            Debug.Log("No main camera found");
            canvas.worldCamera = Camera.main;
            cameraFound = true;
        }
        //else
        //{
        //    //Debug.Log("Main camera found!");
        //    //canvas.transform.position = Camera.main.transform.position;
        //    //canvas.transform.position += Vector3.forward * 20;
        //}
    }
}
