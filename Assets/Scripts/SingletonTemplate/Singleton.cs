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

/*! \file Singleton.cs
 *  \brief Pattern that restricts the instantiation of a class to a single, static instance and provides a static point of access to it.
 * 
 *  In software engineering, the Singleton Pattern is a design pattern (although now considered by many to be an anti-pattern) 
 *  that restricts the instantiation of a class to a single, static instance and provides a static point of access to it.
 *  
 *  It was first popularized by the 1994 Gang of Four book Design Patterns, but in modern computer science has come under 
 *  heavy criticism for essentially being a wrapped Global Variable, inheriting most of the same dangers and limitations imposed by them.
 * By its nature, Singleton Pattern mixes an object's creation with its behavior (violating Single responsibility principle),
 * introduces a hidden dependency into every method that uses it (making proper Unit Testing extremely difficult or impossible),
 * introduces the potential for methods to produce or consume side effects, causes any method that uses it to violate the principle
 * of least knowledge, and causes an object's lifetime to be decoupled from its actual usage (which could be dangerous in the case
 * of a scarce resource, such as a hardware context).
 * 
 * Despite these shortcomings, its usage in objected-oriented design is pervasive. The most likely explanation is that the concept
 * behind Singleton Pattern tends to be attractive to many software engineers coming from a background in an imperative programming 
 * language, such as C, because it is similar to the types of patterns that are normally used in that paradigm. Also, much like other
 * heavily criticized language constructs such as global variables and the goto statement, sometimes Singleton may be the simplest 
 * solution to a design problem once all tradeoffs have been considered.
 * 
 *  Implementation extracted from http://wiki.unity3d.com/index.php/Toolbox
 * */

using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    /// <summary>
    /// The public instance of the singleton (read only)
    /// </summary>
    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopenning the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                            _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}