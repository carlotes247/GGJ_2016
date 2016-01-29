using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ReusableMethods;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// The controller in charge of displaying the different screens of the menu
/// </summary>
public class MenuController : MonoBehaviour {

    /// <summary>
    /// The definition of the different available screens
    /// </summary>
    public enum MenuScreensEnum
    {
        MainMenu,
        AudioMenu,
        LogicMenu,
        NoMenu
    }
    private MenuScreensEnum menuScreen;
    /// <summary>
    /// (Property) The actual menu screen that we are on (Read Only)
    /// </summary>
    public MenuScreensEnum MenuScreen { get { return this.MenuScreen; } }

    [SerializeField]
    private GameObject[] menuCanvasArray;
    /// <summary>
    /// (Property) The array of the diferent canvas components of the menu.
    /// Follow definition of MenuScreensEnum to know which order access. (Read Only)
    /// 0: Main Menu, 1: Volume
    /// </summary>
    public GameObject[] MenuCanvasArray { get { return this.menuCanvasArray; } }

    /// <summary>
    /// (Field) Flag that controls if the menu is open
    /// </summary>
    [SerializeField]
    private bool m_MenuOpen;
    /// <summary>
    /// (Property) Flag that controls if the menu is open
    /// </summary>
    public bool MenuOpen { get { return this.m_MenuOpen; } }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Sets the menu screen to show
    /// </summary>
    /// <param name="screenToChange"> The different screens available</param>
    public void SetMenuScreen (MenuScreensEnum screenToChange)
    {
        switch (screenToChange)
        {
            case MenuScreensEnum.MainMenu:
                Arrays.SetActiveAllArray(ref menuCanvasArray, false);
                this.MenuCanvasArray[0].SetActive(true);
                this.MenuCanvasArray[1].SetActive(true); break;
            case MenuScreensEnum.AudioMenu:
                Arrays.SetActiveAllArray(ref menuCanvasArray, false);
                this.MenuCanvasArray[0].SetActive(true);
                this.MenuCanvasArray[2].SetActive(true); break;
            case MenuScreensEnum.LogicMenu:
                Arrays.SetActiveAllArray(ref menuCanvasArray, false);
                this.MenuCanvasArray[0].SetActive(true);
                this.MenuCanvasArray[3].SetActive(true); break;
            case MenuScreensEnum.NoMenu:
                Arrays.SetActiveAllArray(ref menuCanvasArray, false);
                break;
            default:
                Debug.LogError("The index must be in the range of the MainScreensEnum");
                break;
        }
    }

    /// <summary>
    /// Sets the menu screen to show
    /// </summary>
    /// <param name="screenToChange"> The different screens available </param>
    public void SetMenuScreen(int screenToChange)
    {
        switch (screenToChange)
        {
            case (int) MenuScreensEnum.MainMenu:
                Arrays.SetActiveAllArray(ref menuCanvasArray, false);
                this.MenuCanvasArray[0].SetActive(true);
                this.MenuCanvasArray[1].SetActive(true);
                //Debug.Log("SetMenuScreen " + ((int) MenuScreensEnum.MainMenu).ToString());
                // We update the flag of menuOpen for other scripts to use
                m_MenuOpen = true;
                break;
            case (int) MenuScreensEnum.AudioMenu:
                Arrays.SetActiveAllArray(ref menuCanvasArray, false);
                this.MenuCanvasArray[0].SetActive(true);
                this.MenuCanvasArray[2].SetActive(true);
                break;
            case (int) MenuScreensEnum.LogicMenu:
                Arrays.SetActiveAllArray(ref menuCanvasArray, false);
                this.MenuCanvasArray[0].SetActive(true);
                this.MenuCanvasArray[3].SetActive(true);
                break;
            case (int) MenuScreensEnum.NoMenu:
                //Arrays.SetActiveAllArray(ref menuCanvasArray, false);
                this.MenuCanvasArray[0].SetActive(false);
                //Debug.Log("SetMenuScreen" + MenuScreensEnum.NoMenu.ToString());
                // We update the flag of menuOpen for other scripts to use
                m_MenuOpen = false;
                break;
            default:
                Debug.LogError("The index must be in the range of the MainScreensEnum");
                break;
        }
    }

    /// <summary>
    /// Pause the logic of the game and open the pause menu with it, depending of the bool passed in
    /// </summary>
    /// <param name="option"> True to pause, false to unpause</param>
    public void PauseGame(bool option)
    {
        // If the option is true...
        if (option)
        {
            // We open the menuScreen
            this.SetMenuScreen(MenuScreensEnum.MainMenu);
            // We pause the time of the game
            Toolbox.Instance.GameManager.GameTimeController.PauseTime(true);
            // We apply the snapshot of the game paused, lowpassing the music track
            Toolbox.Instance.GameManager.AudioController.LowpassMusicTrack(true);

            // We update the flag of menuOpen for other scripts to use
            m_MenuOpen = true;

            if (Toolbox.Instance.GameManager.AllowDebugCode)
            {
                Debug.Log("Menu Paused!"); 
            }
        }
        // If the option is false...
        else
        {
            // We close the menu screen
            this.SetMenuScreen(MenuScreensEnum.NoMenu);
            // We resume the timeScale of the game
            Toolbox.Instance.GameManager.GameTimeController.PauseTime(false);
            // We apply the snapshot of the game unpaused, removing the lowpass filter from the music track
            Toolbox.Instance.GameManager.AudioController.LowpassMusicTrack(false);

            // We update the flag of menuOpen for other scripts to use
            m_MenuOpen = false;

            if (Toolbox.Instance.GameManager.AllowDebugCode)
            {
                Debug.Log("Menu Unpaused!");

            }
        }
    }    

    /// <summary>
    /// Exits the application
    /// </summary>
    public void QuitGame()
    {
        //If we are in the editor...
#if UNITY_EDITOR
        //... we exit playmode
        EditorApplication.isPlaying = false;
#else
        //... if not, we stop execution
        Application.Quit();
#endif
    }

}
