using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType<DontDestroyOnLoad>().Length > 1)
        {
            Destroy(gameObject);  // Destroy duplicate
        }
        else
        {
            DontDestroyOnLoad(gameObject);  // Persist the original object
        }
    }
}
