using UnityEngine;
using System.Collections;
using ReusableMethods;


/// <summary>
/// Controls all logic related to the game levels, how to transit them and store them
/// </summary>
public class GameLevelController : MonoBehaviour {

    [SerializeField]
    private GameObject[] gameLevels;
    /// <summary>
    /// (Property) The array of levels in the game
    /// </summary>
    public GameObject[] GameLevels { get { return this.gameLevels; } set { this.gameLevels = value; } }

    [SerializeField]
    private int indexCurrentLevel;
    /// <summary>
    /// (Property) The current level we are in
    /// </summary>
    public int IndexCurrentLevel { get { return this.indexCurrentLevel; } set { this.indexCurrentLevel = value; } }

	// Use this for initialization
	void Start () {
        // We set all the levels to false
        Arrays.SetActiveAllArray(ref gameLevels, false);
        // We start on the first level
        LoadGameLevel(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Load the next level in the array of game levels
    /// </summary>
    public void LoadNextLevel()
    {
        // We load current level +1
        LoadGameLevel(IndexCurrentLevel + 1);
    }

    /// <summary>
    /// Loads the previous level in the array of game levels
    /// </summary>
    public void LoadPreviousLevel()
    {
        // We load current level -1
        LoadGameLevel(IndexCurrentLevel - 1);
    }

    /// <summary>
    /// Loads the level specified by an index
    /// </summary>
    /// <param name="indexLevelToGo"> The index of the level we want to load </param>
    public void LoadGameLevel (int indexLevelToGo)
    {
        // We check if the indexToGo is not out of the range of levels
        if (indexLevelToGo >= GameLevels.Length || indexLevelToGo < 0)
        {
            //Debug.LogError("AAAAA");
            // If it is, we throw an exception
            throw new UnityException("The level " + indexLevelToGo.ToString() + " is out of the range of GameLevels!");            
        }

        // We check that the levelToLoad is not the same one we are
        if (indexLevelToGo != indexCurrentLevel)
        {

            // We activate the next level
            GameLevels[indexLevelToGo].SetActive(true);
            // We deactivate the current level
            GameLevels[indexCurrentLevel].SetActive(false);
            // Current level is now levelToGo
            IndexCurrentLevel = indexLevelToGo;

            
        }
        else
        {
            // If it is, we warn it in the editor
            Debug.LogWarning("You are loading the same level you are in with the GameLevelController!");
            // We deactivate the current level
            GameLevels[indexCurrentLevel].SetActive(false);
            // We activate the current level
            GameLevels[IndexCurrentLevel].SetActive(true);
        }

        

    }

}
