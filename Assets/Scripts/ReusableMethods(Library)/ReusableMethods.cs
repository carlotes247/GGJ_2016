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
using System;

/// <summary>
/// Contains different classes with ReusableMethods in programming
/// </summary>
namespace ReusableMethods
{
    /// <summary>
    /// Class containing methods using probabilities
    /// </summary>
    public static class Probabilities
    {
        /// <summary>
        /// The method for trigerring something based on a probability value
        /// </summary>
        /// <param name="probVal"> The probability float value in the range [0, 1]</param>
        /// <returns> Returns more often True the higher probVal is </returns>
        public static bool GetResultProbability (float probVal)
        {
            // We check that probVal is between [0, 1]
            if (probVal > 1f || probVal < 0f)
            {
                Debug.LogError("The probability in GetResultProbability() is out of the range [0, 1]");
                return false;

            }
            
            // We generate a value between the range [0, 1]
            float valueToCompare = UnityEngine.Random.Range(0f, 1f);            

            // If valueToCompare is below probVal, we return true. 
            // The higher probVal, the most likely to return true we will have
            if (valueToCompare < probVal)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }

    /// <summary>
    /// The class that will contain the methods for creating countdowns
    /// </summary>
    public class Countdown
    {
        /// <summary>
        /// This is just a test empty method
        /// </summary>
        /// <returns> No usable value, is an example</returns>
        public static int CreateCountdown()
        {
            return 4;
        }
    }    

    /// <summary>
    /// Class containing methods and tools for enums
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// Sets a random value for the Enum Type passed in
        /// </summary>
        /// <typeparam name="T"> T must be an Enum Type, from where to select the task</typeparam>
        /// <returns> The new task to perform</returns>
        public static T GetRandomEnumValue<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            // Since Enum Type implements IConvertible interface, we check if T is struct & IConvertible
            // This will still permit passing of value types implementing IConvertible. The chances are rare though.

            // If the type is not an enum, we throw an exception
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type (Enum)");
            }
            
            // We define a local integer
            int taskToChoose;
            // We calculate a random value, based on the Enum length
            taskToChoose = UnityEngine.Random.Range(0, Enum.GetValues(typeof(T)).Length);
            // We cast and select then the value and return the enum
            //return (T) Convert.ChangeType(taskToChoose, typeof(T));
            return (T)(object)taskToChoose;

        }
    }

    /// <summary>
    /// Class containing tools and methods to calculate points
    /// </summary>
    public static class Points
    {
        /// <summary>
        /// The function returns the opposite point to an object based on a direction, determined by a radius
        /// </summary>
        /// <param name="objectPosition"> The actual position of the object to work with</param>
        /// <param name="directionToDangerPoint"> The normalized direction from where the danger is coming from</param>
        /// <param name="radiusForNewPoint"> The radius to calculate the new point</param>
        /// <returns></returns>
        public static Vector3 CalculateOppositePoint(Vector3 objectPosition, Vector3 directionToDangerPoint, float radiusForNewPoint)
        {
            // We calculate the new point, adding the normalized vector times the radius to the current object position
            Vector3 auxOppositePoint = objectPosition + (-directionToDangerPoint * radiusForNewPoint);

            return auxOppositePoint;
        }

    }
    
    /// <summary>
    /// Class containing tools and methods for performing mathematical statistical conversions
    /// </summary>
    public static class Normalization
    {
        /// <summary>
        /// Returns a normalized value depending on a min and max
        /// </summary>
        /// <param name="value"> The current value to normalize </param>
        /// <param name="min"> The min of that value </param>
        /// <param name="max"> The max of that value </param>
        /// <returns> The normalized value calculated </returns>
        public static float Normalize (float value, float min, float max)
        {
            // We calculate the normalize value of the value passed in
            return (value - min) / (max - min);
        }

        /// <summary>
        /// Returns a denormalized value depending on a min and max
        /// </summary>
        /// <param name="valueNormalized"> The current value to denormalize </param>
        /// <param name="min"> The min of that value </param>
        /// <param name="max"> The max of that value </param>
        /// <returns> The denormalized value calculated </returns>
        public static float Denormalize (float valueNormalized, float min, float max)
        {
            // We calculate the denormalized value of the value passed in
            return ( valueNormalized * (max - min) + min );
        }
    }

    /// <summary>
    /// Class containing tools and methods for performing mathematical vector calculations
    /// </summary>
    public static class Vectors
    {
        /// <summary>
        /// Calculates the direction from a position to another
        /// </summary>
        /// <param name="fromPos"> The origin position</param>
        /// <param name="toPos"> The destination position</param>
        /// <returns> The non-normalized vector direction</returns>
        public static Vector3 CalculateDirection (Vector3 fromPos, Vector3 toPos)
        {
            // We calculate the direction to the vector toPos
            return (toPos - fromPos);
        }

        /// <summary>
        /// Checks the distance to an object, to see if it is within radius (this is a fast way of doing it)
        /// </summary>
        /// <param name="direction"> The non-normalized direction vector</param>
        /// <param name="maxRange"> The maximumDistance (radius) to check</param>
        /// <returns> True if the direction is below the maxRange</returns>
        public static bool CheckDistance (Vector3 direction, float maxRange)
        {
            // If the sqrMagnitude of the direction is lower than the squares distance ...
            // The sqrMagnitude property gives the square of the magnitude value, and is calculated like the magnitude but without the time-consuming square root operation
            if (direction.sqrMagnitude < maxRange * maxRange)
            {
                return true;
            }
            // If it is above...
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Search for the closest object with the provided tag within a radius
        /// </summary>
        /// <param name="origin"> The origin from where to search</param>
        /// <param name="radius"> The radius of the search</param>
        /// <param name="tagToSearch"> The tag of the object to find</param>
        /// <returns></returns>
        public static GameObject SearchClosestObjectWihTag (Vector3 origin, float radius, string tagToSearch)
        {            
            // We create the helper array that will contain all the close colliders
            Collider[] allCollidersArray = null;

            // The GameObject to return
            GameObject result = null;

            // To see how many objects are counted
            int aux;

            // We create a sphere with the origin and radius provided and get all colliders hitted by it
            //int aux = Physics.OverlapSphereNonAlloc(origin, radius, allCollidersArray);
            allCollidersArray = Physics.OverlapSphere(origin, radius);
            aux = allCollidersArray.Length;
            
            // We only run this part if there is any object found
            if (aux > 0 && allCollidersArray != null)
            {
                // We define the distance variable to check the object
                float distanceToObject = 0f;
                // We define the actual distance of the closest found object
                float closestObjectDistance = 0f;

                // We go through all the colliders found and save the ones we want to the return colliders array
                for (int i = 0; i < allCollidersArray.Length; i++)
                {
                    //Debug.Log("Searching for " + tagToSearch + " ...");
                    if (allCollidersArray[i].CompareTag(tagToSearch))
                    {
                        // We calculate the distance to the found object
                        distanceToObject = Vector3.Distance(origin, allCollidersArray[i].transform.position);
                        // If there are any other closest object, we set the actual distance as the closest
                        if (closestObjectDistance == 0f)
                        {
                            closestObjectDistance = distanceToObject;
                        }
                        // We check if the distance is below or equal the closestDistance found
                        if (distanceToObject <= closestObjectDistance)
                        {
                            // If it is, we set this object as the result to return and update the closestDistance found
                            result = allCollidersArray[i].gameObject;
                            closestObjectDistance = distanceToObject;
                            Debug.Log(tagToSearch + " found!");
                        }
                    }
                }
            }

            // Debug the amount of colliders found
            Debug.Log(aux.ToString() + "Colliders found");

           
            
            // We return the objects
            return result;
        }
    }

    /// <summary>
    /// Class containing tools and methods for dealing with Arrays
    /// </summary>
    public static class Arrays
    {
        /// <summary>
        /// Set all the values in the array to the valueToSet
        /// </summary>
        /// <typeparam name="T"> The type of the array </typeparam>
        /// <param name="arrayToSet"> The array we want to change </param>
        /// <param name="valueToSet"> The value we want to set in every position of the array </param>
        public static void SetAllArray<T>(ref T[] arrayToSet, T valueToSet)
        {
            // We go through all the array
            for (int i = 0; i < arrayToSet.Length; i++)
            {
                // We set each position of the array to the value we want
                arrayToSet[i] = valueToSet;
            }
        }

        /// <summary>
        /// SetActive all the values in the array to the specified value
        /// </summary>
        /// <typeparam name="T"> The type of the array </typeparam>
        /// <param name="arrayToSet"> The array we want to change </param>
        /// <param name="valueToSet"> The value we want to set in every position of the array </param>
        public static void SetActiveAllArray<T>(ref T[] arrayToSet, bool valueToSet)
        {
            //Debug.Log("Setting array " + arrayToSet.ToString() + " to " + valueToSet.ToString());
            // We evaluate the first component to see what type the array is
            // If it is a Behaviour (almost every component)...
            if (arrayToSet[0] is Behaviour)
            {
                // ... we go through all the array
                for (int i = 0; i < arrayToSet.Length; i++)
                {
                    // We set each position of the array to the value we want                
                    (arrayToSet[i] as Behaviour).enabled = valueToSet;                   
                }

            }
            // If it is a GameObject ...
            else if (arrayToSet[0] is GameObject)
            {
                // ... we go through all the array
                for (int i = 0; i < arrayToSet.Length; i++)
                {
                    // We set each position of the array to the value we want                
                    (arrayToSet[i] as GameObject).SetActive(valueToSet);
                }
            }
            // If it is a Collider ...
            else if (arrayToSet[0] is Collider)
            {
                // ... we go through all the array
                for (int i = 0; i < arrayToSet.Length; i++)
                {
                    // We set each position of the array to the value we want                
                    (arrayToSet[i] as Collider).enabled = valueToSet;
                }
            }
            // If it is a Renderer...
            else if (arrayToSet[0] is Renderer)
            {
                // ... we go through all the array
                for (int i = 0; i < arrayToSet.Length; i++)
                {
                    // We set each position of the array to the value we want                
                    (arrayToSet[i] as Renderer).enabled = valueToSet;
                }
            }
                           
        }
    }
}
