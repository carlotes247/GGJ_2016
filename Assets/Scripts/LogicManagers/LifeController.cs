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

/*! This script handles the life of all the entities in the game */

using UnityEngine;
using System.Collections;

/// <summary>
/// The object life controller. Controls current life, adding and removing
/// </summary>
public class LifeController : MonoBehaviour
{

    /// The Object Manager so that this script has acces to the rest of components
    // Field
    private ObjectManager objectManager;
    // Property
    public ObjectManager ObjectManager { get { return this.objectManager; } set { this.objectManager = value; } }

    private AnimController animController;

    /// The life of the gameObject
    [SerializeField]
    float life;
    float Life
    {
        get
        {
            return life;
        }
        set
        {
            life = value;
        }
    }
    float m_OriginalLife;

    /// The animator of this gameObject
    Animator anim;

    /// The score is giving to kill an enemy
    [SerializeField]
    float scoreWhenKilled;
    public float ScoreWhenKilled { get { return this.scoreWhenKilled; } set { this.scoreWhenKilled = value; } }

    /// Awake is called when the script instance is being loaded
    public void Awake()
    {
        // We find the animator
        anim = this.GetComponent<Animator>();

        // We find the EnemyManager
        ObjectManager = GetComponent<ObjectManager>();

        // We save the life into originalLife. Later, in the next event, we will load it
        m_OriginalLife = life;
    }

    // This function is called when the object becomes enabled and active
    public void OnEnable()
    {
        // Every time we enable the object, we reset the life to the original one
        Life = m_OriginalLife;
    }

    /// Use this for initialization
    void Start()
    {
        
    }

    /// Update is called once per frame
    void Update()
    {
        LifeCycle();
        //float lerp = Mathf.PingPong(Time.time, duration) / duration;
        //objectRenderer.material.color = Color.Lerp(materialColor, Color.red, lerp); 
    }    

    /// The method for removing life
    public void RemoveLife(float amount)
    {
        if (Life > 0)
        {
            Life -= amount;
        }
        VisualHit();
    }

    /// The function that will take care of showing that the object is being damaged
    void VisualHit()
    {
        //this.anim.SetTrigger("Damage");

        //this.anim.ResetTrigger("VisualHit");

        ObjectManager.AnimController.Damage();
    }

    /// The method for controlling the lifeCycle of the gameObject
    void LifeCycle()
    {
        // If the life is inferior or equal to 0, the object dies
        if (Life <= 0)
        {
            Kill(this.gameObject);
        }
    }

    /// The method for killing the object
    void Kill(GameObject target)
    {
        // We increment the score
        Toolbox.Instance.GameManager.ScoreController.UpdateScore(ScoreWhenKilled);
        //Destroy(target);
        // We deactivate the object
        target.SetActive(false);
    }
}
