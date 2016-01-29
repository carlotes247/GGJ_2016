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
using System;

/// <summary>
/// Static AI, will stop the object totally
/// </summary>
public class AIStaticBehaviour : AIBehaviour {

    /// The Enemy Manager so that this script has acces to the rest of components
    // Field
    [SerializeField]
    private ObjectManager objectManager;
    // Property
    public ObjectManager ObjectManager { get { return this.objectManager; } set { this.objectManager = value; } }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    /// <summary>
    /// Static Behaviour, it is not moving
    /// </summary>
    public override void Behave()
    {
        // We completely stop the object
        ObjectManager.MovementController.TotalStop();
    }

    /* 
    Enjoy the cake of nothing! :D
             ,   ,   ,   ,             
           , |_,_|_,_|_,_| ,           
       _,-=|;  |,  |,  |,  |;=-_       
     .-_| , | , | , | , | , |  _-.     
     |:  -|:._|___|___|__.|:=-  :|     
     ||*:  :    .     .    :  |*||     
     || |  | *  |  *  |  * |  | ||     
 _.-=|:*|  |    |     |    |  |*:|=-._ 
-    `._:  | *  |  *  |  * |  :_.'    -
 =_      -=:.___:_____|___.: =-     _= 
    - . _ __ ___  ___  ___ __ _ . -    

    
    */
}
