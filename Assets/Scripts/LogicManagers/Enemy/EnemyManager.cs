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

/*! The Enemy Manager will expose all the controllers of the enemy so that they can interact between them */
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The manager for the enemy. Holds all the necessary controllers and logic
/// </summary>
public class EnemyManager : ObjectManager {

    /// <summary>
    /// (Field) The transfrom of the gameObject
    /// </summary>
    [SerializeField]
    private Transform m_ObjectTransform;
    /// <summary>
    /// (Property) The transfrom of the gameObject
    /// </summary>
    public override Transform ObjectTransform { get { return m_ObjectTransform; } }

    /// The LifeController of the object
    // Field
    [SerializeField]
    private LifeController lifeController;
    // Property
    public override LifeController LifeController { get { return this.lifeController; } }

    /// The MovementController of the object
    // Field
    [SerializeField]
    private MovementController movementController;
    // Property
    public override MovementController MovementController { get { return this.movementController; } }

    /// <summary>
    /// (Field) The RotationController of the object
    /// </summary>
    [SerializeField]
    private RotationController m_RotationController;
    /// <summary>
    /// (Property) The RotationController of the object
    /// </summary>
    public override RotationController RotationController { get { return this.m_RotationController; } }

    /// The AIController
    // Field
    [SerializeField]
    private AIController aiController;
    // Property
    public override AIController AIController { get { return this.aiController; } }

    /// The object rigidbody
    // Field
    [SerializeField]
    private Rigidbody objectRigidbody;
    // Property
    public override Rigidbody ObjectRigidbody { get { return this.objectRigidbody; } }

    [SerializeField]
    private TimerController timerController;
    /// <summary>
    /// (Property) The TimerController
    /// </summary>
    public override TimerController TimerController { get { return this.timerController; } }

    [SerializeField]
    private PointsController pointsController;
    /// <summary>
    /// (Property) The PointsController
    /// </summary>
    public override PointsController PointsController { get { return this.pointsController; } }

    [SerializeField]
    private AnimController animController;
    /// <summary>
    /// (Property) The AnimController (for animations)
    /// </summary>
    public override AnimController AnimController { get { return this.animController; } }

    /// <summary>
    /// (Field) The WeaponController of the enemy
    /// </summary>
    [SerializeField]
    private WeaponController m_WeaponController;
    /// <summary>
    /// (Property) The WeaponController of the enemy
    /// </summary>
    public override WeaponController WeaponController { get { return this.m_WeaponController; } }

    /// <summary>
    /// (Field) The Sight controller of the enemy
    /// </summary>
    [SerializeField]
    private SightController m_SightController;
    /// <summary>
    /// (Property) The Sight controller of the enemy
    /// </summary>
    public override SightController SightController { get { return this.m_SightController; } }
    

    /// Allow the gizmo drawing of all enemy related gizmos
    // Field
    [SerializeField]
    private bool allowGizmos;
    // Property
    public override bool AllowGizmos { get { return this.allowGizmos; } }
    
}
