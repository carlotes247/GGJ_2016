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
/// The behaviour of the bullet
/// </summary>
public class BulletBehaviour : MonoBehaviour {

    #region Properties
    /// <summary>
    /// (Field) The particles of the bullet (if any)
    /// </summary>
    [SerializeField]
    private ParticleSystem m_BulletParticles;

    /// <summary>
    /// (Field) The bullet that is being actually thrown
    /// </summary>
    [SerializeField]
    private GameObject m_VisualBullet;

    /// <summary>
    /// (Field) The rigidbody of the bullet, the one to move.
    /// </summary>
    [SerializeField]
    private Rigidbody m_BulletRigidbody;
    
    /// The velocity of the bullet
    [SerializeField]
    float velocity;

    /// The vector to follow when moving
    // Field
    Vector3 bulletDirection;
    // Property
    public Vector3 BulletDirection { get { return bulletDirection; } set { bulletDirection = value; } }

    /// The damage to apply
    [SerializeField]
    float damageToApply; 

    /// The seconds that the bullet will last
    [SerializeField]
    float secondsAlive;
    // Property
    public float SecondsAlive { get { return secondsAlive; } set { secondsAlive = value; } }

    #endregion

    /// This function is called when the object becomes enabled and active
    public void OnEnable()
    {
        //this.GetComponent<Rigidbody>().velocity = bulletDirection * velocity;

        // We reset the position of the visualBullet
        this.m_VisualBullet.transform.position = this.transform.position;

        // We set the velocity of the rigidbody to move 
        this.m_BulletRigidbody.velocity = bulletDirection * velocity;



        StartCoroutine("LifeCountDown");
    }

	/// Use this for initialization
	void Start () {
        
	}
	
	/// Update is called once per frame
	void Update () {
	
	}

    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    public void FixedUpdate()
    {
        
    }

    /// This function will set the direction of the bullet
    void UpdateDirection (Vector3 dir)
    {
        this.bulletDirection = dir;
    }

    /// This function will handle the damage we apply to the target
    void ApplyDamage(GameObject target)
    {
        //target.SendMessage("RemoveLife", damageToApply, SendMessageOptions.DontRequireReceiver);
        // We have switched from send message to getComponent due to the performace improvement
        LifeController aux = target.GetComponent<LifeController>();
        if (aux != null)
        {
            aux.RemoveLife(damageToApply);
        }
        
    }

    /// <summary>
    /// This function will deactivate this gameObject
    /// </summary>
    void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Deactivates the gameObject passed in
    /// </summary>
    /// <param name="target"> The GameObject to deactivate </param>
    void Deactivate (GameObject target)
    {
        target.SetActive(false);
        //Debug.Log("target deactivated!");
    }

    /// OnCollisionEnter is called when this collider/rigidbody has began touching another rigidbody/collider
    public void OnCollisionEnter(Collision collision)
    {
        ApplyDamage(collision.gameObject);
        Deactivate(this.gameObject);
        //Debug.Log("Bullet OnCollisionEnter");
    }

    /// OnTriggerEnter is called when the Collider other enters the trigger
    public void OnTriggerEnter(Collider other)
    {
        ApplyDamage(other.gameObject);
        Deactivate(this.gameObject);
        //Debug.Log("Bullet OnTriggerEnter");
    }

    /// The coroutine to deactivate bullets
    IEnumerator LifeCountDown() 
    {
        float secondsLeft = this.SecondsAlive;
		while (secondsLeft > 0) {
			secondsLeft -= 1;
			yield return new WaitForSeconds(1);
		}
        Deactivate(this.gameObject);
        StopLifeCountDown();
    }
    
    /// The method in charge of stopping the coroutine
    void StopLifeCountDown()
    {
        StopCoroutine("LifeCountDown");
    }




    


}
