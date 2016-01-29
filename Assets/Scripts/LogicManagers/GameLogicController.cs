using UnityEngine;
using System.Collections;
using ReusableMethods;

/// <summary>
/// The Game Logic Controller
/// </summary>
public class GameLogicController : MonoBehaviour {

    #region Fields&Properties
    /// <summary>
    /// The timer controller of the game logic manager
    /// </summary>
    public TimerController timerControllerSpawning;

    /// <summary>
    /// The timer that controls the points
    /// </summary>
    private TimerController timerControllerPoints;

    /// <summary>
    /// The amount of time until next spawn
    /// </summary>
    public float secondsNextSpawn;

    /// <summary>
    /// The enemyManager to instantiate
    /// </summary>
    public GameObject enemy;

    /// <summary>
    /// The number of enemies to store
    /// </summary>
    public int enemyArraySize;

    /// <summary>
    /// The pool of enemies
    /// </summary>
    public GameObject[] enemyArray;

    /// <summary>
    /// The bool that shows if the game is lost
    /// </summary>
    public bool gameLost;

    /// <summary>
    /// The array of objects we are handling
    /// </summary>
    [SerializeField]
    private GameObject[] m_ObjectArray;

    /// <summary>
    /// The GameObject containing all the enemies
    /// </summary>
    [SerializeField]
    private GameObject m_EnemyParentObject;
    #endregion

    // Use this for initialization
    void Start () {
        //// We create the pool of enemies at the start of the game
        //CreateEnemyArray();
        //// We set how long will it take to spawn an enemy
        //secondsNextSpawn = Random.Range(3f, 5f);

        //// We instantiate another timer for the points
        //timerControllerPoints = new TimerController();

        // We get all the children from the object having the enemies
        GetChildrenArray(m_EnemyParentObject);
        // We deactivate all the enemies
        Arrays.SetActiveAllArray(ref m_ObjectArray, false);
	}
	
	// Update is called once per frame
	void Update () {
     //   // We spawn an enemy every 2 seconds in a calculated position
	    //if (timerControllerSpawning.GenericCountDown(secondsNextSpawn))
     //   {
     //       SpawnEnemy(CalculateSpawnPosition());
     //       // We calculate a new spawning time
     //       secondsNextSpawn = Random.Range(3f, 9f);
     //   }

     //   // Add one point every second
     //   ScoreLogic();

     //   if (gameLost)
     //   {
     //       // We show the loosing screen
     //       Toolbox.Instance.GameManager.HudController.loosingCanvas.enabled = true;
     //       // We pause the game
     //       Time.timeScale = 0f;
     //       // If the player clicks, we restart the game
     //       if (Input.GetMouseButtonDown(0))
     //       {
     //           // We set the bool to false
     //           gameLost = false;
     //           // We deactivate all enemies
     //           DeactivateAllPool();
     //           // We set to false the canvas
     //           Toolbox.Instance.GameManager.HudController.loosingCanvas.enabled = false;
     //           // We reset the score
     //           Toolbox.Instance.GameManager.ScoreController.GameScore = 0f;
     //           // We unpause the game
     //           Time.timeScale = 1f;
     //       }
     //   }
    }

    private void ScoreLogic ()
    {
        // Every second we update one point the score
        if (timerControllerPoints.GenericCountDown(1f))
        {
            Toolbox.Instance.GameManager.ScoreController.UpdateScore(+1);
        }
    }

    /// <summary>
    /// Calculates a random offscreen spawn pos
    /// </summary>
    /// <returns> A Vector3 offscreen</returns>
    private Vector3 CalculateSpawnPosition ()
    {
        // The spawn pos to return
        Vector3 spawnPos;

        // Probability of spawning in the right side of the screen
        if (Probabilities.GetResultProbability(0.2f)) {
            spawnPos = new Vector3(Screen.width, Random.Range(0f, Screen.height), 1f);
            spawnPos = Camera.main.ScreenToWorldPoint(spawnPos);
            return spawnPos;
        }
        // If not, we spawn in the left side
        else
        {
            spawnPos = new Vector3(0f, Random.Range(0f, Screen.height), 1f);
            spawnPos = Camera.main.ScreenToWorldPoint(spawnPos);
            return spawnPos;
        }       
    }

    /// <summary>
    /// The function that spawns an enemy to a position in space
    /// </summary>
    /// <param name="spawnPosition"> The position to spawn the enemy (world coordinates)</param>
    private void SpawnEnemy(Vector3 spawnPosition)
    {
        // We go through all the array of enemies
        for (int i = 0; i < enemyArray.Length; i++)
        {
            // Because of the object pooling, we only want the objects that are deactivated
            if (!enemyArray[i].activeInHierarchy)
            {
                // We put them in the position to spawn                         
                enemyArray[i].transform.position = spawnPosition;
                // An prepare to move them to the player position
                enemyArray[i].GetComponent<EnemyManager>().PointsController.PointsToGo[0] = Toolbox.Instance.GameManager.Player.transform.position ;
                // We activate the object for the enemy to fly free
                enemyArray[i].SetActive(true);
                // And then, we break the for loop to only spawn once
                
                break;
            }
        }
    }

    /// <summary>
    /// Function to deactivate all the objects of the pool
    /// </summary>
    private void DeactivateAllPool ()
    {
        // We go through all the array of enemies
        for (int i = 0; i < enemyArray.Length; i++)
        {
            // Because of the object pooling, we only want the objects that are activated
            if (enemyArray[i].activeInHierarchy)
            {                
                // We deactivate the object
                enemyArray[i].SetActive(false);                
            }
        }
    }

    /// <summary>
    /// The function creates a pool of enemies
    /// </summary>
    void CreateEnemyArray()
    {
        if (enemyArraySize > 0)
        {
            this.enemyArray = new GameObject[enemyArraySize];
            for (int i = 0; i < enemyArraySize; i++)
            {
                enemyArray[i] = (GameObject)Instantiate(enemy);
                enemyArray[i].SetActive(false);
                enemyArray[i].transform.parent = this.transform;
            }
        }
    }

    /// <summary>
    /// Get all the children that a given gameObject has
    /// </summary>
    /// <param name="parentObject"> The gameObject containing the children to get</param>
    void GetChildrenArray (GameObject parentObject)
    {
        // The new size of the object array is the one of the parentObject
        m_ObjectArray = new GameObject[parentObject.transform.childCount];
        // We iterate the parent getting all of the childs into object array
        for (int i = 0; i < m_ObjectArray.Length; i++)
        {
            m_ObjectArray[i] = parentObject.transform.GetChild(i).gameObject;
        }
    }

    /// <summary>
    /// Function to Activate a number of objects in the ObjectArray
    /// </summary>
    /// <param name="numberToActivate"> The amount of enemies to activate</param>
    public void ActivateEnemies (int numberToActivate)
    {
        if (numberToActivate > m_ObjectArray.Length)
        {
            throw new UnityException("You can't activate more enemies that the ObjectArray has!");
        }

        for (int i = 0; i < numberToActivate; i++)
        {
            m_ObjectArray[i].SetActive(true);
        }
    }
}
