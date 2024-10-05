using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TMP_Text standardInstruction;
    [SerializeField] Button playButton;
    [SerializeField] CinemachineVirtualCamera standardCamera;
    [SerializeField] CinemachineVirtualCamera playCamera;

    private CinemachineVirtualCamera currentCarCamera;
    private CinemachineBrain brain;
    private void OnEnable()
    {
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(OnPlayButtonPressed);
    }
    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
    }
    void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
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
                    if(currentCarCamera == null)
                    {
                        return;
                    }
                    standardCamera.Priority = 0;
                    currentCarCamera.Priority = 10;
                    StartCoroutine(WaitForTransition(currentCarCamera));
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
            StartCoroutine(WaitForTransition(standardCamera));
        }
    }

    private IEnumerator WaitForTransition(CinemachineVirtualCamera targetCamera)
    {
        // Wait until the transition begins and is in progress
        while (brain.IsBlending)
        {
            yield return null; // Wait for the next frame
        }

        // Introduce a slight delay before checking if blending has completed
        yield return new WaitForSeconds(0.1f); // Adjust the delay as needed

        // Check again if still blending
        while (brain.IsBlending)
        {
            yield return null; // Wait for the next frame
        }

        OnCameraSwitched(targetCamera);
    }

    private void OnCameraSwitched(CinemachineVirtualCamera camera)
    {
        if(camera == standardCamera)
        {
            standardInstruction.gameObject.SetActive(true);

            playButton.gameObject.SetActive(false);
        }
        else if (camera == playCamera)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            playButton.gameObject.SetActive(true);

            standardInstruction.gameObject.SetActive(false);
        }
    }
    
    private void OnPlayButtonPressed()
    {
        playButton.gameObject.SetActive(false);
        standardCamera.Priority = 0;
        currentCarCamera.Priority = 0;
        playCamera.Priority = 10;
        StartCoroutine(WaitForTransition(playCamera));
    }
}