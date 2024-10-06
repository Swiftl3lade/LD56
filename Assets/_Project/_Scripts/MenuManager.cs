using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class MenuManager : MonoBehaviour
{
    [Header("Standard Menu")]
    [SerializeField] TMP_Text standardInstruction;
    [SerializeField] CinemachineVirtualCamera standardCamera;

    [Header("Car Menu")]
    [SerializeField] Button playButton;
    [SerializeField] CinemachineVirtualCamera playCamera;
    [SerializeField] GameObject carDetails;
    [SerializeField] TMP_Text carNameText;
    [SerializeField] TMP_Text carDescriptionText;
    [SerializeField] Transform powerStats;
    [SerializeField] Transform handlingStats;
    [SerializeField] Transform resistanceStats;

    [Header("Prefabs")]
    [SerializeField] GameObject UpgradedStatPrefab;
    [SerializeField] GameObject NotUpgradedStatPrefab;

    private CarDetails currentCarDetails;
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
        standardCamera.Priority = 20;
        ActiveStandardCameraUI(true);
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
                    currentCarDetails = hit.collider.gameObject.GetComponent<CarDetails>();
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
            ActiveStandardCameraUI(true);
        }
        else if (camera == playCamera)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            ActiveCarCameraUI(true);
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
    private void InitializeTextDetails(string _carName, string _carDescription)
    {
        carNameText.text = _carName;
        carDescriptionText.text = _carDescription;
    }
    private void InitializeStats(StatType _type, int currentUpgrade, int maxUpgrade)
    {
        Transform _stats = null;

        switch (_type)
        {
            case StatType.Power:
                _stats = powerStats;
                break;
            case StatType.Handling:
                _stats = handlingStats;
                break;
            case StatType.Resistance:
                _stats = resistanceStats;
                break;
            default:
                break;
        }

        DeleteAllChildren(_stats);

        for (int i = 0; i < currentUpgrade; i++)
        {
            Instantiate(UpgradedStatPrefab, _stats);
        }
        for (int i = currentUpgrade + 1; i < maxUpgrade; i++)
        {
            Instantiate(NotUpgradedStatPrefab, _stats);
        }
    }
    private void DeleteAllChildren(Transform _parent)
    {
        while(_parent.childCount > 0)
        {
            DestroyImmediate(_parent.GetChild(0));
        }
    }
    private void ActiveStandardCameraUI(bool _state)
    {
        standardInstruction.gameObject.SetActive(_state);

        ActiveCarCameraUI(!_state);
    }
    private void ActiveCarCameraUI(bool _state)
    {
        carDetails.SetActive(_state);
        playButton.gameObject.SetActive(_state);

        ActiveStandardCameraUI(!_state);

        if(_state == true)
        {
            InitializeTextDetails(currentCarDetails.carName, currentCarDetails.carDescription);
            InitializeStats(StatType.Power, currentCarDetails.power, currentCarDetails.maxPower);
            InitializeStats(StatType.Handling, currentCarDetails.handling, currentCarDetails.maxHandling);
            InitializeStats(StatType.Resistance, currentCarDetails.resistance, currentCarDetails.maxResistance);
        }
    }
}
public enum StatType
{
    Power,
    Handling,
    Resistance
}