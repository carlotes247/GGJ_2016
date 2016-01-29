using UnityEngine;
using System.Collections;

public class CollisionController : MonoBehaviour
{

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

        //if (collision.collider.CompareTag("Enemy"))
        //{
        //    // We show the loosing screen only if it's disabled
        //    if (!Toolbox.Instance.GameManager.HudController.loosingCanvas.enabled)
        //    {
        //        // We show the loosing screen
        //        Toolbox.Instance.GameManager.HudController.loosingCanvas.enabled = true;
        //        // We pause the game
        //        Time.timeScale = 0f;

        //    }
        //}
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    public void OnTriggerEnter(Collider other)
    {
        
        //if (other.CompareTag("Enemy"))
        //{
        //    Debug.Log("TriggerEntered!");
        //    // We show the loosing screen only if it's disabled
        //    if (!Toolbox.Instance.GameManager.HudController.loosingCanvas.enabled)
        //    {
        //        // We set the game to lost
        //        Toolbox.Instance.GameManager.gameLogicController.gameLost = true;                

        //    }
        //}
    }
}
