using UnityEngine;
using System.Collections;

public class GGJ_2016_Logic : MonoBehaviour {

    /// <summary>
    /// (Field) The position of the player during the main menu
    /// </summary>
    [SerializeField]
    private Vector3 m_PositionPlayerMainMenu;

    /// <summary>
    /// (Field) The initial pos of the player at the start of the game
    /// </summary>
    [SerializeField]
    private Vector3 m_PositionPlayerInitialGame;

    /// <summary>
    /// The object mainMenu
    /// </summary>
    [SerializeField]
    private GameObject m_MainMenuGameObj;

	// Use this for initialization
	void Start () {
        // When the game starts, the player is seeing the main menu
        SetPlayerInMainMenu();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Sets the player looking at the main Menu and activates the Main Menu
    /// </summary>
    public void SetPlayerInMainMenu ()
    {
        // We activate the main menu
        m_MainMenuGameObj.SetActive(true);

        // We remove the control of the player from the controller (ririgidbody will do good)
        Toolbox.Instance.GameManager.Player.MovementController.TypeOfMovement = MovementController.TypeOfMovementEnum.Rigidbody;
        // We set the position of the player to the desired one
        Toolbox.Instance.GameManager.Player.ObjectTransform.position = m_PositionPlayerMainMenu;
        // We set the rotation of the player/camera to zero
        //Toolbox.Instance.GameManager.InputController.RotationCamera = Vector2.zero;
        Toolbox.Instance.GameManager.InputController.RestartRotationCameraPlayer();


    }

    /// <summary>
    /// Sets the player position in the initial game position
    /// </summary>
    public void SetPlayerInitialGamePos ()
    {
        // We deactivate the main menu
        m_MainMenuGameObj.SetActive(false);

        // We give the control of the player from the controller
        Toolbox.Instance.GameManager.Player.MovementController.TypeOfMovement = MovementController.TypeOfMovementEnum.InputController;
        // We set the position of the player to the desired one
        Toolbox.Instance.GameManager.Player.ObjectTransform.position = m_PositionPlayerInitialGame;
        // We set the rotation of the player/camera to zero
        Toolbox.Instance.GameManager.InputController.RestartRotationCameraPlayer();
    }

    /// <summary>
    /// We restart the game
    /// </summary>
    public void RestartGame ()
    {
        // We close the options menu of the game (if open)
        Toolbox.Instance.GameManager.MenuController.SetMenuScreen(MenuController.MenuScreensEnum.NoMenu);

        // We reload the scene
        Toolbox.Instance.GameManager.GameLevelController.LoadScene(0);

        // We set the player back in the initialGame Pos
        SetPlayerInitialGamePos();
    }

    /// <summary>
    /// We open again the main Menu
    /// </summary>
    public void GoToMainMenu()
    {
        // We close the options menu of the game (if open)
        Toolbox.Instance.GameManager.MenuController.SetMenuScreen(MenuController.MenuScreensEnum.NoMenu);

        // We reload the scene
        Toolbox.Instance.GameManager.GameLevelController.LoadScene(0);

        // We set the player back in the mainMenu Pos
        SetPlayerInMainMenu();
    }
}
