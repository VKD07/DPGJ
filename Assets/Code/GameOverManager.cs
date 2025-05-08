using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerValue;
        [SerializeField] private GameObject _timerPanel;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private float _maxTimeInSeconds = 240f;
        [SerializeField] private PlayerSpawn _playerSpawn;
        [SerializeField] private TextMeshProUGUI _player1Score, _player2Score;
        [SerializeField] private BuildingManager _buildingManager;
        [SerializeField] private Button _restartButton;
        private float _currentTime;
        private bool _gameOverTriggered;

        private void Awake()
        {
            _gameOverPanel.SetActive(false);
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
        }

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
            _buildingManager.ALlBuildingsDestroyed += TriggerGameOver;
        }
        
        private void OnDisable()
        {
            _buildingManager.ALlBuildingsDestroyed -= TriggerGameOver;
        }

        private void Start()
        {
            _currentTime = _maxTimeInSeconds;
            _gameOverPanel.SetActive(false);
        }

        private void Update()
        {
            if (_gameOverTriggered)
                return;

            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0f)
            {
                _currentTime = 0f;
                TriggerGameOver();
            }

            int minutes = Mathf.FloorToInt(_currentTime / 60f);
            int seconds = Mathf.FloorToInt(_currentTime % 60f);
            _timerValue.text = $"{minutes:00}:{seconds:00}";
        }

        private void TriggerGameOver()
        {
            _gameOverTriggered = true;
            Time.timeScale = 0f;
            _gameOverPanel.SetActive(true);
            _timerPanel.SetActive(false);
            _player1Score.text = _playerSpawn.playerKillHandlers[0].numOfDroneKilled.ToString();
            _player2Score.text = _playerSpawn.playerKillHandlers[1].numOfDroneKilled.ToString();
        }
    }
}