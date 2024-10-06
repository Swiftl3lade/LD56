using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CarDetails : MonoBehaviour
{
    [SerializeField] string carName;
    [SerializeField] string carDescription;

    private CinemachineVirtualCamera virtualCamera;

    private int power;
    private int resistance;
    private int handling;

    private int maxPower;
    private int maxResistance;
    private int maxHandling;
    private void Start()
    {
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
