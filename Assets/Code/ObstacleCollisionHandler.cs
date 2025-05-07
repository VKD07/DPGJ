using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Code
{
    public class ObstacleCollisionHandler : MonoBehaviour
    {
        [SerializeField] private PlayerDeathHandler _playerDeathHandler;
        [SerializeField] private ParticleSystem _explosionParticle;
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out IObstacle obstacle))
            {
                obstacle.OnCollided();
                _explosionParticle.Play();
                _playerDeathHandler.KillPlayer();
            }
        }
    }
}