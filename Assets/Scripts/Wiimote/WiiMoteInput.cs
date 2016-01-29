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
using System;
using System.Collections;
using System.Runtime.InteropServices;
using ReusableMethods;

/// <summary>
/// This is the class that controls the input data from the wiimote
/// </summary>
public class WiiMoteInput : MonoBehaviour {
	
	[DllImport ("UniWii")]
	private static extern void wiimote_start();
	
	[DllImport ("UniWii")]
	private static extern void wiimote_stop();
	
	[DllImport ("UniWii")]
	private static extern int wiimote_count();
	
	[DllImport ("UniWii")]
	private static extern byte wiimote_getAccX(int which);
	[DllImport ("UniWii")]
	private static extern byte wiimote_getAccY(int which);
	[DllImport ("UniWii")]
	private static extern byte wiimote_getAccZ(int which);
	
	/* The IR Returns a float (from -1 to 1) of the value of the IR sensors. 
	 * (-1,-1) is topleft, and (1,1) is bottom right.
	 * (0, 0) is the center of the screen in Ir coordinates
	 * (-1, 0) left center, (1, 0) right center
	 * (0, -1) center top, (0, 1) center bottom
	 * A value of -100 means the IR sensor could not be seen 
	 * (i.e. the IR LED was occluded, or the wiimote was pointed away, etc.)  */
	[DllImport ("UniWii")]
	private static extern float wiimote_getIrX(int which);
	[DllImport ("UniWii")]
	private static extern float wiimote_getIrY(int which);

	[DllImport ("UniWii")]
	private static extern float wiimote_getRoll(int which);
	[DllImport ("UniWii")]
	private static extern float wiimote_getPitch(int which);
	[DllImport ("UniWii")]
	private static extern float wiimote_getYaw(int which);

	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonA(int which);

	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonB(int which);

	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonUp(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonLeft(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonRight(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonDown(int which);

    [DllImport("UniWii")]
    private static extern void wiimote_rumble(int which, float duration);

    // Imports for expansion
    [DllImport("UniWii")]
    private static extern bool wiimote_isExpansionPortEnabled(int which);

    [DllImport("UniWii")]
    private static extern byte wiimote_getNunchuckStickX(int which);
    [DllImport("UniWii")]
    private static extern byte wiimote_getNunchuckStickY(int which);


    //	[DllImport ("UniWii")]	
    //	private static extern bool wiimote_enableIR( int which );

    // Display the info of the wiimote data
    private string display;
	// The x and y position of the pointer ingame
	private int cursor_x, cursor_y;
    //public int Cursor_x { get { return this.cursor_x; } set { this.cursor_x = value; } }
    //public int Cursor_y { get { return this.cursor_y; } set { this.cursor_y = value; } }
    private Vector3 cursorPosition;
    public Vector3 CursorPosition { get {return this.cursorPosition; } }
	// The texture to draw ingame through OnGUI() [OnGUI is deprecated]
	// public Texture2D cursor_tex;
	// The ingame cursor to control in the scene
	//public GameObject ingameCursor;
	// oldVec is use in applyRPYValuesToModel()
	private Vector3 oldVec;

	// A variable to store the values of the accelerometer
	Vector3 accValues;
	// A variable to store the roll
	float roll;
	// A variable to store the pitch
	float pitch;
	// A variable to store the yaw (always 0, no wiimote plus support)
	float yaw;
	// A variable to store the IR values
	Vector2 irValues;

    // The bool for knowing where is the sensor bar (put value throught editor)
    [SerializeField]
    private bool sensorBar_below;

	// The gameObject to send messages [deprecated. Reference the controller through the Toolbox instance]
	//public GameObject objectToMessage;

    /// The bool that is stating if the wiimote is pointing the screen or not
    // Field
    private bool wiiMoteOnScreen;
    // Property
    public bool WiiMoteOnScreen { get { return this.wiiMoteOnScreen; } set { this.wiiMoteOnScreen = value; } }

    /// The integer that counts how many wiimotes are connected
    // Field
    private int wiiMoteCount;
    // Property
    public int WiiMoteCount { get { return this.wiiMoteCount; } set { this.wiiMoteCount = value; } }

    /// The code for the different buttons of the wiimote
    // Button B
    // Field
    private bool buttonB;
    // Property
    public bool ButtonB { get { return this.buttonB; } set { this.buttonB = value; } }

    /// <summary>
    /// (Field) The time to rumble
    /// </summary>
    [SerializeField]
    float timeToRumble;
    /// <summary>
    /// (Property) The time to rumble
    /// </summary>
    public float TimeToRumble { get { return this.timeToRumble; } set { this.timeToRumble = value; } }

    /// <summary>
    /// (Field) The timer for not be counting wiimotes every frame
    /// </summary>
    private TimerController m_WiimoteCountTimer;

    /// <summary>
    /// (Field) Controls if there is any expansion port enabled (UniWii only supports nunchuck)
    /// </summary>
    [SerializeField]
    private bool m_IsExpansionPortEnabled;

    /// <summary>
    /// (Field) The values of the nunchuck joystick
    /// </summary>
    [SerializeField]
    private Vector2 m_NunchuckJoystickValues;
    /// <summary>
    /// (Property) The values of the nunchuck joystick
    /// </summary>
    public Vector2 NunchuckJoystickValues { get { return this.m_NunchuckJoystickValues; } }


    // Use this for initialization (WE ONLY INITIALIZE ON STANDALONE)
    void Start () {
#if UNITY_STANDALONE_WIN
        // We start invoking the UniWii dll, to initialize all possible wiimotes
        wiimote_start();

        // We add the timer for counting wiimotes
        m_WiimoteCountTimer = gameObject.AddComponent<TimerController>();
        // We write the label to identify in the inspector
        m_WiimoteCountTimer.ObjectLabel = "WiimoteInput Timer";

        // THE DRAWING OF THE CURSOR IS NOW RESPONSIBILITY OF THE HUDCONTROLLER
        //this.ingameCursor.SetActive(false);
        //Toolbox.Instance.GameManager.HudController.InGameCursor.enabled = false;  
#endif
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //WiimoteInputLogic();
	}
    
    /// <summary>
    /// The logic of the wiimote, in charge of populating all the variables
    /// </summary>
    public void WiimoteInputLogic()
    {
        // We count how many wiimotes there are connected (once per second)
        if (m_WiimoteCountTimer.GenericCountDown(1f))
        {
            WiiMoteCount = wiimote_count(); 
        }
        // If there are any wiimotes...
        if (WiiMoteCount > 0)
        {
            display = "";
            if (Toolbox.Instance.GameManager.AllowDebugCode)
            {
                Debug.Log("Wiimotes connected: " + WiiMoteCount.ToString()); 
            }
            // We go through all the wiimotes with a for, and make them do the same
            for (int i = 0; i <= WiiMoteCount - 1; i++)
            {
                //CalculateAccelerometerValues(i);
                //CalculateRollPitchYaw(i);
                CalculateIrPos(i);

                //				display += "Wiimote " + i + " accX: " + this.accValues.x + " accY: " + this.accValues.y + " accZ: " + this.accValues.z +
                //					" roll: " + this.roll + " pitch: " + this.pitch + " yaw: " + this.yaw + 
                //						" IR X: " + this.irValues.x + " IR Y: " + this.irValues.y + "\n";

                CalculateWiimotePointer(i);

                // This is my code for detecting when A is pressed
                //bool aux = wiimote_getButtonA(i);
                //objectToMessage.SendMessage("SetButtonA", aux);

                // The code for the D-Pad
                //bool buttonUp = wiimote_getButtonUp(i);
                //bool buttonDown = wiimote_getButtonDown(i);
                //bool buttonLeft = wiimote_getButtonLeft(i);
                //bool buttonRight = wiimote_getButtonRight(i);
                //objectToMessage.SendMessage("setButtonUp", buttonUp);
                //objectToMessage.SendMessage("setButtonDown", buttonDown);
                //objectToMessage.SendMessage("setButtonLeft", buttonLeft);
                //objectToMessage.SendMessage("setButtonRight", buttonRight);

                //bool buttonB = wiimote_getButtonB(i);

                // We collect the raw input from the wiimote
                ButtonB = wiimote_getButtonB(i);
                //// If the button has been pressed...
                //if (Time.time >= timestamp && m_RawButtonB)
                //{
                //    ButtonB = m_RawButtonB;
                //    timestamp = Time.time + m_KeyInputDelay;
                //}
                //else
                //{
                //    ButtonB = false;
                //}

                //objectToMessage.SendMessage("SetButtonB", buttonB);

                // Code for the rumble
                //if (ButtonB /*|| aux*/)
                //{
                //    wiimote_rumble(i, timeToRumble);
                //}

                // We populate the value for checking the extension port
                CheckForExtensionPort(i);
                // We collect the nunchuck joystick data if there is an extension port
                if (m_IsExpansionPortEnabled)
                {
                    GetNunchuckJoystick(i);
                }
                
            }
        }
        else
        {
            Debug.LogWarning("Wiimote(s) not found! Connect it before continuing!");
        }
        //		else display = "Press the '1' and '2' buttons on your Wii Remote.";
    }

    /// <summary>
    /// Sets to rumble a wiimote woth its Id for a time
    /// </summary>
    /// <param name="wiimoteId"> The Id of the wiimote to rumble</param>
    /// <param name="timeToRumble"> The amount of time we want it to rumble</param>
    public void SetWiimoteRumble (int wiimoteId, float timeToRumble)
    {
        wiimote_rumble(wiimoteId, timeToRumble);
    }

    // This function calculates the accelerometer values from the wiimote
    void CalculateAccelerometerValues(int i)
    {
        this.accValues = new Vector3(wiimote_getAccX(i), wiimote_getAccY(i), wiimote_getAccZ(i));
    }

    // This function calculates the roll, pitch, yaw values form the wiimote
    void CalculateRollPitchYaw(int i)
    {
        this.roll = Mathf.Round(wiimote_getRoll(i));
        this.pitch = Mathf.Round(wiimote_getPitch(i));
        this.yaw = Mathf.Round(wiimote_getYaw(i));
        //float yaw = wiimote_getYaw(i);
    }

    // This function calculates the IR (x,y) values form the wiimote
    void CalculateIrPos(int i)
    {
        // The Y value needs to get reversed
        this.irValues = new Vector3(wiimote_getIrX(i), wiimote_getIrY(i) * -1f);
    }

    // This function applies the values we got from RPY to a 3D model
    void applyRPYValuesToModel(string objectName)
    {
        // This is the code for moving in space a 3d model according to the values we get
        if (!float.IsNaN(roll) && !float.IsNaN(pitch))
        {
            Vector3 vec = new Vector3(pitch, yaw, -1 * roll);
            vec = Vector3.Lerp(oldVec, vec, Time.deltaTime * 5);
            oldVec = vec;
            GameObject.Find(objectName).transform.eulerAngles = vec;
        }
    }
	
	// This function draws a pointer on Screen
	void CalculateWiimotePointer (int i) {
		// This is the code for the pointer in screen space
		// This code is executed if the wiimote is pointing the screen
		// This code calculates a value for the variables cursor_x and cursor_y. We will draw them on screen
		if ( ((irValues.x != -100) && (irValues.y != -100)) ) {
			//If we are pointing the screen, we send a message to other script
			//objectToMessage.SendMessage("SetWiimoteOnScreen", true);
            this.WiiMoteOnScreen = true;
			//this.ingameCursor.SetActive(true);
            //Toolbox.Instance.GameManager.HudController.InGameCursor.enabled = true;

			float temp_x;	
			float temp_y;
            // We calculate the position of the cursor depending of the position of the sensor bar
            if (sensorBar_below) {
				// Sensor bar BELOW algorithm
				temp_x = ( Screen.width / 2) + irValues.x * (float) Screen.width / (float)2.0;
				temp_y = Screen.height - (irValues.y * (float) Screen.height / (float)2.0);
			} else {
				// Sensor bar ABOVE algorithm
				temp_x = ((irValues.x + (float) 1.0)/ (float)2.0) * (float) Screen.width;
				temp_y = (float) Screen.height - (((irValues.y + (float) 1.0)/ (float)2.0) * (float) Screen.height);
			}

            // We update the exposed position of the wiimote cursor, so that every other script can have access to it
			//cursor_x = Mathf.RoundToInt(temp_x);
			//cursor_y = Mathf.RoundToInt(temp_y);

            // We set the cursor position
            cursorPosition.x = Mathf.RoundToInt(temp_x);
            cursorPosition.y = Mathf.RoundToInt(temp_y);
            //Debug.Log("X: " + cursor_x.ToString() + ", Y: " + cursor_y.ToString());

            //if ((cursor_x != 0) || (cursor_y != 0))

            /* ============ ALL THE RESPONSABILITY OF DRAWING THE CURSOR NOW IS OUTSIDE THE WIIMOTEINPUT SCRIPT =========*/

            // We draw a box with our cursor position (I needed to tweak the Y value)
            // ingameCursor.transform.position = new Vector3 (cursor_x, (Screen.height) - cursor_y, 0);
            //Toolbox.Instance.GameManager.HudController.InGameCursor.transform.position = new Vector3(cursor_x, (Screen.height) - cursor_y, 0);

            //ingameCursor.transform.position = Camera.main.scre new Vector3 (cursor_x, (Screen.height) - cursor_y, 0);

            // We prepare the send the pointer position in screen points so that the script StarMovement can work with it
            // I have translated the values of the ir directly inteo viewport numbers, so that is more precise
            //objectToMessage.SendMessage("SetPointerValues" , ingameCursor.transform.position);
            //Toolbox.Instance.GameManager.WeaponController.SetWeaponPointerValues(Toolbox.Instance.GameManager.InputController.ScreenPointerPos);


        } else {
            //If we are not pointing the screen, we set the WiiMoteOnScreen to false
            //objectToMessage.SendMessage("SetWiimoteOnScreen", false);
            //this.ingameCursor.SetActive(false);
            WiiMoteOnScreen = false;
            //Toolbox.Instance.GameManager.HudController.InGameCursor.enabled = false;
		}

	}

    /// <summary>
    /// Checks if there is any expansion port enabled on the desired wiimote
    /// </summary>
    /// <param name="i"> Which wiimote to check </param>
    private void CheckForExtensionPort (int i)
    {
        m_IsExpansionPortEnabled = wiimote_isExpansionPortEnabled(i);
    }

    /// <summary>
    /// Gets the nunchuck joystick values
    /// </summary>
    /// <param name="i"> The index of which Wiimote to acess</param>
    private void GetNunchuckJoystick(int i)
    {
        // We get the raw data from the nunchuk
        m_NunchuckJoystickValues.x = wiimote_getNunchuckStickX(i);
        m_NunchuckJoystickValues.y = wiimote_getNunchuckStickY(i);

        // We normalize them (I checked the values on both axis several times and these are the maximum and minimum I got. Weird)
        m_NunchuckJoystickValues.x = Normalization.Normalize(m_NunchuckJoystickValues.x, 25, 230);
        m_NunchuckJoystickValues.y = Normalization.Normalize(m_NunchuckJoystickValues.y, 27, 221);

        // We scale the normalized values between (-1, 1)
        m_NunchuckJoystickValues.x = Normalization.ScaleNormalize(m_NunchuckJoystickValues.x, -1, 1);
        m_NunchuckJoystickValues.y = Normalization.ScaleNormalize(m_NunchuckJoystickValues.y, -1, 1);

    }


    void OnApplicationQuit() {
		//wiimote_stop();
        // I MAY NEED TO UNCOMMENT THIS FOR THE FINAL BUILD OF THE GAME!!
	}
	
	void OnGUI() {
		// Where the info if the values will be displayed
		//GUI.Label( new Rect(10,10, 500, 100), display);

//		// This is the code for the pointer in screen space
//		// First, we do a function depending on the position of the cursor calculated above. Don't really know why
//		if ((cursor_x != 0) || (cursor_y != 0)) GUI.Box ( new Rect (cursor_x, cursor_y, 50, 50), cursor_tex); //"Pointing\nHere");
//		// We count how many wiimotes there are connected
//		int c = wiimote_count();
//		// We go through all the wiimotes with a for, and make them do the same
//		for (int i=0; i<=c-1; i++) {
//			// Get the X position of the current wiimote pointer
//			float ir_x = wiimote_getIrX(i);
//			// Get the Y position of the current wiimote pointer
//			float ir_y = wiimote_getIrY(i);
//
//
//			// This code is executed if the wiimote is pointing the screen
//			// This code calculates a value for the variables cursor_x and cursor_y. We will draw them on OnGUI
//			// This code is if the sensor bar is BELOW the screen
//			if ( (i==c-1) && (ir_x != -100) && (ir_y != -100) ) {
//				//float temp_x = ((ir_x + (float) 1.0)/ (float)2.0) * (float) Screen.width;
//				//float temp_y = (float) Screen.height - (((ir_y + (float) 1.0)/ (float)2.0) * (float) Screen.height);
//				float temp_x = ( Screen.width / 2) + ir_x * (float) Screen.width / (float)2.0;
//				float temp_y = Screen.height - (ir_y * (float) Screen.height / (float)2.0);
//				cursor_x = Mathf.RoundToInt(temp_x);
//				cursor_y = Mathf.RoundToInt(temp_y);
//			}
//			
//			
//			// This code is if the sensor bar is ABOVE the screen
//			// We check if the WiiMote is pointing the sensor bar
//			if ( (ir_x != -100) && (ir_y != -100) ) {
//				// We get a temporal (x, y) position and... modify it a bit? (I guess is whether you put the sensor bar above the screen or velow the screen)
//				float temp_x = ((ir_x + (float) 1.0)/ (float)2.0) * (float) Screen.width;
//				float temp_y = (float) Screen.height - (((ir_y + (float) 1.0)/ (float)2.0) * (float) Screen.height);
//				// Returns an int rounded to the nearest integer.
//				temp_x = Mathf.RoundToInt(temp_x);
//				temp_y = Mathf.RoundToInt(temp_y);
//				//if ((cursor_x != 0) || (cursor_y != 0))
//				// We draw a box with our temporal x,y position
//				GUI.Box ( new Rect (temp_x, temp_y, 64, 64), "Pointing\nHere" + i);
//			}
//		}
	}
}