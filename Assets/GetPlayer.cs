using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayer : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera followCam;
    // Start is called before the first frame update
    void Start()
    {
        var _car = CarSelectionManager.Instance.GetCar();
        followCam.Follow = _car.transform;
        followCam.LookAt = _car.transform;
    }
}
