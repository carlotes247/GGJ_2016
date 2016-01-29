using UnityEngine;
using System.Collections;
using System;

public class PlayerManager : ObjectManager {

    /// <summary>
    /// (Field) The transfrom of the gameObject
    /// </summary>
    [SerializeField]
    private Transform m_ObjectTransform;
    /// <summary>
    /// (Property) The transfrom of the gameObject
    /// </summary>
    public override Transform ObjectTransform { get { return m_ObjectTransform; } }

    /// <summary>
    /// (Field) The body-centered position of the player
    /// </summary>
    private Vector3 m_ObjectPosition;
    /// <summary>
    /// (Property) The body-centered position of the player
    /// </summary>
    public Vector3 ObjectPosition { get { return this.m_ObjectTransform.position + (Vector3.up * 0.7f); } }
    
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
    /// (Field) The WeaponController of the player
    /// </summary>
    [SerializeField]
    private WeaponController m_WeaponController;
    /// <summary>
    /// (Property) The WeaponController of the player
    /// </summary>
    public override WeaponController WeaponController { get { return this.m_WeaponController; } }

    /// <summary>
    /// (Property) The SightController [NOT IMPLEMENTED]
    /// </summary>
    public override SightController SightController
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    /// Allow the gizmo drawing of all enemy related gizmos
    // Field
    [SerializeField]
    private bool allowGizmos;
    // Property
    public override bool AllowGizmos { get { return this.allowGizmos; } }
    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
