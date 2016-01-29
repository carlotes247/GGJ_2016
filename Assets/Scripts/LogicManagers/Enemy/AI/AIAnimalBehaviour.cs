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
using ReusableMethods;

/// <summary>
/// Animal AI, should move randomly between some points for harder prediction
/// </summary>
public class AIAnimalBehaviour : AIBehaviour
{

    #region Fields&Properties
    [SerializeField]
    private ObjectManager objectManager;
    /// <summary>
    /// (Property) The object manager of the current GameObject. Contains all the controllers
    /// </summary>
    public ObjectManager ObjectManager { get { return this.objectManager; } }

    /// <summary>
    /// The definition of the enumeration of states of the animal
    /// </summary>
    public enum AnimalStateEnum
    {
        Calm,
        Scared,
        RunningAway,
    }
    [SerializeField]
    private AnimalStateEnum animalState;
    /// <summary>
    /// (Property) The actual state of the animal
    /// </summary>s
    public AnimalStateEnum AnimalState { get { return this.animalState; } set { this.animalState = value; } }

    [SerializeField, Range(0f, 100f)]
    private float radiusOfDangerDetection;
    /// <summary>
    /// (Property) The radius use to detect a threat
    /// </summary>
    public float RadiusOfDangerDetection { get { return this.radiusOfDangerDetection; } set { this.RadiusOfDangerDetection = value; } }

    private Vector3 pointToScape;
    /// <summary>
    /// (Property) The point in space to go when the object is scared
    /// </summary>
    private Vector3 PointToScape { get { return this.pointToScape; } set { this.pointToScape = value; } }

    private Vector3 dangerPoint;
    /// <summary>
    /// (Property) The point that is dangerous to the object in world space 
    /// </summary>
    private Vector3 DangerPoint { get { return this.dangerPoint; } set { this.dangerPoint = value; } }

    ///// <summary>
    ///// (Property) The last shot point registered
    ///// </summary>
    //private Vector3 lastShotPos;

    private Vector3 directionOfDanger;
    /// <summary>
    /// (Property) The normalized direction from where the danger comes
    /// </summary>
    public Vector3 DirectionOfDanger { get { return this.directionOfDanger; } set { this.directionOfDanger = value; } }

    [SerializeField]
    private bool animalBehaviourCoroutineRunning;
    /// <summary>
    /// (Property) The flag that controls if the coroutine is running or not
    /// </summary>
    public bool AnimalBehaviourCoroutineRunning { get { return this.animalBehaviourCoroutineRunning; } set { this.animalBehaviourCoroutineRunning = value; } }

    private IEnumerator coroutineInstance;
    /// <summary>
    /// (Property) The instance of the coroutine to run
    /// </summary>
    private IEnumerator CoroutineInstance { get { return this.coroutineInstance; } set { this.coroutineInstance = value; } }

    /// <summary>
    /// The enum of the different tasks the animal can perform
    /// </summary>
    public enum AnimalTasksEnum
    {
        Moving,
        LookingAround,
        Eating
    }
    [SerializeField]
    private AnimalTasksEnum animalTask;
    /// <summary>
    /// (Property) The current task that the animal is performing
    /// </summary>
    public AnimalTasksEnum AnimalTask { get { return this.animalTask; } set { this.animalTask = value; } }

    [SerializeField]
    private float secondsToNewTask;
    /// <summary>
    /// (Property) The amount of seconds needed to perform a new task
    /// </summary>
    private float SecondsToNewTask { get { return this.secondsToNewTask; } set { this.secondsToNewTask = value; } }

    [SerializeField, Range(0f, 1f)]
    private float probNewTask;
    /// <summary>
    /// (Property) The probability the object has of performing a new task
    /// </summary>
    public float ProbNewTask { get { return this.probNewTask; } set { this.probNewTask = value; } }

    private float speedToMove;
    /// <summary>
    /// (Property) The speed at which the objects moves. It will be a random value between the max in the moveController and 1f. We recalculate it everytime we set a new task
    /// </summary>
    public float SpeedToMove { get { return this.speedToMove; } set { this.speedToMove = value; } }
    #endregion

    // This function is called when the object becomes enabled and active
    public void OnEnable()
    {
        /* Set false to the coroutine flag. This is allowing that if the object gets
        deactivated and then activated, it can start the coroutine again */
        AnimalBehaviourCoroutineRunning = false;
    }

    // Use this for initialization
    void Start()
    {
        // We initialize the class
        Initialize();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Runs away from a shot, to the opposite direction where the shot was performed up to a determined distance
    /// </summary>
    public override void Behave()
    {
        
        //// We start the coroutine protected by a flag, that is, if the coroutine is not runnin we start its execution
        //Toolbox.Instance.GameManager.CoroutineController.StartCoroutineFlag(AnimalBehaviourCoroutine(), 
        //    ref coroutineInstance, ref animalBehaviourCoroutineRunning);

        this.AnimalBehaviourLogic();

    }

    /// <summary>
    /// Initializes all the fields and propertes of the class to their desire initial status
    /// </summary>
    private void Initialize()
    {
        SecondsToNewTask = Random.Range(0f, 5f);
    }

    /// <summary>
    /// The coroutine that is handling the animal behaviour
    /// </summary>
    /// <returns> Null for next frame. Break to end the coroutine. </returns>
    private IEnumerator AnimalBehaviourCoroutine()
    {
        // Debug.Log("Coroutine STARTED in " + ObjectManager.name.ToString());

        // The loop of the coroutine. It will stop looping when we change the type of AI in the AIController
        // It will also stop running if the AIController gets disabled
        while (ObjectManager.AIController.AIType == AIController.TypeOfAI.AIAnimal && ObjectManager.AIController.enabled)
        {

            // Debug.Log("Coroutine RUNNING in " + ObjectManager.name.ToString());

            // The main logic of the animal
            this.AnimalBehaviourLogic();

            // we wait for the next frame to execute (IT IS NECESSARY!)
            yield return null;

        }

        // Because we have exited the loop, it means that TypeOfAI in AIcontroller is not AIAnimal anymore
        // We set the running flag to false, the Behave method is not executing
        AnimalBehaviourCoroutineRunning = false;

        // Debug.Log("Coroutine ENDED in " + ObjectManager.name.ToString());

        // We stop the execution of the coroutine
        yield break;

    }

    /// <summary>
    /// The logic of the animal behaviour
    /// </summary>
    private void AnimalBehaviourLogic()
    {
        // Logic of the loop
        switch (AnimalState)
        {
            case AnimalStateEnum.Calm:                

                // Every X seconds, we could trigger a change in the task to perform
                if (ObjectManager.TimerController.GenericCountDown(SecondsToNewTask))
                {
                    // We check if the object needs to change its task 
                    if (Probabilities.GetResultProbability(ProbNewTask))
                    {
                        // We get a random task from the enum
                        AnimalTask = Enums.GetRandomEnumValue<AnimalTasksEnum>();

                        // We calculate a random speed to move calmly
                        SpeedToMove = Random.Range(1f, ObjectManager.MovementController.MaxVelocity);

                    }
                    // We update the random amount of seconds needed for the new task
                    SecondsToNewTask = Random.Range(0f, 5f);
                }

                // Now we check which task the animal needs to perform
                switch (AnimalTask)
                {
                    case AnimalTasksEnum.Moving:
                        // If the object needs to move, then we move it in a Dynamic Predictable manner (that can be configured as we want in the editor)               
                        ObjectManager.AIController.AIBehaviours[1].Behave();
                        break;
                    case AnimalTasksEnum.LookingAround:
                        // We stop the object
                        ObjectManager.MovementController.TotalStop();
                        // We show the animation
                        ObjectManager.AnimController.LookAround(true);
                        //Debug.Log(ObjectManager.gameObject.name + ": I think I see something interesting...");
                        break;
                    case AnimalTasksEnum.Eating:
                        // We stop the object
                        ObjectManager.MovementController.TotalStop();
                        // We show the animation
                        ObjectManager.AnimController.Eat(true);
                        //Debug.Log(ObjectManager.gameObject.name + ": I am eating, nom, nom, nom...");
                        break;
                    default:
                        break;
                }



                //// We only calculate a new danger point if the last danger point changed
                //if (Toolbox.Instance.GameManager.WeaponController.ShotPosition != lastShotPos)
                //{
                //    DangerPoint = CalculateDangerPoint(Toolbox.Instance.GameManager.WeaponController.ShotPositionShort);
                //    lastShotPos = Toolbox.Instance.GameManager.WeaponController.ShotPosition;
                //    Debug.LogWarning(ObjectManager.gameObject.name + ": New Danger Detected!");
                //}

                // We chek for danger if the short-term shotPosition is not zero (that is, it haven't been erased yet)
                if (Toolbox.Instance.GameManager.Player.WeaponController.ShotPositionShort != Vector3.zero)
                {
                    // We update the danger point with the short-time shotPosition from the weapon
                    DangerPoint = CalculateDangerPoint(Toolbox.Instance.GameManager.Player.WeaponController.ShotPositionShort);
                }
                // if it has been erased, we set the dangerPoint to Vector3.zero too
                else
                {
                    // We erase the position of dangerPoint
                    DangerPoint = Vector3.zero;
                }
                               

                // We update the animal state checking for danger
                UpdateAnimalState();

                break;
            case AnimalStateEnum.Scared:
                // If the object is scared, we calculate the opposite point to the last shot performed
                PointToScape = Points.CalculateOppositePoint(ObjectManager.ObjectRigidbody.position, DirectionOfDanger, 
                    ObjectManager.PointsController.RadiusOfPointToCalculate);
                // We update the state of the animal so that it starts running away
                AnimalState = AnimalStateEnum.RunningAway;
                break;
            case AnimalStateEnum.RunningAway:
                // If the object is running away, we keep moving the object until is stopped
                if (ObjectManager.MovementController.MovementPhase != MovementController.MovementPhaseEnum.Stopping)
                {
                    ObjectManager.MovementController.Move(PointToScape);
                    // We show the running animation
                    ObjectManager.AnimController.Run(true);
                    //Debug.LogError(ObjectManager.gameObject.name + ": RUN ANIMAL, RUN!!!");
                }
                // If the object is stopped, we calm it down so that it behaves normally
                else
                {
                    // The state is to be calmed
                    AnimalState = AnimalStateEnum.Calm;

                    // We set the next task to move
                    AnimalTask = AnimalTasksEnum.Moving;

                    // We set a random time for a new task
                    SecondsToNewTask = Random.Range(0f, 5f);

                    // We reset the danger point
                    DangerPoint = Vector3.zero;
                    //Debug.LogWarning(ObjectManager.gameObject.name + ": Everything clear, calm down...");
                }
                break;
            default:
                break;
        }
    }
        
    /// <summary>
    /// Checks for danger and updates the state of the animal to Scared if true
    /// </summary>
    private void UpdateAnimalState()
    {
        // We check for danger
        if (CheckForDanger(RadiusOfDangerDetection, DangerPoint))
        {
            AnimalState = AnimalStateEnum.Scared;
            //Debug.LogError(ObjectManager.gameObject.name + ": Fuck, I'm scared!");
        }
    }

    /// <summary>
    /// Returns a point in world space in reference to the current object
    /// </summary>
    /// <param name="shotPosition"> The position in screen coordinates of the last shot </param>
    /// <returns> A point in world space in reference to the current object < /returns>
    private Vector3 CalculateDangerPoint(Vector3 shotPosition)
    {
        // We calculate a point in world coordinates in the z axis of the current object
        Vector3 auxPoint = new Vector3(shotPosition.x, shotPosition.y, (ObjectManager.ObjectRigidbody.position.z - Camera.main.transform.position.z));
        auxPoint = Camera.main.ScreenToWorldPoint(auxPoint);
        // We return the calculated point
        return auxPoint;

    }

    /// <summary>
    /// Returns true if the pointToCheck is inside the radiusToCheck (from our object position)
    /// </summary>
    /// <param name="radiusToCheck"> The radius that determines when a point can be a threat </param>
    /// <param name="pointToCheck"> The point to check for danger </param>
    /// <returns> True if the pointToCheck is inside the radiusToCheck </returns>
    private bool CheckForDanger(float radiusToCheck, Vector3 pointToCheck)
    {
        // The x and y position of the current object
        Vector3 auxPosition = new Vector3(ObjectManager.ObjectRigidbody.position.x, ObjectManager.ObjectRigidbody.position.y, pointToCheck.z);

        Debug.DrawLine(this.transform.position, auxPosition, Color.green);

        // We calculate the total direction of the danger
        DirectionOfDanger = (pointToCheck - auxPosition);

        // The distance from the object to the point to check
        float auxDistance = DirectionOfDanger.magnitude;        

        // We normalize the direction of danger
        DirectionOfDanger = DirectionOfDanger.normalized;

        Debug.DrawRay(this.transform.position, DirectionOfDanger*auxDistance, Color.red);

        // If the distance is close enough to our object, the object is in danger
        if (auxDistance < radiusToCheck)
        {
            return true;
        }
        // If the pointToCheck is too far, we are not in danger
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Scares the animal with a point in world coordinates
    /// </summary>
    /// <param name="pointToCheck"> The point in world coordinates that will be checked by the logic </param>
    public void ScareAnimal(Vector3 pointToCheck)
    {
        // We check that the animal is not runningAway
        if (animalState != AnimalStateEnum.RunningAway)
        {
            // We update the DangerPoint to the point passed in
            DangerPoint = pointToCheck;

            // We update the animal state checking for danger
            UpdateAnimalState(); 
        }
    }

    // Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn
    public void OnDrawGizmosSelected()
    {
        if (ObjectManager.AllowGizmos)
        {
            
            // Red for the radius of detection and the dangerPoint
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ObjectManager.ObjectRigidbody.position, RadiusOfDangerDetection);
            Gizmos.DrawWireSphere(DangerPoint, 1f);


            // Cyan for the point to scape
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(PointToScape, 1f);

            
        }
    }
}
