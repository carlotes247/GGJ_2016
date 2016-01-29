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

///<summary>
/// This is the base script for any AIBehaviour. It needs to be inherited by other AIBehaviours
///</summary>
public abstract class AIBehaviour : MonoBehaviour {

    /// <summary>
    /// The basic AIBehaviour function - each derived class knows waht to do with this function
    /// </summary>
    public abstract void Behave();

}
