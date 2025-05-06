using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private Transform [] _spawnPoints;
        private int _playerCount;

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            playerInput.transform.position = _spawnPoints[_playerCount].position;
            _playerCount++;
        }
    }
}
