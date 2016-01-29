using UnityEngine;
using System.Collections;

/// <summary>
/// This is the Controller that provides tools for coroutines
/// </summary>
public class CoroutineController : MonoBehaviour {

	/// <summary>
    /// Allows a script to start a coroutine only once, protected by a flag
    /// </summary>
    /// <param name="coroutineToStart"> The method that we want to pass to the coroutineInstance</param>
    /// <param name="coroutineInstance"> The instance of the method we passed (NEEDS TO BE INITIALIZED BEFORE CALLING)</param>
    /// <param name="coroutineFlag"> The control flag of the coroutine (NEEDS TO BE INITIALIZED BEFORE CALLING)</param>
    public void StartCoroutineFlag(IEnumerator coroutineToStart, ref IEnumerator coroutineInstance, ref bool coroutineFlag)
    {
        // If the coroutine is not running, we start its the execution 
        if (!coroutineFlag)
        {
            // We define the instance of the coroutine to run
            coroutineInstance = coroutineToStart;
            // We start the coroutine. It is faster to start a defined instance than to use the string method
            StartCoroutine(coroutineInstance);
            // We set the flag to true, so that no other coroutine will be started
            coroutineFlag = true;
        }
    }
}
