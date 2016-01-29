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
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This component, once attached to a button, let it be triggerred by collisions (right now only bullets)
/// </summary>
public class OnCollisionOnClick : MonoBehaviour
{

    /// The button to click when a bullet collides with it
    [SerializeField]
    private Button buttonToClick;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        buttonToClick = this.gameObject.GetComponent<Button>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            buttonToClick.image.color = buttonToClick.colors.pressedColor;
            buttonToClick.onClick.Invoke();
            if (Toolbox.Instance.GameManager.AllowDebugCode)
            {
                Debug.Log("I've been shot OnCollisionEnter!");

            }
        }
        //this.gameObject.SendMessage("OnClick");
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    public void OnTriggerEnter(Collider other)
    {       
        if (other.CompareTag("Bullet"))
        {
            buttonToClick.image.color = buttonToClick.colors.pressedColor;
            buttonToClick.onClick.Invoke();
            if (Toolbox.Instance.GameManager.AllowDebugCode)
            {
                Debug.Log("I've been shot OnTriggerEnter!");

            }            
        }
        //gameObject.SendMessage("OnClick");
        //other.gameObject.GetComponent<Button>().onClick.Invoke();

    }


}
