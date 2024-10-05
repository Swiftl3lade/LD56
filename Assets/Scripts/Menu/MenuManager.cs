using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MenuManager : MonoBehaviour
{
    public CinemachineVirtualCamera standardCamera;
    public CinemachineVirtualCamera carCamera;
    private float focusSpeed = 5f; // Speed at which camera moves closer
    private float zOffset = 1f; // Adjust this value to set the Z offset from the car
    private float heightOffset = 0.5f; // Adjust this value to set the height offset from the car's center
    private float xOffset = 0.5f; 

    private Transform targetCar; // The car that was clicked
    private bool isZoomingIn = false;

    void Start()
    {
        standardCamera.Priority = 10;
        carCamera.Priority = 0;
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.CompareTag("CarShelf"))
                {
                    // Set the car as the target
                    targetCar = hit.collider.transform;

                    // Switch to the car camera
                    standardCamera.Priority = 0;
                    carCamera.Priority = 10;

                    // Set the Follow and LookAt targets for the car camera
                    carCamera.Follow = targetCar;
                    carCamera.LookAt = targetCar;

                    // Start zooming in
                    isZoomingIn = true;
                }
            }
        }

        // Smooth zoom in on the car
        if (isZoomingIn && targetCar != null)
        {
            // Calculate the target position to center the car
            Vector3 targetPosition = new Vector3(
                targetCar.position.x + xOffset,
                targetCar.position.y + heightOffset, // Adjust the Y position to be higher
                targetCar.position.z + zOffset // Adjust Z position to be smaller
            );

            // Smoothly move the camera towards the target position
            carCamera.transform.position = Vector3.Lerp(carCamera.transform.position, targetPosition, focusSpeed * Time.deltaTime);

            // Stop zooming in after reaching the target position
            if (Vector3.Distance(carCamera.transform.position, targetPosition) < 0.1f)
            {
                isZoomingIn = false; // Stop zooming in once we are close enough
            }
        }
    }
}
