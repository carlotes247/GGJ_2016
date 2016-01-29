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

/*! This script will control the different AIs in the game */
using UnityEngine;
using System.Collections;


/// <summary>
/// AI Controller controls all the AI bheaviours
/// </summary>
[ExecuteInEditMode]
public class AIController : MonoBehaviour
{

    /// The Enemy Manager so that this script has acces to the rest of components
    // Field
    [SerializeField]
    private ObjectManager objectManager;
    // Property
    public ObjectManager ObjectManager { get { return this.objectManager; } set { this.objectManager = value; } }

    /// The different AI behaviours
    /* 
    0: AIStatic,
    1: AIDynamic,
    2: AIAnimal,
    3: AISemiHuman        
        */
    // Field
    [SerializeField]
    public AIBehaviour[] aiBehaviours;
    // Property
    public AIBehaviour[] AIBehaviours { get { return this.aiBehaviours; } }

    /// The enum of AIs
    public enum TypeOfAI
    {
        AIStatic,
        AIDynamic,
        AIAnimal,
        AISemiHuman
    }
    // Field
    [SerializeField]
    private TypeOfAI aiType;
    // Property
    public TypeOfAI AIType { get { return this.aiType; } set { this.aiType = value; } }

    /* DEPRECATED. NOW ALL THE POINTS ARE PART OF POINTSTOGOCONTROLLER */
    ///// <summary>
    ///// The field of the array of points
    ///// </summary>
    //[SerializeField]
    //private Vector3[] pointsToGo;
    ///// <summary>
    ///// The array of points to go
    ///// </summary>
    //public Vector3[] PointsToGo { get { return this.pointsToGo; } set { this.pointsToGo = value; } }

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

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    public void FixedUpdate()
    {
        switch (AIType)
        {
            case TypeOfAI.AIStatic:
                AIBehaviours[0].Behave();                
                break;
            case TypeOfAI.AIDynamic:
                AIBehaviours[1].Behave();
                break;
            case TypeOfAI.AIAnimal:
                AIBehaviours[2].Behave();
                break;
            case TypeOfAI.AISemiHuman:
                AIBehaviours[3].Behave();
                break;
            default:
                break;
        }
    }

    
}
