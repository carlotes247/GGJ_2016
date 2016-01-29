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
/// AIDynamic, move predictably between points
/// </summary>
public class AIDynamicBehaviour : AIBehaviour
{

    #region Fields&Attributes
    /// The Enemy Manager so that this script has acces to the rest of components
    // Field
    [SerializeField]
    private ObjectManager objectManager;
    // Property
    public ObjectManager ObjectManager { get { return this.objectManager; } set { this.objectManager = value; } }

    /// The selection mode of points enumaeration
    public enum PointSelectionModeEnum
    {
        Normal,
        Inverse,
        Random
    }
    [SerializeField]
    private PointSelectionModeEnum pointSelectionMode;
    /// <summary>
    /// (Property) The mode in which the points will be selected for moving
    /// </summary>
    public PointSelectionModeEnum PointSelectionMode { get { return this.pointSelectionMode; } set { this.pointSelectionMode = value; } }

    /// <summary>
    /// (Field) The index of the array of points to go
    /// </summary>
    [SerializeField]
    private int indexOfPointsToGo;
    /// <summary>
    /// (Property) The index of the array of points to go
    /// </summary>
    public int IndexOfPointsToGo { get { return this.indexOfPointsToGo; } set { this.indexOfPointsToGo = value; } } 
    #endregion

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
        
    }

    /// <summary>
    /// The Dynamic Behaviour will move the object to as many points as are specified in the AIController
    /// </summary>
    public override void Behave()
    {
        // We make the object go to a new point only when it has reached a point or is not moving at all
        if (ObjectManager.MovementController.MovementPhase == MovementController.MovementPhaseEnum.Stopping)
        {
            SwitchPoint(1);
        }
        
        // We move the object to the point
        //ObjectManager.MovementController.Move(ObjectManager.AIController.PointsToGo[IndexOfPointsToGo]);
        ObjectManager.MovementController.Move(ObjectManager.PointsController.PointsToGo[IndexOfPointsToGo]);

        // We animate the object to show the movement
        ObjectManager.AnimController.Walk(true);
    }

    /// <summary>
    /// Changes the index to access the array of pointsToGo in the AIController by the specified amount
    /// </summary>
    /// <param name="offSet"> The amount to add to the integer </param>
    private void SwitchPoint (int offSet)
    {
        
        int aux = 0;

        switch (PointSelectionMode)
        {
            case PointSelectionModeEnum.Normal:
                aux = IndexOfPointsToGo + offSet;
                // If it is higher than the length of the array, we reset the index to 0
                if (aux > ObjectManager.PointsController.PointsToGo.Length - 1)
                {
                    aux = 0;
                }
               
                break;
            case PointSelectionModeEnum.Inverse:
                aux = IndexOfPointsToGo;
                // We update the value of the index if it is still above 0
                if (aux > 0)
                {
                    aux -= offSet;                    
                }
                // If it is lower than 0, we set the index to the length of the array minus 1 (the maximun index possible)
                else
                {
                    aux = objectManager.PointsController.PointsToGo.Length - 1;
                }                
                                                                
                break;
            case PointSelectionModeEnum.Random:
                aux = UnityEngine.Random.Range(0, ObjectManager.PointsController.PointsToGo.Length);
                break;
            default:
                break;
        }

        // We set the desired index
        IndexOfPointsToGo = aux;     

    }

    

}
