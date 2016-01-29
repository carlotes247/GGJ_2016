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
/// This is the base script for any ObjectManager. It needs to be inherited by other Managers
///</summary>
public abstract class ObjectManager : MonoBehaviour {

    /// <summary>
    /// (Property) The position of the object
    /// </summary>
    public abstract Transform ObjectTransform { get; }
    
    /// <summary>
    /// The property object's rigidbody
    /// </summary>
    public abstract Rigidbody ObjectRigidbody { get; }

    /// <summary>
    /// The LifeController of the object
    /// </summary>
    public abstract LifeController LifeController { get; }

    /// <summary>
    /// The MovementController of the object
    /// </summary>
    public abstract MovementController MovementController { get; }

    /// <summary>
    /// The RotationController of the object
    /// </summary>
    public abstract RotationController RotationController { get; }

    /// <summary>
    /// The AIController
    /// </summary>
    public abstract AIController AIController { get; }

    /// <summary>
    /// The TimerController
    /// </summary>
    public abstract TimerController TimerController { get; }

    /// <summary>
    /// The PointsController
    /// </summary>
    public abstract PointsController PointsController { get; }

    /// <summary>
    /// The AnimController
    /// </summary>
    public abstract AnimController AnimController { get; }

    /// <summary>
    /// The WeaponController
    /// </summary>
    public abstract WeaponController WeaponController { get; }

    /// <summary>
    /// The SightController
    /// </summary>
    public abstract SightController SightController { get; }

    /// <summary>
    /// Allow the gizmo drawing of all object's related gizmos
    /// </summary>
    public abstract bool AllowGizmos { get; }
}
