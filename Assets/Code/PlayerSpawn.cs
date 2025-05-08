using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class PlayerSpawn : MonoBehaviour
    {
        public Transform [] _spawnPoints;
        public int _playerCount { get; private set; }
        
        public List<PlayerDroneKillHandler> playerKillHandlers = new List<PlayerDroneKillHandler>();

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            playerInput.transform.position = _spawnPoints[_playerCount].position;
            playerKillHandlers.Add(playerInput.transform.GetComponent<PlayerDroneKillHandler>());
            _playerCount++;
        }
    }
}
