using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;

public class MenuManager : MonoBehaviour
{
    [Header("Standard Menu")]
    [SerializeField] TMP_Text standardInstruction;
    [SerializeField] CinemachineVirtualCamera standardCamera;

    [Header("Car Menu")]
    [SerializeField] Button playButton;
    [SerializeField] CinemachineVirtualCamera playCamera;
    [SerializeField] Transform carsTransform;
    [SerializeField] GameObject carDetails;
    [SerializeField] TMP_Text carNameText;
    [SerializeField] TMP_Text carDescriptionText;
    [SerializeField] TMP_Text powerupDescriptionText;
    [SerializeField] TMP_Text powerupNameText;
    [SerializeField] Transform powerStats;
    [SerializeField] Transform handlingStats;
    [SerializeField] Transform resistanceStats;
    [SerializeField] Transform speedStats;
    [SerializeField] Button leftArrowButton;
    [SerializeField] Button rightArrowButton;

    [Header("Prefabs")]
    [SerializeField] GameObject UpgradedStatPrefab;
    [SerializeField] GameObject NotUpgradedStatPrefab;

    private CarDetails currentCarDetails;
    private CinemachineVirtualCamera currentCarCamera;
    private CinemachineBrain brain;

    private int _currentCarIndex = -1;

    private void OnEnable()
    {
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(OnPlayButtonPressed);

        leftArrowButton.onClick.RemoveAllListeners();
        leftArrowButton.onClick.AddListener(OnLeftArrowPressed);

        rightArrowButton.onClick.RemoveAllListeners();
        rightArrowButton.onClick.AddListener(OnRightArrowPressed);

        InitializePowerupDetails("","");
    }
    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
        leftArrowButton.onClick.RemoveAllListeners();
        rightArrowButton.onClick.RemoveAllListeners();
    }
    void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        standardCamera.Priority = 10;
        ActiveStandardCameraUI(true);
    }
    void Update()
    {
        CheckFocusCarCamera();
        CheckStandardCamera();
        CheckArrowControls();
    }
    private void OnLeftArrowPressed()
    {
        ChangeCarPaint(true);
    }
    private void OnRightArrowPressed()
    {
        ChangeCarPaint(false);
    }
    private void ChangeCarPaint(bool left)
    {
        Transform currentCarTransform = currentCarDetails.transform;

        // Use currentCarTransform as the parent transform
        Transform parent = currentCarTransform;

        int childCount = parent.childCount - 1;
        if (childCount == 0) return; // If no children, exit early

        // Step 1: Find the currently active child
        int activeIndex = -1;
        for (int i = 0; i < childCount; i++)
        {
            if (parent.GetChild(i).gameObject.activeSelf)
            {
                activeIndex = i;
                break;
            }
        }

        // Step 2: Deactivate all children
        for (int i = 0; i < childCount; i++)
        {
            parent.GetChild(i).gameObject.SetActive(false);
        }

        // Step 3: Calculate the new index (either previous or next)
        int newIndex;
        if (left)
        {
            // Move left (previous index), wrapping around if needed
            newIndex = (activeIndex - 1 + childCount) % childCount;
        }
        else
        {
            // Move right (next index), wrapping around if needed
            newIndex = (activeIndex + 1) % childCount;
        }

        // Step 4: Activate the new child
        var _child = parent.GetChild(newIndex);
        _child.gameObject.SetActive(true);
        SetCar();
    }

    public void SetCar()
    {
        Transform currentCarTransform = currentCarDetails.transform;

        // Use currentCarTransform as the parent transform
        Transform parent = currentCarTransform;

        int childCount = parent.childCount - 1;
        if (childCount == 0) return; // If no children, exit early

        int activeIndex = -1;
        for (int i = 0; i < childCount; i++)
        {
            if (parent.GetChild(i).gameObject.activeSelf)
            {
                activeIndex = i;
                break;
            }
        }

        currentCarDetails.SetColor(parent.GetChild(activeIndex).gameObject.GetComponent<MeshFilter>().mesh);

        CarSelectionManager.Instance.SelectCar(currentCarDetails);
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
                    _currentCarIndex = GetIndexInParent(hit.collider.gameObject.transform);
                    if (currentCarCamera == null)
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
            _currentCarIndex = -1;
            StartCoroutine(WaitForTransition(standardCamera));
        }
    }
    private void SetNewCarCamera(int _carIndex)
    {
        _currentCarIndex = _carIndex; 
        Debug.Log("new car index" + _currentCarIndex);
        if (currentCarCamera == null)
        {
            standardCamera.Priority= 0;
        }
        else
        {
            currentCarCamera.Priority = 0;
        }
        currentCarDetails = carsTransform.GetChild(_carIndex).gameObject.GetComponent<CarDetails>();
        SetCar();
        currentCarCamera = carsTransform.GetChild(_carIndex).gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        if (currentCarCamera == null)
        {
            return;
        }
        currentCarCamera.Priority = 10;
        StartCoroutine(WaitForTransition(currentCarCamera));
    }
    private void CheckArrowControls()
    {
        int _newIndex = -1;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_currentCarIndex == -1)
            {
                _newIndex = 0;
            }
            else
            {
                _newIndex = _currentCarIndex - 2;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_currentCarIndex == -1)
            {
                _newIndex = 0;
            }
            else
            {
                _newIndex = _currentCarIndex + 2;
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_currentCarIndex == -1)
            {
                _newIndex = 0;
            }
            else
            {
                _newIndex = _currentCarIndex - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_currentCarIndex == -1)
            {
                _newIndex = 0;
            }
            else
            {
                _newIndex = _currentCarIndex + 1;
            }
        }
        
        
        if (_newIndex != -1)
        {
            _newIndex = Mathf.Clamp(_newIndex, 0, carsTransform.childCount - 1);
            SetNewCarCamera(_newIndex);
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
        carDetails.SetActive(false);
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
    
    private void InitializePowerupDetails(string powerupName, string _powerupDescription)
    {
        powerupDescriptionText.text = _powerupDescription;
        powerupNameText.text = powerupName;
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
            case StatType.Speed:
                _stats = speedStats;
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
        List<GameObject> children = new List<GameObject>();

        // Collect all children
        foreach (Transform child in _parent)
        {
            children.Add(child.gameObject);
        }

        // Now destroy them
        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }
    }
    private void ActiveStandardCameraUI(bool _state)
    {
        standardInstruction.gameObject.SetActive(_state);

        carDetails.SetActive(!_state);
        playButton.gameObject.SetActive(!_state);
    }
    private void ActiveCarCameraUI(bool _state)
    {
        carDetails.SetActive(_state);
        playButton.gameObject.SetActive(_state);

        standardInstruction.gameObject.SetActive(!_state);

        if (_state == true)
        {
            SetCar();
            InitializeTextDetails(currentCarDetails.carName, currentCarDetails.carDescription);
            InitializePowerupDetails(currentCarDetails.powerupName,currentCarDetails.powerupDescription);
            InitializeStats(StatType.Power, currentCarDetails.power, currentCarDetails.maxPower);
            InitializeStats(StatType.Handling, currentCarDetails.handling, currentCarDetails.maxHandling);
            InitializeStats(StatType.Resistance, currentCarDetails.resistance, currentCarDetails.maxResistance);
            InitializeStats(StatType.Speed, currentCarDetails.speed, currentCarDetails.maxSpeed);
        }
    }
    private int GetIndexInParent(Transform child)
    {
        Transform parent = child.parent;

        if (parent == null)
        {
            Debug.LogError("The provided transform has no parent.");
            return -1; // Return an invalid index if there's no parent
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i) == child)
            {
                return i; // Return the index when the child is found
            }
        }

        Debug.LogError("The transform is not a child of its parent.");
        return -1; // Return an invalid index if the child is not found
    }
}
public enum StatType
{
    Power,
    Handling,
    Resistance,
    Speed
}