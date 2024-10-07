using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project._Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] Transform playerSpawn;
        [SerializeField] CinemachineVirtualCamera followCam;

        [Header("Game Settings")]
        public Transform[] carStartingPositions;
        public List<GameObject> cars;
        public float countdownTime = 3f;

        [Header("UI Elements")]
        public TextMeshProUGUI centerText;
        public TextMeshProUGUI gameTimerText;
        public TextMeshProUGUI remainingCarsText;
        public GameObject pauseMenuPanel;
        public GameObject gameMenuPanel;

        private float _gameTimer = 0f;
        private bool _gameStarted = false;
        private int _unDestroyedCarsCount;

        private void Start()
        {
            CarStats.destroyed += OnCarDestroyed;

            var _playerCar = CarSelectionManager.Instance.CreateCar(playerSpawn.position);
            cars.Add(_playerCar);
            followCam.Follow = _playerCar.transform;
            followCam.LookAt = _playerCar.transform;

            EnableCars(false);
            SetCarsAtStartPositions();

            _unDestroyedCarsCount = cars.Count;
            pauseMenuPanel.SetActive(false);
            gameMenuPanel.SetActive(true);
            centerText.text = "";

            StartCoroutine(StartCountdown());
        }

        void Update()
        {
            // Update the game timer if the game has started
            if (_gameStarted)
            {
                _gameTimer += Time.deltaTime;
                int minutes = Mathf.FloorToInt(_gameTimer / 60F);  // Get the total minutes
                int seconds = Mathf.FloorToInt(_gameTimer % 60F);  // Get the remaining seconds
                gameTimerText.text = $"{minutes:00}:{seconds:00}";
            }

            // Handle pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_gameStarted)
                {
                    TogglePause();
                }
            }
        }

        void SetCarsAtStartPositions()
        {
            for (int i = 0; i < cars.Count; i++)
            {
                cars[i].transform.position = carStartingPositions[i].position;
                cars[i].transform.rotation = carStartingPositions[i].rotation;
            }
        }

        IEnumerator StartCountdown()
        {
            float countdown = countdownTime;
            while (countdown > 0)
            {
                centerText.text = countdown.ToString("F0");
                yield return new WaitForSeconds(1f);
                countdown--;
            }
            centerText.text = "Go";
            yield return new WaitForSeconds(1f);
            centerText.gameObject.SetActive(false);

            StartGame();
        }

        void StartGame()
        {
            _gameStarted = true;
            _gameTimer = 0f;
            gameTimerText.gameObject.SetActive(true);
            remainingCarsText.text = $"{_unDestroyedCarsCount}/{cars.Count} cars";

            EnableCars();
        }

        private void EnableCars(bool enable = true)
        {
            // Enable movement for all cars
            for (int i = 0; i < cars.Count; i++)
            {
                Controller carController = cars[i].GetComponent<Controller>();
                if (carController != null)
                {
                    carController.enabled = enable;
                }
            }
        }

        public void TogglePause()
        {
            if (pauseMenuPanel.activeSelf)
            {
                pauseMenuPanel.SetActive(false);
                gameMenuPanel.SetActive(true);
                Time.timeScale = 1f;  // Resume game
            }
            else
            {
                pauseMenuPanel.SetActive(true);
                gameMenuPanel.SetActive(false);
                Time.timeScale = 0f;  // Pause game
            }
        }

        // Restart the current game scene
        public void RestartGame()
        {
            Time.timeScale = 1f;  // Ensure normal time scale
            SceneManager.LoadScene("Game");
        }

        // Go to the main menu scene
        public void GoToMainMenu()
        {
            Time.timeScale = 1f;  // Ensure normal time scale
            SceneManager.LoadScene("Menu");  // Replace with your main menu scene name
        }

        private void OnCarDestroyed()
        {
            _unDestroyedCarsCount--;

            remainingCarsText.text = $"{_unDestroyedCarsCount}/{cars.Count} cars";
            if (_unDestroyedCarsCount == 1)
            {
                ShowGameOver();
            }
        }

        void ShowGameOver()
        {
            _gameStarted = false;

            centerText.text = "victory";
        }
    }
}
