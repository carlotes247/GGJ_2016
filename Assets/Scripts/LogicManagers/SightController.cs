using UnityEngine;
using System.Collections;
using ReusableMethods;

/// <summary>
/// Controller that will inform if the target is on sight
/// </summary>
public class SightController : MonoBehaviour
{

    /// <summary>
    /// (Field) The ObjectManager to access all of the components of this object
    /// </summary>
    [SerializeField]
    private ObjectManager m_ObjectManager;

    /// <summary>
    /// (Field) Number of degrees, centred on forward, for the enemy see.
    /// </summary>
    [SerializeField, Range(0f, 360f)]
    private float m_FieldOfViewAngle = 110f;

    /// <summary>
    /// (Field) The radius from where the object is sighting
    /// </summary>
    [SerializeField, Range(0f, 25f)]
    private float m_RadiusOfSight;
    /// <summary>
    /// (Property) The radius from where the object is sighting
    /// </summary>
    public float RadiusOfSight { get { return this.m_RadiusOfSight; } }

    /// <summary>
    /// (Field) Whether or not the objective is currently sighted.
    /// </summary>
    private bool m_ObjectInSight;
    /// <summary>
    /// (Property) Whether or not the objective is currently sighted.
    /// </summary>
    public bool ObjectInSight { get { return this.m_ObjectInSight; } }

    // <summary>
    /// (Field) The current distance to the objective
    /// </summary>
    private float m_DistanceToObject;

    /// <summary>
    /// (Field) The vector direction to the objective
    /// </summary>
    private Vector3 m_DirectionToObject;


    /// <summary>
    /// (Field) The angle to the objective
    /// </summary>
    private float m_AngleToObject;

    /// <summary>
    /// (Field) The Raycast of the object
    /// </summary>
    private RaycastHit m_EnemyRaycast;




    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        // We get the object manager
        m_ObjectManager = GetComponent<ObjectManager>();
    }

    /// <summary>
    /// Sights an objective depending on a specified tag
    /// </summary>
    /// <param name="objectivePositon"> The position of the objective</param>
    /// <param name="sightRadius"> The maximum reach of the sight</param>
    /// <param name="tagToCompare"> The tag we want to compare against</param>
    public void Sight(Vector3 objectivePositon, float sightRadius, string tagToCompare)
    {
        // We update the vector from the current object to the objective and store the angle between it and forward
        m_DirectionToObject = Vectors.CalculateDirection(m_ObjectManager.ObjectRigidbody.position, objectivePositon).normalized;
        m_AngleToObject = Vector3.Angle(m_DirectionToObject, transform.forward);

        // By default, the objective is not seen
        m_ObjectInSight = false;

        // If the angle between forward and where the player is, is less than half the angle of view...
        if (m_AngleToObject < m_FieldOfViewAngle * 0.5f)
        {
            //Debug.Log(this.gameObject.name + "Player in Angle...");
            // ... and if a raycast towards the objective hits something...
            if (Physics.Raycast(m_ObjectManager.ObjectRigidbody.position + transform.up, m_DirectionToObject,
                out m_EnemyRaycast, sightRadius))
            {
                // ... and if the raycast hits the objective...
                if (m_EnemyRaycast.collider.gameObject.CompareTag(tagToCompare))
                {
                    //Debug.Log(this.gameObject.name + "Player on Sight!");
                    // ... the objective is in sight.
                    m_ObjectInSight = true;
                }

            }

        }
    }

    // Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn
    public void OnDrawGizmosSelected()
    {

        if (m_ObjectManager.AllowGizmos)
        {
            // Magenta for Sight radius
            Gizmos.color = Color.magenta;
            //Gizmos.DrawWireSphere(m_ObjectManager.ObjectTransform.position, m_RadiusOfSight);
            //Gizmos.DrawFrustum(m_ObjectManager.ObjectTransform.position, m_FieldOfViewAngle, m_RadiusOfSight, 0f, 1f);             
            // Code for the field of view
            Quaternion leftRayRotation = Quaternion.AngleAxis(-m_FieldOfViewAngle * 0.5f, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(m_FieldOfViewAngle * 0.5f, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;
            Gizmos.DrawRay(transform.position, leftRayDirection * m_RadiusOfSight);
            Gizmos.DrawRay(transform.position, rightRayDirection * m_RadiusOfSight);
        }
    }
}
