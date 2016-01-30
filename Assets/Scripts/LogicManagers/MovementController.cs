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

/*! This script will be in charge of moving objects */
using UnityEngine;
using System.Collections;

/// <summary>
/// The controller in charge of moving the object. Right now rigidbody movement
/// </summary>
public class MovementController : MonoBehaviour
{

    #region Fields&Properties
    // Field
    [SerializeField]
    private ObjectManager objectManager;
    /// <summary>
    /// (Property) The Object Manager so that this script has acces to the rest of components
    /// </summary>
    public ObjectManager ObjectManager { get { return this.objectManager; } set { this.objectManager = value; } }

    [SerializeField]
    private NavMeshAgent navMAgent;
    /// <summary>
    /// (Property) The NavMeshAgent of the current Object
    /// </summary>
    public NavMeshAgent NavMAgent { get { return this.navMAgent; } set { this.navMAgent = value; } }

    /// <summary>
    /// The definition of the different types of movement
    /// </summary>
    public enum TypeOfMovementEnum
    {
        Rigidbody,
        NavMeshAgent,
        InputController
    }
    // Field
    [SerializeField]
    private TypeOfMovementEnum typeOfMovement;
    /// <summary>
    /// (Property) The actual type of movement
    /// </summary>
    public TypeOfMovementEnum TypeOfMovement
    {
        get { return this.typeOfMovement; }
        set { this.typeOfMovement = value; }
    }

    /// The point in space where the object is going to move
    // Field
    [SerializeField]
    private Vector3 positionToMove;
    /// <summary>
    /// (Property) The point in space where the object is going to move
    /// </summary>
    public Vector3 PositionToMove { get { return this.positionToMove; } set { this.positionToMove = value; } }

    /// <summary>
    /// The radius where we detect that the point has been reached (needing to stop or other action)
    /// </summary>
    // Field
    [Range (0f, 5f), SerializeField]
    private float stopRadius;
    /// <summary>
    /// (Property) The radius where we detect that the point has been reached (needing to stop or other action)
    /// </summary>
    public float StopRadius { get { return this.stopRadius; } set { this.stopRadius = value; } }

    /// The maximum velocity that the object can reach
    // Field
    [SerializeField, Range (0f, 20f)]
    private float maxVelocity;
    /// <summary>
    /// (Property) The maximum velocity that the object can reach
    /// </summary>
    public float MaxVelocity { get { return this.maxVelocity; } set { this.maxVelocity = value; } }

    /// The amount of force to apply to the object every fixed frame
    // Field
    [SerializeField, Range (0f, 10f)]
    private float forceToApply;
    /// <summary>
    /// (Property) The amount of force to apply to the object every fixed frame
    /// </summary>
    private float ForceToApply { get { return this.forceToApply; } set { this.forceToApply = value; } }

    /// The multiplier of the force to stop
    // Field
    [SerializeField, Range(0f, 20f)]
    private float multiplierForceToStop;
    /// <summary>
    /// (Property) The multiplier of the force to stop
    /// </summary>
    private float MultiplerForceToStop { get { return this.multiplierForceToStop; } }

    /// The direction in which the force is going to be applied
    // Field
    private Vector3 directionOfForce;
    /// <summary>
    /// (Property) The direction in which the force is going to be applied
    /// </summary>
    private Vector3 DirectionOfForce { get { return this.directionOfForce; } set { this.directionOfForce = value; } }

    /// The initial distance to the point when the movement started
    // Field
    [SerializeField]
    private float initialDistanceToPoint;
    /// <summary>
    /// (Property) The initial distance to the point when the movement started
    /// </summary>
    public float InitialDistanceToPoint { get { return this.initialDistanceToPoint; } set { this.initialDistanceToPoint = value; } }

    /// The current distance to the point
    // Field
    [SerializeField]
    private float distanceToPoint;
    /// <summary>
    /// (Property) The current distance to the point
    /// </summary>
    public float DistanceToPoint { get { return this.distanceToPoint; } set { this.distanceToPoint = value; } }

    /// The bool that states when the movement started
    // Field
    [SerializeField]
    private bool movementStarted;
    /// <summary>
    /// (Property) The bool that states when the movement started
    /// </summary>
    public bool MovementStarted { get { return this.movementStarted; } set { this.movementStarted = value; } }

    /// <summary>
    /// The definiton of the different phases of movement as an enum
    /// </summary>
    public enum MovementPhaseEnum
    {
        Stopped,
        Moving,
        Stopping
    }
    /// <summary>
    /// The field of the movementPhase
    /// </summary>
    [SerializeField]
    private MovementPhaseEnum movementPhase;
    /// <summary>
    /// The actual phase in the movement
    /// </summary>
    // Property
    public MovementPhaseEnum MovementPhase { get { return this.movementPhase; } set { this.movementPhase = value; } }

    /// <summary>
    /// (Field) Variable that stores the approximation of the point in the navmesh
    /// </summary>
    private NavMeshHit sampledHit;
    /// <summary>
    /// (Property) Variable that stores the approximation of the point in the navmesh
    /// </summary>
    public NavMeshHit SampledHit { get { return this.sampledHit; } set { this.sampledHit = value; } }

    #endregion

    #region MainFunctions
    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        UpdateMovementPhase(MovementPhaseEnum.Stopped);
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
        //Move(PositionToMove);
    }
    #endregion

    #region ExtraFunctions
    /// <summary>
    /// Dependant on TypeOfMovement. The public function that will move the object to a given point in space
    /// </summary>
    /// <param name="pointToMove"> This is the point where the object will move towards </param>
    public void Move(Vector3 pointToMove)
    {
        switch (TypeOfMovement)
        {
            case TypeOfMovementEnum.Rigidbody:
                MoveRigidbody(pointToMove);
                break;
            case TypeOfMovementEnum.NavMeshAgent:
                MoveNavMeshAgent(pointToMove);
                break;            
            default:
                break;
        }
    }

    /// <summary>
    /// Dependant on TypeOfMovement. The public function that will move the object to a given point in space
    /// </summary>
    /// <param name="pointToMove"> This is the point where the object will move towards </param>
    /// <param name="speed"> The speed at which the object will move (if not set, the object will move at MaxVelocity)</param>
    public void Move(Vector3 pointToMove, float speed)
    {
        switch (TypeOfMovement)
        {
            case TypeOfMovementEnum.Rigidbody:
                MoveRigidbody(pointToMove);
                break;
            case TypeOfMovementEnum.NavMeshAgent:
                MoveNavMeshAgent(pointToMove);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// The function that will move the object to a given point in space using the RIGIDBODY
    /// </summary>
    /// <param name="pointToMove"> This is the point that the object will move towards </param>
    private void MoveRigidbody (Vector3 pointToMove)
    {
        // We calculate the normalized direction to move (it includes an automatic update of the distance)
        CalculateDirection(ObjectManager.ObjectRigidbody.position, pointToMove);


        // We check if we are not in the point already
        if (DistanceToPoint < StopRadius)
        {
            Stop();
        }
        // We apply a positive force if the velocity is lower than the maxVelocity
        else if (ObjectManager.ObjectRigidbody.velocity.magnitude < MaxVelocity)
        {
            ObjectManager.ObjectRigidbody.AddForce(ForceToAdd(ForceToApply) * DirectionOfForce);
            // We update the movement phase
            UpdateMovementPhase(MovementPhaseEnum.Moving);

        }
        // If the velocity is higher, we remove velocity with a negative force to keep it constant
        else
        {
            ObjectManager.ObjectRigidbody.AddForce(ForceToAdd(ForceToApply) * (-ObjectManager.ObjectRigidbody.velocity.normalized));

        }

        DebugCode();
    }

    private void MoveRigidbody (Vector3 pointToMove, float speed)
    {

    }

    /// <summary>
    /// The function that will move the object to a given point in space using the NAVMESHAGENT
    /// </summary>
    /// <param name="pointToMove"> This is the point that the object will move towards </param>
    private void MoveNavMeshAgent (Vector3 pointToMove)
    {
        
        // We only sample the position if the point has changed
        if (PositionToMove != pointToMove)
        {
            // We sample the position of the pointToMove into the NavMesh
            // THE HEIGHT OF THE AGENT NEEDS TO BE EQUAL TO THE RADIUS OF THE OPPOSITE POINT CALCULATION IN THE AIANIMAL
            NavMesh.SamplePosition(pointToMove, out sampledHit,
                NavMAgent.height * ObjectManager.PointsController.RadiusOfPointToCalculate, NavMesh.AllAreas);
            // We save the position to move to compare it next frame
            PositionToMove = pointToMove; 
        }

        // We get the distance to the sample position
        CalculateDistance(ObjectManager.ObjectRigidbody.position, sampledHit.position);
        
        // We check if we are not in the point already
        if (DistanceToPoint < StopRadius)
        {

            // We stope the agent
            //NavMAgent.Stop();
            // We update the movement phase
            UpdateMovementPhase(MovementPhaseEnum.Stopping);
        }
        // We keep moving the NavMeshAgent
        else
        {
            // We set destination twice while resuming the navmeshagent
            //NavMAgent.SetDestination(sampledHit.position);
            NavMAgent.Resume();
            NavMAgent.SetDestination(sampledHit.position);
            // We update the movement phase
            UpdateMovementPhase(MovementPhaseEnum.Moving);

        }
    }

    private void MoveNavMeshAgent(Vector3 pointToMove, float speed)
    {
        // We only sample the position if the point has changed
        if (PositionToMove != pointToMove)
        {
            // We sample the position of the pointToMove into the NavMesh
            // THE HEIGHT OF THE AGENT NEEDS TO BE EQUAL TO THE RADIUS OF THE OPPOSITE POINT CALCULATION IN THE AIANIMAL
            NavMesh.SamplePosition(pointToMove, out sampledHit,
                NavMAgent.height * ObjectManager.PointsController.RadiusOfPointToCalculate, NavMesh.AllAreas);
            // We save the position to move to compare it next frame
            PositionToMove = pointToMove; 
        }

        // We get the distance to the sample position
        CalculateDistance(ObjectManager.ObjectRigidbody.position, sampledHit.position);

        // We check if we are not in the point already
        if (DistanceToPoint < StopRadius)
        {

            // We stope the agent
            //NavMAgent.Stop();
            // We update the movement phase
            UpdateMovementPhase(MovementPhaseEnum.Stopping);
        }
        // We keep moving the NavMeshAgent
        else
        {
            // We update the speed of our object
            NavMAgent.speed = speed;
            // We set destination twice while resuming the navmeshagent            
            //NavMAgent.SetDestination(sampledHit.position);
            NavMAgent.Resume();
            NavMAgent.SetDestination(sampledHit.position);
            // We update the movement phase
            UpdateMovementPhase(MovementPhaseEnum.Moving);

        }
    }

    /// <summary>
    /// Moves the current object according to the axis and speed passed in
    /// </summary>
    /// <param name="axisValues"> The axis to move the current object  </param>
    /// <param name="speed"> The speed to move at </param>
    public void MoveInputController(Vector2 axisValues, float speed)
    {
        // If x is between (-0.1, 0.1) ...
        if (axisValues.x > -0.1 && axisValues.x < 0.1)
        {
            // x is too small, we set it to 0
            axisValues.x = 0f;
        }
        // If y is between (-0.1, 0.1) ...
        if (axisValues.y > -0.1 && axisValues.y < 0.1)
        {
            // y is too small, we set it to 0
            axisValues.y = 0f;
        }
        // We move only in the x and z axis (x, 0, z)
        ObjectManager.ObjectTransform.Translate(axisValues.x * speed, 0f, axisValues.y * speed);
        
        
    }

    /// The function that will calculate the direction to add a force to the object
    private void CalculateDirection (Vector3 fromPos, Vector3 toPos)
    {
        // We calculate the direction vector between the two points
        DirectionOfForce = (toPos - fromPos);
        // We calculate the distance to the point 
        CalculateDistance(DirectionOfForce);
        // We calculate the normal vector of the direction
        DirectionOfForce.Normalize();        
    }

    /// The function to calculate the distance based on a direction vector
    private void CalculateDistance (Vector3 directionVector)
    {
        // We calculate the current distance to the point
        DistanceToPoint = directionVector.magnitude;

        // We set the initial distance to point if the object was stopped or stopping
        if (MovementPhase == MovementPhaseEnum.Stopped)
        {
            InitialDistanceToPoint = DistanceToPoint;
        }
        
    }

    /// The function to calculate the distance based on origin and destination
    private void CalculateDistance (Vector3 fromPos, Vector3 toPos)
    {
        DistanceToPoint = (toPos - fromPos).magnitude;
    }

    /// The function that will stop the object
    private void Stop ()
    {
        // If the velocity reaches a value close to 0, we complelety stop the object
        if (ObjectManager.ObjectRigidbody.velocity.magnitude < 0.05f)
        {
            TotalStop();
        }
        // We apply a force only if the object is still moving
        else if (ObjectManager.ObjectRigidbody.velocity.magnitude > 0.1f)
        {
            // We apply a big force (the value of the velocity) in the opposite direction of the movement
            ObjectManager.ObjectRigidbody.AddForce((ForceToStop(ForceToApply) ) * (-DirectionOfForce));

            // We update the movementPhase
            UpdateMovementPhase(MovementPhaseEnum.Stopping);
        }
        else if (ObjectManager.ObjectRigidbody.velocity.magnitude < 0.1f)
        {
            // We apply a big force (the value of the velocity) in the opposite direction of the movement
            ObjectManager.ObjectRigidbody.AddForce((ForceToStop(ForceToApply) * Normalize(ObjectManager.ObjectRigidbody.velocity.magnitude, MaxVelocity) / MultiplerForceToStop ) * (-DirectionOfForce));

            // We update the movementPhase
            UpdateMovementPhase(MovementPhaseEnum.Stopping);

        }
        
       
        
    }

    /// <summary>
    /// Returns a normalized value for an input
    /// </summary>
    private float Normalize (float numerator, float denominator)
    {
        if (numerator <= denominator)
        {
            return numerator / denominator;
        }
        else
        {
            return 1f;
        }
    }

    /// <summary>
    /// Returns the force to add to the object based on the following formula: ((ForceToApply * DistanceToPoint.normalized) - velocity )
    /// </summary>
    /// <param name="force"> The input force to apply in the ecuation</param>
    /// <returns> The calculated force to add. The more vel, the less force. The closest to the point, the less force.</returns>
    private float ForceToAdd(float force)
    {
        float aux;
        aux = (force * Normalize(DistanceToPoint, InitialDistanceToPoint)*MultiplerForceToStop) - ObjectManager.ObjectRigidbody.velocity.magnitude;
        return aux;
    }

    /// <summary>
    /// Returns the force to add in order to STOP the object, based on the following formula: (((ForceToApply * StopRadius.normalized) + velocity ) * multiplier)
    /// </summary>
    /// <param name="force"> The input force to apply in the ecuation</param>
    /// <returns> The calculated force to stop. The more vel, the more forceToStop. The closest to the point, the less force to stop.</returns>
    private float ForceToStop(float force)
    {
        float aux;
        aux = ((force * Normalize(DistanceToPoint, StopRadius)) + ObjectManager.ObjectRigidbody.velocity.magnitude) * MultiplerForceToStop;
        return aux;
    }

    /// <summary>
    /// Updates the value of the phase in the movement
    /// </summary>
    /// <param name="newPhase">The new movementPhase to update to </param>
    private void UpdateMovementPhase(MovementPhaseEnum newPhase)
    {
        // We check if the phase is not the same one, to avoid unncesary overwriting
        if (MovementPhase != newPhase)
        {
            MovementPhase = newPhase;
        }
    }

    /// <summary>
    /// Completely stops the movement of the object
    /// </summary>
    public void TotalStop ()
    {
        // We see what kind of movement the object has
        switch (TypeOfMovement)
        {
            case TypeOfMovementEnum.Rigidbody:
                // We totally freeze the object position and rotation
                ObjectManager.ObjectRigidbody.velocity = Vector3.zero;
                ObjectManager.ObjectRigidbody.angularVelocity = Vector3.zero;                
                break;
            case TypeOfMovementEnum.NavMeshAgent:
                // We totally freeze the object position and rotation
                ObjectManager.ObjectRigidbody.velocity = Vector3.zero;
                ObjectManager.ObjectRigidbody.angularVelocity = Vector3.zero;

                // We stop the navMeshAgent
                NavMAgent.Stop();
                

                //Debug.Log("TotalStop called!");
                break;
            default:
                break;
        }

        // We update the movementPhase
        UpdateMovementPhase(MovementPhaseEnum.Stopped);

    }

    // We draw a gizmo for the positionToMove
    void OnDrawGizmos()
    {
        if (ObjectManager.AllowGizmos)
        {
            // Green for the positionToMove
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(PositionToMove, StopRadius);

            // Magenta for the Sampled position on the navmesh
            if (ObjectManager.MovementController.TypeOfMovement == MovementController.TypeOfMovementEnum.NavMeshAgent)
            {                
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(SampledHit.position, 1f);               
            }
        }
    }

    /// We debug the code 
    private void DebugCode ()
    {
        if (Toolbox.Instance.GameManager.AllowDebugCode)
        {
            Debug.DrawRay(this.transform.position, DirectionOfForce, Color.red);
            Debug.Log("Object Speed is: " + ObjectManager.ObjectRigidbody.velocity.magnitude.ToString());
        }
    }
    #endregion
}
