using System;
using UnityEngine;

namespace Code
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        [SerializeField] private float _deathTime = 5f;
        [SerializeField] private GameObject _visuals;
        private Vector3 _deathPosition;
        private float _currentTime;
        private bool _isDead;

        private MonoBehaviour[] playerScripts;

        private void Awake()
        {
            playerScripts = GetComponents<MonoBehaviour>();
        }

        private void Update()
        {
            DeathTimer();
        }

        private void DeathTimer()
        {
            if (_isDead)
            {
                if (_currentTime < _deathTime)
                {
                    _currentTime += Time.deltaTime;
                    return;
                }
                _currentTime = 0;
                RevivePlayer();
            }
        }

        public void KillPlayer()
        {
            _deathPosition = transform.position;
            _isDead = true;
            SetEnablePlayerScripts(false);
            _visuals.SetActive(false);
        }

        public void RevivePlayer()
        {
            _isDead = false;
            transform.position = _deathPosition + (Vector3.up * 40f);
            SetEnablePlayerScripts(true);
            _visuals.SetActive(true);
        }

        private void SetEnablePlayerScripts(bool val)
        {
            for (int i = 0; i < playerScripts.Length; i++)
            {
                if (playerScripts[i] == this)
                {
                    continue;
                }
                playerScripts[i].enabled = val;
            }
        }
    }
}