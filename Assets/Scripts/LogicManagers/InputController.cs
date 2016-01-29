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

/// <summary>
/// The Input Controller, controls the input devices and logic of the game
/// </summary>
public class InputController : MonoBehaviour {

    /// The pointer position in screen coordinates
    // Field 
    [SerializeField]
    private Vector3 screenPointerPos;
    // Property
    public Vector3 ScreenPointerPos { get { return this.screenPointerPos; } set { this.screenPointerPos = value; } }

    /// The Wiimote Input received by the computer
    // Field
    [SerializeField]
    private WiiMoteInput wiimoteInput;
    // Property
    public WiiMoteInput WiimoteInput { get { return this.wiimoteInput; } set { this.wiimoteInput = value; } }

    /// The input mode that is currently active
    // The enum to decide which control do we have
    public enum TypeOfInput
    {
        Mouse,
        WiiMote
    }
    // Field
    [SerializeField]
    private TypeOfInput inputType;
    // Property
    public TypeOfInput InputType
    {
        get { return this.inputType; }
        set { this.inputType = value; }
    }

    /// <summary>
    /// (Field) The timer of the object
    /// </summary>
    private TimerController m_Timer;

    /// <summary>
    /// (Field) The delay between the shots
    /// </summary>
    [SerializeField]
    private float m_DelayBetweenShots;
    /// <summary>
    /// (Property) The delay between the shots
    /// </summary>
    public float DelayBetweenShots { get { return this.m_DelayBetweenShots; } set { this.m_DelayBetweenShots = value; } }

    // Use this for initialization
    void Start () {
        //WiimoteInput.WiimoteInputLogic();

        // We set always mouse on android to avoid invoking the UniWii dll
#if UNITY_ANDROID
        this.InputType = TypeOfInput.Mouse;
#endif
        // We add a timer for the input
        m_Timer = this.gameObject.AddComponent<TimerController>();
        m_Timer.ObjectLabel = "InputController Timer";

        // We check the type of input currently connected
        CheckInputType();

	}
	
	// Update is called once per frame
	void Update () {
        // We check the type of input for any changes
        CheckInputType();
        // We update the values of the screenPointerPos
        UpdateScreenPointerPos();
        // We draw the pointer in the specified pos
        DrawScreenPointer(ScreenPointerPos);
        // We check if the user press any buttons
        CheckUserInput();
	}

    /// Check the type of the Input
    void CheckInputType()
    {
        //if (WiimoteInput.WiiMoteCount > 0)
        //{
        //    InputType = TypeOfInput.WiiMote;
        //}
        //else
        //{
        //    InputType = TypeOfInput.Mouse;
        //}

    }

    /// Update ScreenPointer position
    void UpdateScreenPointerPos ()
    {
        switch (InputType)
        {
            case TypeOfInput.Mouse:
                ScreenPointerPos = Input.mousePosition;
                break;
            case TypeOfInput.WiiMote:
                // We run the logic of the wiimote, to populate all the variables
                //WiimoteInput.WiimoteInputLogic();
                // We get the position of the wiimote
                ScreenPointerPos = WiimoteInput.CursorPosition;
                break;
            default:
                Debug.LogError("Type of Input not recognized!");
                break;
        }
    }

    /// The function in charge of Drawing on Screen a pointer (can be used by the any source of input, including mouse, Wiimote, PSMove, etc)
    void DrawScreenPointer(Vector3 values)
    {
        Toolbox.Instance.GameManager.HudController.InGameCursor.transform.position = new Vector3(values.x, values.y, 0);        
    }

    void DrawScreenPointer(int value_x, int value_y)
    {
        Toolbox.Instance.GameManager.HudController.InGameCursor.transform.position = new Vector3(value_x, value_y, 0);
    }

    /// The function to check User the user Input
    private void CheckUserInput()
    {
        if (Input.GetMouseButtonDown(0) || WiimoteInput.ButtonB && (m_Timer.GenericCountDown(m_DelayBetweenShots)) )
        {
            if (!Toolbox.Instance.GameManager.MenuController.MenuOpen)
            {
                // Toolbox.Instance.GameManager.WeaponController.Shoot(Camera.main.ScreenPointToRay(ScreenPointerPos).direction);
                Toolbox.Instance.GameManager.Player.WeaponController.Shoot(Camera.main.ScreenPointToRay(ScreenPointerPos).direction);
                // If the wiimote is selected as the input...
                if (InputType == TypeOfInput.WiiMote)
                {
                    // We rumble the wiimote of player 1
                    WiimoteInput.SetWiimoteRumble(0, WiimoteInput.TimeToRumble);  
                }
            }
        }
    }

    
}
