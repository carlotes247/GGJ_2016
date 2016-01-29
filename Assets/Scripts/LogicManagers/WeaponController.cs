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
/// The Weapon Controller, controls the amount of bullets and how to shoot them
/// </summary>
public class WeaponController : MonoBehaviour
{

    #region Fields&Properties
    /// The position of the pointer in screen coordinates [deprecated. Use now cursor from inputController]
    //Vector3 pointerPosition;

    /// The bool to know if the wiimote is pointing to the screen [deprecated. Use now WiimoteOnScreen from WiimoteInput]
    //bool wiimoteOnScreen;

    /// <summary>
    /// (Field) The timer of the weapon, to know when to erase shotPositionShort
    /// </summary>
    [SerializeField]
    private TimerController m_WeaponTimerShotPositionShort;
    /// <summary>
    /// (Property) The timer of the weapon, to know when to erase shotPositionShort
    /// </summary>
    public TimerController WeaponTimerShotPositionShort { get { return this.m_WeaponTimerShotPositionShort; } }

    /// <summary>
    /// (Field) The timer of the weapon, to control the shooting delay
    /// </summary>
    private TimerController m_WeaponTimerShotDelay;
    /// <summary>
    /// (Property) The timer of the weapon, to control the shooting delay
    /// </summary>
    public TimerController WeaponTimerShotDelay { get { return m_WeaponTimerShotDelay; } }

    /// The bullet to put ingame
    [SerializeField]
    GameObject bullet;

    /// Size of the ammo clip
    [SerializeField]
    int ammoClipSize;

    /// The array of bullets to do object pooling
    [SerializeField]
    GameObject[] bulletArray;

    /// <summary>
    /// (Field) The distance from where to shoot bullets
    /// </summary>
    [SerializeField]
    float m_DistanceBulletFromObject;

    private Vector3 shotPosition;
    /// <summary>
    /// (Porperty) The position of the last shot performed
    /// </summary>
    public Vector3 ShotPosition { get { return this.shotPosition; } set { this.shotPosition = value; } }

    private Vector3 shotPositionShort;
    /// <summary>
    /// (Porperty) The position of the last shot performed, staying in time for a short time (1 second) 
    /// </summary>
    public Vector3 ShotPositionShort { get { return this.shotPositionShort; } set { this.shotPositionShort = value; } }

    /// <summary>
    /// (Field) The delay between the shots
    /// </summary>
    [SerializeField]
    private float m_DelayBetweenShots;

    /// The bool of the button A [deprecated. Use ButtonA from WiimoteInput]
    //bool buttonA;
    /// The state of the button B [deprecated. Use ButtonB from WiimoteInput]
    //bool buttonB; 

    /// <summary>
    /// (Field) The particles of the fire shown when the shooting gets fired
    /// </summary>
    [SerializeField]
    private ParticleSystem m_WeaponParticles;

    /// <summary>
    /// (Field) The Transform component of the Weapon
    /// </summary>
    [SerializeField]
    private Transform m_WeaponTransform;
    /// <summary>
    /// (Property) The Transform component of the Weapon
    /// </summary>
    public Transform WeaponTransform { get { return this.m_WeaponTransform; } }

    /// <summary>
    /// (Field) The AudioSource of the Weapon
    /// </summary>
    [SerializeField]
    private AudioSource m_WeaponAudioSource;
    #endregion
       

    // Use this for initialization
    void Start()
    {
        CreateBulletArray();

        // We add the weaponTimerShotDelay TimeController to the GameObject as a component
        m_WeaponTimerShotPositionShort = this.gameObject.AddComponent<TimerController>();
        // We label the timerController, to identify it in the inspector
        m_WeaponTimerShotPositionShort.ObjectLabel = "WeaponTimerShotPositionShort";

        // We add the weaponTimerShotDelay TimeController to the GameObject as a component
        m_WeaponTimerShotDelay = this.gameObject.AddComponent<TimerController>();
        // We label the timerController, to identify it in the inspector
        m_WeaponTimerShotDelay.ObjectLabel = "Weapon Timer Shot Delay";

        // We assign the transform to WeaponTransform       
        m_WeaponTransform = this.transform;                     
    }

    // Update is called once per frame
    void Update()
    {
        //if (/*buttonB*/ Toolbox.Instance.GameManager.InputController.WiimoteInput.ButtonB || Input.GetMouseButtonDown(0))
        //{
        //    CheckInputAndShoot();
        //}

        // We erase the shotPositionShort every second
        if (WeaponTimerShotPositionShort.GenericCountDown(1f))
        {
            ShotPositionShort = Vector3.zero;
        }
    }

    // [DEPRECATED. it seems that now we don't need to launch anything a little bit outside of the camera]
    // Function for setting the pointerPosition (it is in screen units)
    //public void SetWeaponPointerValues (Vector3 values) {
    //       Vector3 auxVal = new Vector3(values.x, values.y, distanceBulletFromCamera);
    //       //this.pointerPosition = auxVal;
    //       Toolbox.Instance.GameManager.InputController.ScreenPointerPos = auxVal;
    //       //Debug.Log("auxVal: " + auxVal.ToString());
    //       //this.pointerPosition = Camera.main.ScreenToWorldPoint(auxVal);
    //       //Debug.Log("Pointer: " + pointerPosition);

    //}

    // [DEPRECATED. USE THE PROPERTIES IN THE WIIMOTEINPUT]
    // Function to see if the wiimote is on the screen
    //void SetWiimoteOnScreen (bool value) {
    //	this.wiimoteOnScreen = value;
    //}

    //void SetButtonA(bool value)
    //{
    //    this.buttonA = value;
    //}

    //void SetButtonB(bool value)
    //{
    //    this.buttonB = value;
    //}

    /// This function will check the input of the computer and shoot if the trigger pressed
    void CheckInputAndShoot()
    {
        if (/*wiimoteOnScreen*/ Toolbox.Instance.GameManager.InputController.WiimoteInput.WiiMoteOnScreen)
        {
            // The values are ready to be used
            // We get the direction of the shoot using a raycast from the camera to the pointer position and shoot            
            //Shoot(Camera.main.ScreenPointToRay(pointerPosition).direction);
            Shoot(Camera.main.ScreenPointToRay(Toolbox.Instance.GameManager.InputController.ScreenPointerPos).direction);





        }
        else
        {
            // If the wiimote is not pointing the screen, we use the values of the mouse
            //SetWeaponPointerValues(Input.mousePosition);

            // We get the direction of the shoot using a raycast from the camera to the pointer position and shoot
            //Shoot(Camera.main.ScreenPointToRay(pointerPosition).direction);
            //Debug.DrawRay(Camera.main.transform.position, Camera.main.ScreenPointToRay(pointerPosition).direction, Color.red, 2f);
            Shoot(Camera.main.ScreenPointToRay(Toolbox.Instance.GameManager.InputController.ScreenPointerPos).direction);
            Debug.DrawRay(Camera.main.transform.position, Camera.main.ScreenPointToRay(
                Toolbox.Instance.GameManager.InputController.ScreenPointerPos).direction, Color.red, 2f);
        }

        //Debug.Log("Disparo efectuado!");

    }

    /// <summary>
    /// The function that will be in charge of shooting bullets from the main camera
    /// </summary>
    /// <param name="shootDir"> The direction where to shoot</param>
    public void Shoot(Vector3 shootDir)
    {
        // We go through all the array of bullets
        for (int i = 0; i < bulletArray.Length; i++)
        {
            // Because of the object pooling, we only want the objects that are deactivated
            if (!bulletArray[i].activeInHierarchy)
            {
                // We upate the position of the shot to shoot
                ShotPosition = Toolbox.Instance.GameManager.InputController.ScreenPointerPos;
                // We duplicate the shotPos into a variable that will last only one second. Useful for AIs
                ShotPositionShort = ShotPosition;

                // We put them in the position of our pointer
                //bulletArray[i].transform.position = Camera.main.ScreenToWorldPoint(pointerPosition);                
                bulletArray[i].transform.position = Camera.main.ScreenToWorldPoint(ShotPosition);
                // We position the WeaponParticles in the same place as the bullet plus an offset to see it from the camera
                m_WeaponParticles.transform.position = bulletArray[i].transform.position + (shootDir * 1f);

                // An prepare to shoot them with the right direction!
                bulletArray[i].GetComponent<BulletBehaviour>().BulletDirection = shootDir;
                // We activate the object for the bullet to fly free
                bulletArray[i].SetActive(true);
                
                // We activate the WeaponParticles for one shot
                m_WeaponParticles.Play();
                // We play one shot of the audio
                m_WeaponAudioSource.Play();                

                // And then, we break the for loop to only shoot once
                break;

            }
        }
    }

    /// <summary>
    /// The function that will be in charge of shooting bullets
    /// </summary>
    /// <param name="positionToShootFrom"> The position where we want to shoot from </param>
    /// <param name="shootDir"> The direction where to shoot at</param>
    public void Shoot(Vector3 positionToShootFrom, Vector3 shootDir)
    {
        // We shoot a bullet every m_DelayBetweenShots seconds
        if (m_WeaponTimerShotDelay.GenericCountDown(m_DelayBetweenShots))
        {
            // We go through all the array of bullets
            for (int i = 0; i < bulletArray.Length; i++)
            {
                // Because of the object pooling, we only want the objects that are deactivated
                if (!bulletArray[i].activeInHierarchy)
                {
                    // We upate the position of the shot to shoot
                    ShotPosition = positionToShootFrom;
                    // We place an offset to the shotPosition
                    shotPosition += (this.gameObject.transform.forward * m_DistanceBulletFromObject);
                    // We duplicate the shotPos into a variable that will last only one second. Useful for AIs
                    ShotPositionShort = ShotPosition;
                    // We put them in the position of our pointer
                    //bulletArray[i].transform.position = Camera.main.ScreenToWorldPoint(pointerPosition);                
                    bulletArray[i].transform.position = ShotPosition;
                    // We position the WeaponParticles in the same place as the bullet
                    m_WeaponParticles.transform.position = ShotPosition;
                    // An prepare to shoot them with the right direction!
                    bulletArray[i].GetComponent<BulletBehaviour>().BulletDirection = shootDir;
                    // We activate the object for the bullet to fly free
                    bulletArray[i].SetActive(true);

                    // We activate the WeaponParticles for one shot
                    m_WeaponParticles.Play();
                    // We play one shot of the audio
                    m_WeaponAudioSource.Play();

                    // And then, we break the for loop to only shoot once
                    break;

                }
            }
        }
    }

    /// The function will create 
    void CreateBulletArray()
    {
        if (ammoClipSize > 0)
        {
            this.bulletArray = new GameObject[ammoClipSize];
            for (int i = 0; i < ammoClipSize; i++)
            {
                bulletArray[i] = (GameObject)Instantiate(bullet);
                bulletArray[i].SetActive(false);
                bulletArray[i].transform.parent = this.transform;
            }
        }
    }
}
