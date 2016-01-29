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

/*! \file Toolbox.cs
 *  \brief The toolbox is a singleton, but it improves upon the concept.
 * 
 *  The toolbox is a singleton, before anything else. But it improves upon the concept.
 *  Basically this encourages better coding practices, such as reducing coupling and unit testing.
 *  
 *  Implementation extracted from http://wiki.unity3d.com/index.php/Toolbox
 *  
 * The toolbox creates the Game Manager and instantiates it as a child, giving it all the benefits from a Singleton!
 * */

using UnityEngine;


/// <summary>
/// The toolbox is a singleton, but it improves upon the concept. (access the Instance)
/// </summary>
public class Toolbox : Singleton<Toolbox> {
	protected Toolbox () {} // guarantee this will be always a singleton only - can't use the constructor!

    [SerializeField]
    private GameObject gameManagerObject;
    /// <summary>
    /// The gameManager to instantiate [DON'T ACCESS]
    /// </summary>
    public GameObject GameManagerObject { get { return this.gameManagerObject; } set { this.gameManagerObject = value; } }

    [SerializeField]
    private GameManager gameManager;
    /// <summary>
    /// The GameManager of the game. It contains the controllers of the logic of the game
    /// </summary>
    public GameManager GameManager { get { return this.gameManager; } set { this.gameManager = value; } }


    /// The variable that will be the direct child of the toolbox and benefit from all its properties.
    //private GameObject directChild;

    // I think that my global var is for creating whatever object we want with the toolbox
    //public GameObject myGlobalVar;
 
	//public Language language = new Language();
 
	void Awake () {
		// Your initialization code here
        //Debug.Log("Toolbox launched");
        // The toolbox will create the instance of the gameManager! Awesome!
	}

    public void Start()
    {
        // We ensure that the gameManager is not already instantiated
        if (GameManager == null)
        {
            //GameManagerObject = Instantiate(GameManagerObject) as GameObject;
            GameManagerObject = Instantiate(Resources.Load("GameManager")) as GameObject;
            //gameManager = Instantiate(Resources.Load("GameManager")) as GameObject;
            //Debug.Log("Game Manager instantiated");
            GameManagerObject.transform.parent = this.transform;            
            GameManager = GameManagerObject.GetComponent<GameManager>();
            // We make sure that the GameManagerObject is always active when the game starts
            GameManagerObject.SetActive(true);
            //Debug.Log("Game Manager is now a child of Toolbox");
            //// We create the directChild in the scene
            //directChild = new GameObject("DirectChild");
            //// We set the directChild as a child in the inspector, once the toolbox has been loaded
            //directChild.transform.SetParent(this.transform);
            //// We then instantiate the gameManager as the child of the Toolbox
            //directChild = Instantiate(gameManager);
        }

    }
 
	// (optional) allow runtime registration of global objects
	/*static public T RegisterComponent<T> () {
        //return Instance.GetComponent<T>();
		return Instance.GetOrAddComponent<T>();
	}*/
}
 
/*[System.Serializable]
public class Language {
	public string current;
	public string lastLang;
}*/