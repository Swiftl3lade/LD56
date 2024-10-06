using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CarDetails : MonoBehaviour
{
    public string carName;
    public string carDescription;

    private CinemachineVirtualCamera virtualCamera;

    [HideInInspector] public int power;
    [HideInInspector] public int resistance;
    [HideInInspector] public int handling;

    [HideInInspector] public int maxPower;
    [HideInInspector] public int maxResistance;
    [HideInInspector] public int maxHandling;
    private void Start()
    {
        if(carName == "")
        {
            carName = transform.name;
        }

        if (virtualCamera == null)
        {
            virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        power = Random.Range(1, 4);
        resistance = Random.Range(1, 4);
        handling = Random.Range(1, 4);

        maxPower = Random.Range(4, 6);  
        maxResistance = Random.Range(4, 6);  
        maxHandling = Random.Range(4, 6);  
    }
}
