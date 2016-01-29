using UnityEngine;
using System.Collections;
using System;

public class EnemyAnimController : AnimController {

    [SerializeField]
    private ObjectManager objectManager;
    /// <summary>
    /// (Property) The Object Manager so that this script has acces to the rest of components
    /// </summary>
    public override ObjectManager ObjectManager { get { return this.objectManager; } }

    [SerializeField]
    Animator anim;
    /// <summary>
    /// (Property) The animator of this gameObject
    /// </summary>
    public override Animator Anim { get { return this.anim; } }

    /// <summary>
    /// The id of the damageTrigger
    /// </summary>
    private int damageTriggerID;
    /// <summary>
    /// 
    /// </summary>
    private int interactTriggerID;
    /// <summary>
    /// The id of the walkBool
    /// </summary>
    private int walkBoolID;
    /// <summary>
    /// The id of the runBool
    /// </summary>
    private int runBoolID;
    /// <summary>
    /// The id of the eatBool
    /// </summary>
    private int eatBoolID;
    /// <summary>
    /// The id of the lookAroundBool
    /// </summary>
    private int lookAroundBoolID;

    private bool setAllAnimOffCalled;
    /// <summary>
    /// (Property) Flag to control that the function SetAllAnimationsOff is called only once, to avoid infinite loops
    /// </summary>
    public bool SetAllAnimOffCalled { get { return this.setAllAnimOffCalled; } set { this.setAllAnimOffCalled = value; } }

    // Called before start, when the component loads
    void Awake ()
    {
        // We get all the Ids from the animator, to use on the logic
        HashIDs();
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Gets the ids from all the parameters of the animator
    /// </summary>
    private void HashIDs ()
    {
        damageTriggerID = Animator.StringToHash("Damage");
        interactTriggerID = Animator.StringToHash("Interact");
        walkBoolID = Animator.StringToHash("Walk");
        runBoolID = Animator.StringToHash("Run");
        eatBoolID = Animator.StringToHash("Eat");
        lookAroundBoolID = Animator.StringToHash("LookAround");
    }

    /// <summary>
    /// Deactivates any animation to set the idle one
    /// </summary>
    public override void Idle()
    {
        SetAllAnimationsOff();
    }

    /// <summary>
    /// Activates the walk animation
    /// </summary>
    public override void Walk(bool value)
    {
        // We only execute the code if the value is not the same
        if (Anim.GetBool(walkBoolID) != value)
        {
            SetAllAnimationsOff();
            Anim.SetBool(walkBoolID, value);
        }        
    }

    /// <summary>
    /// Activates the run animation
    /// </summary>
    public override void Run(bool value)
    {
        // We only execute the code if the value is not the same
        if (Anim.GetBool(runBoolID) != value)
        {
            SetAllAnimationsOff();
            Anim.SetBool(runBoolID, value); 
        }
    }

    /// <summary>
    /// Activates the attack animation
    /// </summary>
    public override void Attack(bool value)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Triggers the hit animation
    /// </summary>
    public override void Damage()
    {
        // Because it is a trigger, it has a bool that is always true (ReturningAll) to come back to idle, so it only happens once
        this.Anim.SetTrigger(damageTriggerID);
    }

    /// <summary>
    /// Activates the eat animation
    /// </summary>
    public override void Eat(bool value)
    {
        // We only execute the code if the value is not the same
        if (Anim.GetBool(eatBoolID) != value)
        {
            SetAllAnimationsOff();
            Anim.SetBool(eatBoolID, value); 
        }
    }

    /// <summary>
    /// Activates the look around animation
    /// </summary>
    public override void LookAround(bool value)
    {
        // We only execute the code if the value is not the same
        if (Anim.GetBool(lookAroundBoolID) != value)
        {
            SetAllAnimationsOff();
            Anim.SetBool(lookAroundBoolID, value); 
        }
    }

    /// <summary>
    /// Triggers the Interact animation
    /// </summary>
    public override void Interact()
    {
        // Because it is a trigger, it has a bool that is always true (ReturningAll) to come back to idle, so it only happens once
        this.Anim.SetTrigger(interactTriggerID);
    }

    /// <summary>
    /// Sets all animations off
    /// </summary>
    private void SetAllAnimationsOff()
    {
        // We only execute this if it is not being called already
        if (!SetAllAnimOffCalled)
        {
            // We protect the function with a flag
            SetAllAnimOffCalled = true;

            Walk(false);
            Run(false);
            //Attack(false);
            Eat(false);
            LookAround(false);

            // We unprotect the function
            SetAllAnimOffCalled = false;
        }
    }

    
}
