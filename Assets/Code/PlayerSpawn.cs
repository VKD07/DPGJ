using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class PlayerSpawn : MonoBehaviour
    {
        public Transform[] _spawnPoints;
        [SerializeField] private TextMeshProUGUI _player1Ready;
        [SerializeField] private TextMeshProUGUI _player2Ready;
        [SerializeField] private GameObject _pressStartToReady;
        [SerializeField] private GameObject _playerTimerPanel;
        [SerializeField] private GameObject _gameCamera;
        [SerializeField] private GameObject _gameplayManagers;
        [SerializeField] private GameObject _playerJoinPanel;
        [SerializeField] private GameObject _titleImage;

        public int _playerCount { get; private set; }
        private bool _gameStarted;

        public List<PlayerDroneKillHandler> playerKillHandlers = new List<PlayerDroneKillHandler>();
        private List<PlayerInput> _joinedPlayers = new List<PlayerInput>();

        private void Awake()
        {
            _playerTimerPanel.SetActive(false);
        }

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            if (_playerCount == 0)
            {
                _player1Ready.text = "Player 1 Ready!";
                // playerInput.transform.GetComponent<PlayerDeathHandler>().SetEnablePlayerScripts(false);
            }
            else if (_playerCount == 1)
            {
                _player2Ready.text = "Player 2 Ready!";
                // playerInput.transform.GetComponent<PlayerDeathHandler>().SetEnablePlayerScripts(true);
                _playerTimerPanel.SetActive(true);
                _gameplayManagers.SetActive(true);
                _gameCamera.SetActive(false);
                _playerJoinPanel.SetActive(false);
                _titleImage.SetActive(false);
            }

            // playerInput.gameObject.SetActive(false);
            playerInput.transform.position = _spawnPoints[_playerCount].position;

            _joinedPlayers.Add(playerInput);
            playerKillHandlers.Add(playerInput.GetComponent<PlayerDroneKillHandler>());
            _playerCount++;
        }

        private void Update()
        {
            // if (_playerCount == 2 && !_gameStarted)
            // {
            //     foreach (var playerInput in _joinedPlayers)
            //     {
            //         var joinAction = playerInput.actions["Join"];
            //         if (joinAction != null && joinAction.WasPerformedThisFrame())
            //         {
            //             _gameStarted = true;
            //             _playerTimerPanel.SetActive(true);
            //             _gameplayManagers.SetActive(true);
            //             _gameCamera.SetActive(true);
            //
            //             foreach (var handler in playerKillHandlers)
            //                 handler.gameObject.SetActive(true);
            //             break;
            //         }
            //     }
            // }
        }
    }
}
