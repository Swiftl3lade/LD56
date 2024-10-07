using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonobehaviour<T> : MonoBehaviour where T :MonoBehaviour
{

    private static T instance;

    // Property to access the instance
    public static T Instance
    {
        get
        {
            // If instance is null, find the object in the scene or create one
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                // If no instance is found, create a new GameObject with the component
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";
                }
            }

            return instance;
        }
    }

    // To ensure the instance persists across scenes
    protected virtual void Awake()
    {
        // If the instance already exists and it's not the current one, destroy this one
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }
}
