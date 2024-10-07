using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CarDetails : MonoBehaviour
{
    public string carName;
    public string carDescription;
    public string powerupDescription;
    public string powerupName;
    public GameObject playerCar;
    public Mesh color;

    private CinemachineVirtualCamera virtualCamera;

    public int power;
    public int resistance;
    public int handling;
    public int speed;

    public int maxPower;
    public int maxResistance;
    public int maxHandling;
    public int maxSpeed;
    private void Start()
    {
        if (carName == "")
        {
            carName = transform.name;
        }

        if (virtualCamera == null)
        {
            virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        // power = Random.Range(1, 4);
        // resistance = Random.Range(1, 4);
        // handling = Random.Range(1, 4);
        //
        // maxPower = Random.Range(4, 6);
        // maxResistance = Random.Range(4, 6);
        // maxHandling = Random.Range(4, 6);
    }

    public void SetColor(Mesh mesh)
    {
        color = mesh;
    }
}
