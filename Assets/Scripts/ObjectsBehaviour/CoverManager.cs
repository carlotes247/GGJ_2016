using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// The controller for the covers, providing a list of safeSpots
/// </summary>
public class CoverManager : ObjectManager {

    #region Fields&Properties
    /// <summary>
    /// (Field) These are all the safe spots that the Cover is providing
    /// </summary>
    [SerializeField]    
    private List<Transform> m_SafeSpots;
    /// <summary>
    /// (Property) These are all the safe spots that the Cover is providing
    /// </summary>
    public List<Transform> SafeSpots { get { return this.m_SafeSpots; } }

    /*====ABSTRACT CLASS IMPLEMENTATION===*/

    public override AIController AIController
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override bool AllowGizmos
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override AnimController AnimController
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override LifeController LifeController
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override MovementController MovementController
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override Rigidbody ObjectRigidbody
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override Transform ObjectTransform
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override PointsController PointsController
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override TimerController TimerController
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override WeaponController WeaponController
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override SightController SightController
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override RotationController RotationController
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    #endregion


    // Use this for initialization
    void Start () {
        // We get all the safeSpots that the cover is having
        GetComponentsInChildren<Transform>(m_SafeSpots);
        for (int i = 0; i < SafeSpots.Count; i++)
        {
            Debug.Log("Cover found!");
        }
	}
		
}
