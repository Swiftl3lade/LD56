using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MenuManager : MonoBehaviour
{
    public CinemachineVirtualCamera standardCamera;
    private CinemachineVirtualCamera currentCarCamera;

    void Start()
    {
        standardCamera.Priority = 10;
    }
 
    void Update()
    {
        CheckFocusCarCamera();
        CheckStandardCamera();
    }
    
    private void CheckFocusCarCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.CompareTag("CarShelf"))
                {
                    currentCarCamera = hit.collider.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
                    standardCamera.Priority = 0;
                    currentCarCamera.Priority = 10;
                }
            }
        }
    }

    private void CheckStandardCamera()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            standardCamera.Priority = 10;
            currentCarCamera.Priority = 0;
        }
    }

}
