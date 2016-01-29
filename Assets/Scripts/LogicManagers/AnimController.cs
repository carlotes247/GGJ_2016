using UnityEngine;
using System.Collections;
using System;

///<summary>
/// This is the base script for any AnimatorController. It needs to be inherited by other AnimControllers
///</summary>
public abstract class AnimController : MonoBehaviour {

    /// <summary>
    /// (Property) The objectManager of the controller, to access the rest of components
    /// </summary>
    public abstract ObjectManager ObjectManager { get; }

    /// <summary>
    /// (Property) The Animator component to know what to animate
    /// </summary>
    public abstract Animator Anim { get; }

    /// <summary>
    /// Animates the Idle
    /// </summary>
    public abstract void Idle();

    /// <summary>
    /// Animaties the Walk
    /// </summary>
    public abstract void Walk(bool value);

    /// <summary>
    /// Animates the Run
    /// </summary>
    public abstract void Run(bool value);

    /// <summary>
    /// Animates the Attack
    /// </summary>
    public abstract void Attack(bool value);

    /// <summary>
    /// Animates the Damaged (trigger)
    /// </summary>
    public abstract void Damage();

    /// <summary>
    /// Animates the Eat
    /// </summary>
    public abstract void Eat(bool value);

    /// <summary>
    /// Animates the LookAround
    /// </summary>
    public abstract void LookAround(bool value);

    /// <summary>
    /// Animates the Interact() (trigger)
    /// </summary>
    public abstract void Interact();



}
