using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Code
{
    public class ObstacleCollisionHandler : MonoBehaviour
    {
        [SerializeField] private PlayerDeathHandler _playerDeathHandler;
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out IObstacle obstacle))
            {
                obstacle.OnCollided();
                _playerDeathHandler.KillPlayer();
            }
        }
    }
}