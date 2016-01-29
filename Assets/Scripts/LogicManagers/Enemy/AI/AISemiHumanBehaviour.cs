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
using ReusableMethods;


/// <summary>
/// Semi Human AI, it will react to the input of the player
/// </summary>
public class AISemiHumanBehaviour : AIBehaviour
{
    #region Fields&Properties
    [SerializeField]
    private ObjectManager objectManager;
    /// <summary>
    /// (Property) The object manager of the current GameObject. Contains all the controllers
    /// </summary>
    public ObjectManager ObjectManager { get { return this.objectManager; } }

    /// <summary>
    /// The definition of the enumeration of personalities of the human
    /// </summary>
    public enum PersonalityEnum
    {
        Coward,
        Prudent,
        Aggresive,
    }
    /// <summary>
    /// (Field) The actual personality of the human
    /// </summary>
    [SerializeField]
    private PersonalityEnum m_Personality;
    /// <summary>
    /// (Property) The actual personality of the human
    /// </summary>s
    public PersonalityEnum Personality { get { return this.m_Personality; } set { this.m_Personality = value; } }

    /// <summary>
    /// The animal behaviour (DIRTY CODE)
    /// </summary>
    private AIAnimalBehaviour m_AIAnimal;

    ///// <summary>
    ///// (Field) Number of degrees, centred on forward, for the enemy see.
    ///// </summary>
    //[SerializeField, Range(0f, 360f)]
    //private float m_FieldOfViewAngle = 110f;

    /// <summary>
    /// (Field) Whether or not the player is currently sighted.
    /// </summary>
    [SerializeField]
    private bool m_PlayerInSight { get { return ObjectManager.SightController.ObjectInSight; } }

    /// <summary>
    /// (Field) Flag to control if the player was seen at least once
    /// </summary>
    [SerializeField]
    private bool m_PlayerSeen;

    /// <summary>
    /// (Field) The current distance to the player
    /// </summary>
    private float m_DistanceToPlayer;    

    /// <summary>
    /// (Field) The radius use to detect a threat
    /// </summary>
    private float m_DangerRadius { get { return ObjectManager.SightController.RadiusOfSight; } }

    /// <summary>
    /// (Field) The vector direction to the player
    /// </summary>
    private Vector3 m_DirectionToPlayer;

    ///// <summary>
    ///// (Field) The angle to the player
    ///// </summary>
    //private float m_AngleToPlayer;

    ///// <summary>
    ///// (Field) The Raycast of the object
    ///// </summary>
    //private RaycastHit m_EnemyRaycast;

    ///// <summary>
    ///// (Field) The point to move
    ///// </summary>
    //private Vector3 m_PointToMove;

    /// <summary>
    /// (Field) Flag controlling if the semi-human is covered
    /// </summary>
    [SerializeField]
    private bool m_CurrentCovered;
    /// <summary>
    /// (Field) The CoverManager of the cover we want to reach
    /// </summary>
    private GameObject m_CoverToGo;

    
    #endregion

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        m_AIAnimal = this.gameObject.GetComponent<AIAnimalBehaviour>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Semi Human Behaviour, reacting to player's input
    /// </summary>
    public override void Behave()
    {
        // We update the current distance to the player
        //m_DistanceToPlayer = Vector3.Distance(Toolbox.Instance.GameManager.Player.transform.position,
        //  ObjectManager.ObjectRigidbody.position);
        m_DirectionToPlayer = Vectors.CalculateDirection(ObjectManager.ObjectRigidbody.position,
            Toolbox.Instance.GameManager.Player.ObjectPosition);

        // If the player is inside our DangerRadius...
        if (Vectors.CheckDistance(m_DirectionToPlayer, m_DangerRadius))
        {
            // If the player has never been seen ...
            if (!m_PlayerSeen)
            {
                // We run the animal beaviour one more frame
                m_AIAnimal.Behave();

                // We sight the player ...
                ObjectManager.SightController.Sight(Toolbox.Instance.GameManager.Player.ObjectTransform.position, 
                    m_DangerRadius, "Player");
                // If we saw the player... 
                if (m_PlayerInSight)
                {
                    // ... we update the playerSeen flag, now we will not look for him again until we forget we saw him
                    m_PlayerSeen = true; 
                }
            }
            // If the player has been seen at least once (the enemy remembers)... 
            else
            {
                // We update the animator to animate in purple
                ObjectManager.AnimController.Interact();

                // We behave depending on the personality active
                switch (m_Personality)
                {
                    case PersonalityEnum.Coward:
                        CowardLogic();
                        break;
                    case PersonalityEnum.Prudent:
                        PrudentLogic();
                        break;
                    case PersonalityEnum.Aggresive:
                        AggressiveLogic();
                        break;
                    default:
                        break;
                }
            }
        }
        // If the player is out of our dangerRadius...
        else
        {
            // We uncover
            m_CurrentCovered = false;
            // (It's not needed anymore) We stop the possible control of the rotation 
            //ObjectManager.RotationController.StopControlRotation();
            // We behave like an animal
            m_AIAnimal.Behave();
        }
        
    }

    private void SemiHumanLogic()
    {
        //// We update the current distance to the player
        //m_DistanceToPlayer = Vector3.Distance(Toolbox.Instance.GameManager.Player.transform.position,
        //    ObjectManager.ObjectRigidbody.position);

        //// If the player is inside our DangerRadius and the player is not in sight already...
        //if (m_DistanceToPlayer < m_DangerRadius /*&& m_PlayerInSight == false*/)
        //{
        //    //Debug.Log(this.gameObject.name + "Player in Radius...");

        //    // By default the player is not in sight.
        //    //m_PlayerInSight = false;

        //    // We update the vector from the enemy to the player and store the angle between it and forward
        //    m_DirectionToPlayer = Vectors.CalculateDirection(ObjectManager.ObjectRigidbody.position, Toolbox.Instance.GameManager.Player.transform.position).normalized;
        //    m_AngleToPlayer = Vector3.Angle(m_DirectionToPlayer, transform.forward);

        //    // If the angle between forward and where the player is, is less than half the angle of view...
        //    if (m_AngleToPlayer < m_FieldOfViewAngle * 0.5f)
        //    {
        //        //Debug.Log(this.gameObject.name + "Player in Angle...");
        //        // ... and if a raycast towards the player hits something...
        //        if (Physics.Raycast(ObjectManager.ObjectRigidbody.position + transform.up, m_DirectionToPlayer,
        //            out m_EnemyRaycast, m_DangerRadius))
        //        {
        //            // ... and if the raycast hits the player...
        //            if (m_EnemyRaycast.collider.gameObject.CompareTag("Player"))
        //            {
        //                //Debug.Log(this.gameObject.name + "Player on Sight!");
        //                // ... the player is in sight.
        //                //m_PlayerInSight = true;



        //                // We update the animator to animate in purple
        //                ObjectManager.AnimController.Interact();

        //                switch (m_Personality)
        //                {
        //                    case PersonalityEnum.Coward:
        //                        CowardLogic();
        //                        break;
        //                    case PersonalityEnum.Prudent:
        //                        PrudentLogic();
        //                        break;
        //                    case PersonalityEnum.Aggresive:
        //                        AggressiveLogic();
        //                        break;
        //                    default:
        //                        break;
        //                }

        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    //Debug.Log(this.gameObject.name + "Player outside radius :(");
        //    //m_PlayerInSight = false;
        //}

        //// If the player is not in radius...
        //if (!m_PlayerInSight)
        //{
        //    // we behave like animals
        //    m_AIAnimal.Behave();
        //}
    }

    /// <summary>
    /// The logic of the coward personality. Will scape from the player like an animal
    /// </summary>
    private void CowardLogic ()
    {
        // The object is going to run away, behaving like an animal
        //// We set the direction of danger in the animal AI to the direction to the player
        //m_AIAnimal.DirectionOfDanger = m_DirectionToPlayer.normalized;
        //// We scare the animal
        //m_AIAnimal.AnimalState = AIAnimalBehaviour.AnimalStateEnum.Scared;

        // We scare the animal with the position of the player
        m_AIAnimal.ScareAnimal(Toolbox.Instance.GameManager.Player.ObjectTransform.position);

        // We behave like an animal
        m_AIAnimal.Behave();
        
        // We completely stop the object
        //ObjectManager.MovementController.TotalStop();
    }

    /// <summary>
    /// The logic of the Prudent personality. Will search for the closest cover to go and shoot, but if there is none, will be coward
    /// </summary>
    private void PrudentLogic()
    {
        // We completely stop the object
        //ObjectManager.MovementController.TotalStop();

        // If the object is not covered...
        if (!m_CurrentCovered)
        {
            // ... We search for the closest cover
            m_CoverToGo = Vectors.SearchClosestObjectWihTag(ObjectManager.ObjectTransform.position, m_DangerRadius, "Cover");
            // ... If the search returned something...
            if (m_CoverToGo != null)
            {
                // We go there
                ObjectManager.MovementController.Move(m_CoverToGo.transform.position);
                // The object is now covered
                m_CurrentCovered = true;
            }
            // If the player gets covered
            else
            {
                // If it didn't, the enemy gets scared and leave
                CowardLogic();
            }
            
            
        }
        // If the player is covered already..
        else
        {
            // We keep where the closest cover is
            ObjectManager.MovementController.Move(m_CoverToGo.transform.position);

            // We shoot at the player
            //ObjectManager.WeaponController.Shoot(ObjectManager.WeaponController.WeaponTransform.position, m_DirectionToPlayer.normalized);
            AggressiveLogic();
        }
    }

    /// <summary>
    /// Logic Agressive personality. Will immediately attack the player.
    /// </summary>
    private void AggressiveLogic()
    {
        // We completely stop the object
        //ObjectManager.MovementController.TotalStop();

        // We look at the player (we add one unit to avoid shooting at the floor)
        ObjectManager.RotationController.LookAt(Toolbox.Instance.GameManager.Player.ObjectPosition);

        // We shoot at the player
        ObjectManager.WeaponController.Shoot(ObjectManager.WeaponController.WeaponTransform.position , m_DirectionToPlayer.normalized);
    }

    //private void SightLogic()
    //{
    //    // We update the vector from the enemy to the player and store the angle between it and forward
    //    m_DirectionToPlayer = Vectors.CalculateDirection(ObjectManager.ObjectRigidbody.position, Toolbox.Instance.GameManager.Player.transform.position).normalized;
    //    m_AngleToPlayer = Vector3.Angle(m_DirectionToPlayer, transform.forward);

    //    // If the angle between forward and where the player is, is less than half the angle of view...
    //    if (m_AngleToPlayer < m_FieldOfViewAngle * 0.5f)
    //    {
    //        //Debug.Log(this.gameObject.name + "Player in Angle...");
    //        // ... and if a raycast towards the player hits something...
    //        if (Physics.Raycast(ObjectManager.ObjectRigidbody.position + transform.up, m_DirectionToPlayer,
    //            out m_EnemyRaycast, m_DangerRadius))
    //        {
    //            // ... and if the raycast hits the player...
    //            if (m_EnemyRaycast.collider.gameObject.CompareTag("Player"))
    //            {
    //                //Debug.Log(this.gameObject.name + "Player on Sight!");
    //                // ... the player is in sight.
    //                //m_PlayerInSight = true;
    //            }
    //        }
    //    }
    //}

    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    public void OnDrawGizmos()
    {
        if (ObjectManager.AllowGizmos)
        {
            // Yellow for player
            Gizmos.color = Color.white;
            Gizmos.DrawRay(ObjectManager.ObjectRigidbody.position, m_DirectionToPlayer * m_DistanceToPlayer);

            // Blue for forward vector
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(ObjectManager.ObjectRigidbody.position, transform.forward * m_DangerRadius);

          
        }
    }
}
