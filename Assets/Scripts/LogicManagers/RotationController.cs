using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the code to rotate and lookAt of the current object
/// </summary>
public class RotationController : MonoBehaviour {

    /// <summary>
    /// (Field) The ObjectManager to access all of the components of this object
    /// </summary>
    [SerializeField]
    private ObjectManager m_ObjectManager;

    /// <summary>
    /// (Field) Flag to know if the controller is currently rotating something. 
    /// </summary>
    private bool m_ControllerInUse;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}    

    /// <summary>
    /// Makes the current object face the objective
    /// </summary>
    /// <param name="objective"> The point to look at in world coordinates</param>
    public void LookAt (Vector3 objective)
    {
        //// If the current object is moving with a navMeshAgent...
        //if (m_ObjectManager.MovementController.TypeOfMovement == MovementController.TypeOfMovementEnum.NavMeshAgent)
        //{
        //    // ... We deactivate the automatic rotation update from the navMeshAgent
        //    m_ObjectManager.MovementController.NavMAgent.updateRotation = false;
        //}
        m_ObjectManager.ObjectTransform.LookAt(objective);
    }

    /// <summary>
    /// [NOT IMPLEMENTED] Makes the transform origin to face the provided direction 
    /// </summary>
    /// <param name="origin"> The Transform to rotate </param>
    /// <param name="dir"> The direction to look at</param>
    public void LookAt(Transform origin, Vector3 dir)
    {
        throw new UnityException("LookAt(dir) not implemented!");
    }

    /// <summary>
    /// [NOT IMPLEMENTED] Makes origin face the objective
    /// </summary>
    /// <param name="origin"> The transform to rotate</param>
    /// <param name="objective"> The transform objective to face</param>
    public void LookAt (Transform origin, Transform objective)
    {
        throw new UnityException("LookAt(transform, transform) not implemented!");
    }

    /// <summary>
    /// Stops controlling the rotation of the current object, giving back control to other sources (navmeshAgent, etc)
    /// </summary>
    public void StopControlRotation ()
    {
        // If the controller is actually controlling the current object
        if (m_ControllerInUse)
        {
            // If the navMeshAgent is not controlling the rotation...
            if (m_ObjectManager.MovementController.NavMAgent.updateRotation == false)
            {
                // ... we give it back the rotation
                m_ObjectManager.MovementController.NavMAgent.updateRotation = true;
            }

            m_ControllerInUse = false;
        }
    }
  
}
