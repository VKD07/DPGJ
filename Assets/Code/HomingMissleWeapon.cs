using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    [RequireComponent(typeof(EnemyUITracker), typeof(Gun), typeof(PlayerInput))]
    public class HomingMissleWeapon : FactoryPool<HomingMissle>
    {
        [SerializeField] private int _numOfMisslesAvailable = 10;
        [SerializeField] private float _missleSpeed = 30f;
        [SerializeField] private float _missleDamage = 1000f;

        public bool IsActivated;

        private EnemyUITracker _enemyUITracker;
        private Gun _playergun;
        private InputAction _attackAction;
        private int _initMissleAmount;

        protected override void Awake()
        {
            base.Awake();
            _enemyUITracker = GetComponent<EnemyUITracker>();
            _playergun = GetComponent<Gun>();

            var playerInput = GetComponent<PlayerInput>();
            _attackAction = playerInput.actions["Attack"];

            _initMissleAmount = _numOfMisslesAvailable;
        }

        private void OnEnable()
        {
            for (int i = 0; i < Products.Count; i++)
            {
                Products[i].gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (!IsActivated)
                return;

            if (_numOfMisslesAvailable > 0 && _attackAction.WasPressedThisFrame())
            {
                _numOfMisslesAvailable--;
                ActivateMissle();
            }

            if (_numOfMisslesAvailable <= 0)
            {
                IsActivated = false;
                _playergun.DisableGun = false;
            }
        }

        private void ActivateMissle()
        {
            _playergun.DisableGun = true;

            for (int i = 0; i < Products.Count; i++)
            {
                if (!Products[i].gameObject.activeSelf)
                {
                    if (_enemyUITracker.VisibleEnemiesOnCam.Count == 0)
                        return;

                    int targetIndex = (_initMissleAmount - _numOfMisslesAvailable - 1) % _enemyUITracker.VisibleEnemiesOnCam.Count;
                    GameObject targetEnemy = _enemyUITracker.VisibleEnemiesOnCam[targetIndex];

                    Products[i].gameObject.SetActive(true);
                    Products[i].OnSpawn(
                        _playergun._bulletSpawnPoint.position,
                        targetEnemy.transform,
                        _missleSpeed,
                        _missleDamage
                    );
                    return;
                }
            }
        }

        public void ReloadMissles()
        {
            _numOfMisslesAvailable = _initMissleAmount;
        }
    }
}