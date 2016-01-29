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
#if UNITY_EDITOR
using UnityEditor;
#endif


[ExecuteInEditMode]
/// <summary>
/// Updates the position of the AIController.PointsToGo only in editor mode.
/// They will be local to the object while in Editor and not in play mode
/// </summary>
public class PointsController : MonoBehaviour
{
    // We execute the code in unity editor (we also need [ExecuteInEditMode] at the top of the script)
    //#if UNITY_EDITOR
    #region Fields&Properties
    /// The Enemy Manager so that this script has acces to the rest of components
    // Field
    [SerializeField]
    private ObjectManager objectManager;
    // Property
    public ObjectManager ObjectManager { get { return this.objectManager; } set { this.objectManager = value; } }

    /// <summary>
    /// The field of the array of points
    /// </summary>
    [SerializeField]
    private Vector3[] pointsToGo;
    /// <summary>
    /// The array of points to go
    /// </summary>
    public Vector3[] PointsToGo { get { return this.pointsToGo; } set { this.pointsToGo = value; } }

    /// <summary>
    /// Aun auxiliary copy of the points to go, to nor modify the original points to go
    /// </summary>
    [SerializeField]
    private Vector3[] auxPointsToGo;

    [SerializeField, Range(0f, 100f)]
    private float radiusOfPointToCalculate;
    /// <summary>
    /// (Property) The radius that determines the distance to the point to move when we are getting a new point
    ///  For example, if the AIAnimal is scared.
    /// </summary>
    public float RadiusOfPointToCalculate { get { return this.radiusOfPointToCalculate; } set { this.radiusOfPointToCalculate = value; } }

    //[SerializeField]
    //private bool alreadyStarted;

    //[SerializeField]
    //private bool stopControlling; 

    #endregion

    // Awake is called when the script instance is being loaded
    public void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        //if (!alreadyStarted && !stopControlling)
        //{
        //    auxPointsToGo = this.PointsToGo; 
        //}
        //alreadyStarted = true;

        
    }

    // This function is called when the object becomes enabled and active
    public void OnEnable()
    {
        PointsToGoLogic();
    }


    // Update is called once per frame
    void Update()
    {
        //// Only execute while the game is not playing
        //if (!EditorApplication.isPlaying && alreadyStarted && !stopControlling)
        //{

        //    for (int i = 0; i < ObjectManager.AIController.PointsToGo.Length; i++)
        //    {
        //        ObjectManager.AIController.PointsToGo[i] = auxPoints[i] + ObjectManager.transform.position;
        //    }

        //}

        // Only execute while the game is not playing


        //for (int i = 0; i < this.auxPointsToGo.Length; i++)
        //{
        //    this.PointsToGo[i] = auxPointsToGo[i] + ObjectManager.transform.position;
        //}

    }    

    //#endif

    /// <summary>
    /// Converts the array of PointsToGo to the object local space
    /// </summary>
    private void PointsToGoLogic ()
    {
        // We resize the length of pointsToGo if the size is different from auxPointsToGo
        if (PointsToGo.Length != auxPointsToGo.Length)
        {
            // We resize pointsToGo to the auxPointsToGo
            System.Array.Resize<Vector3>(ref pointsToGo, auxPointsToGo.Length);
        }

        //System.Array.Copy(auxPointsToGo, PointsToGo, auxPointsToGo.Length);        

        // Every point to go is the actual position of the object plus the offset we want
        for (int i = 0; i < PointsToGo.Length; i++)
        {
            // Every point is local to the object
            PointsToGo[i] = ObjectManager.transform.position + auxPointsToGo[i];
        }
    }    

    // Sent to all game objects before the application is quit
    public void OnApplicationQuit()
    {
        //for (int i = 0; i < ObjectManager.AIController.PointsToGo.Length; i++)
        //{
        //    ObjectManager.AIController.PointsToGo[i] = auxPoints[i];
        //}
        
    }

    /// We draw a gizmo for every pointToGo
    void OnDrawGizmosSelected()
    {

        if (ObjectManager.AllowGizmos)
        {
            // Yellow for all the PointsToGo
            Gizmos.color = Color.yellow;
            for (int i = 0; i < PointsToGo.Length; i++)
            {
                Gizmos.DrawWireSphere(PointsToGo[i], ObjectManager.MovementController.StopRadius);
            }

            // Green for the radius of point to calculate
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(ObjectManager.ObjectRigidbody.position, RadiusOfPointToCalculate);

        }
    }

    
    #if UNITY_EDITOR
    // OnDrawGizmos is executing constantly in the editor
    public void OnDrawGizmos()
    {
        // I am going to write the logic of the editor update here
        // It will not execute in the build, take in mind!

        // We only execute this code if the editor is not in play mode
        if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            PointsToGoLogic();

        }

    }
    #endif

}
