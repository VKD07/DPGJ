using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        [SerializeField] private float _deathTime = 5f;
        [SerializeField] private GameObject _visuals;
        [SerializeField] private GameObject _respawnPanel;
        [SerializeField] private TextMeshProUGUI _respawnNumber;
        private Vector3 _deathPosition;
        private float _currentTime;
        private bool _isDead;

        private MonoBehaviour[] playerScripts;
        private PlayerSpawn _playerSpawn;

        private void Awake()
        {
            playerScripts = GetComponents<MonoBehaviour>();
            _playerSpawn = FindAnyObjectByType<PlayerSpawn>();
            _respawnPanel.SetActive(false);

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
                    int time = Mathf.CeilToInt(_deathTime - _currentTime);
                    _respawnNumber.text = time.ToString();
                    return;
                }
                _currentTime = 0;
                RevivePlayer();
            }
        }

        public void KillPlayer()
        {
            _deathPosition = transform.position;
            _respawnPanel.SetActive(true);
            _isDead = true;
            SetEnablePlayerScripts(false);
            _visuals.SetActive(false);
        }

        public void RevivePlayer()
        {
            _respawnPanel.SetActive(false);
            _isDead = false;
            transform.position = _playerSpawn._spawnPoints[0].position;
            SetEnablePlayerScripts(true);
            _visuals.SetActive(true);
        }

        public void SetEnablePlayerScripts(bool val)
        {
            for (int i = 0; i < playerScripts.Length; i++)
            {
                if (playerScripts[i] == this || playerScripts[i] == GetComponent<PlayerInput>())
                {
                    continue;
                }
                playerScripts[i].enabled = val;
            }
        }
    }
}