using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private Transform [] _spawnPoints;
        public int _playerCount { get; private set; }

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            playerInput.transform.position = _spawnPoints[_playerCount].position;
            _playerCount++;
        }
    }
}
