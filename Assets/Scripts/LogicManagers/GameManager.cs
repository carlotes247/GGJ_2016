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
using UnityEngine.UI;

/// <summary>
/// This script will take charge of managing the game
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Attributes

    public int coolValue = 100;

    /// Value to allow debug code
    // Field
    [SerializeField]
    private bool allowDebugCode;
    // Property
    public bool AllowDebugCode { get { return this.allowDebugCode; } set { this.allowDebugCode = value; } }

    /// The exposed scoreController so that every other script can interact with it
    // Field
    [SerializeField]
    private ScoreController scoreController;
    // Property
    public ScoreController ScoreController { get { return this.scoreController; } set { this.scoreController = value; } }

    /// The exposed HUD Canvas 
    // Field
    [SerializeField]
    private HUDController hudController;
    // Property
    public HUDController HudController { get { return this.hudController; } set { this.hudController = value; } }

    /// The exposed InputController so that we can control the input from the user
    // Field
    [SerializeField]
    private InputController inputController;
    // Property
    public InputController InputController { get { return this.inputController; } set { this.inputController = value; } }

    // [DEPRECATED] Use now instead Player.WeaponController
    ///// The exposed WeaponController
    //// Field
    //[SerializeField]
    //private WeaponController weaponController;
    //// Property
    //private WeaponController WeaponController { get { return this.weaponController; } set { this.weaponController = value; } }

    [SerializeField]
    private PlayerManager player;
    /// <summary>
    /// (Property) The main player
    /// </summary>
    public PlayerManager Player { get { return this.player; } }

    /// <summary>
    /// The game logic controller
    /// </summary>
    public GameLogicController gameLogicController;

    [SerializeField]
    private CoroutineController coroutineController;
    /// <summary>
    /// (Property) The CoroutineController that provides tools for coroutines
    /// </summary>
    public CoroutineController CoroutineController { get { return this.coroutineController; } set { this.coroutineController = value; } }

    [SerializeField]
    private GameTimeController gameTimeController;
    /// <summary>
    /// (Property) The GameTimeController that controls the timeScale logic
    /// </summary>
    public GameTimeController GameTimeController { get { return this.gameTimeController; } }

    [SerializeField]
    private AudioController audioController;
    /// <summary>
    /// (Property) The AudioController that controls the logic of the audio ingame
    /// </summary>
    public AudioController AudioController { get { return this.audioController; } }

    /// <summary>
    /// (Field) The MenuController that controls the logic of the In-Game Menu
    /// </summary>
    [SerializeField]
    private MenuController m_MenuController;
    /// <summary>
    /// (Property) The MenuController that controls the logic of the In-Game Menu
    /// </summary>
    public MenuController MenuController { get { return this.m_MenuController; } }

    /// <summary>
    /// (Field) The GameLevelController
    /// </summary>
    [SerializeField]
    private GameLevelController m_GameLevelController;
    /// <summary>
    /// (Property) The GameLevelController
    /// </summary>
    public GameLevelController GameLevelController { get { return this.m_GameLevelController; } }


    #endregion

    #region MainFunctions

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        ScoreController = GetComponent<ScoreController>();
        HudController = GetComponent<HUDController>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    public void TestAlive()
    {
        Debug.Log("GameMAnager Alive!");
    }

}
