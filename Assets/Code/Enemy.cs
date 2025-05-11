using System;
using System.Collections.Generic;
using Code.PowerUps;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code
{
    public abstract class Enemy : MonoBehaviour, IDamageable, IDestroyable, ILootable
    {
        [SerializeField] protected float _health;
        [SerializeField] private Animator _animator;

        [Header("Dropped Loot Settings")] [SerializeField]
        private float _dropPercentage = 0.5f;

        private int _randomIndex;
        private LootPowerUpsManager _lootPowerUpsManager;
        private readonly List<GameObject> _lootPoolBuffer = new List<GameObject>();
        protected Action OnDeath;

        protected virtual void Awake()
        {
            _lootPowerUpsManager = FindAnyObjectByType<LootPowerUpsManager>();
        }

        protected virtual void OnEnable()
        {
            _health = 100;
        }

        protected virtual void OnDisable()
        {
            
        }

        public void OnDamageTaken(float value, PlayerDroneKillHandler damageTakenHandler)
        {
            _animator.SetTrigger("Hit");
            _health -= value;
            
            if (_health <= 0)
            {
                damageTakenHandler.numOfDroneKilled++;
                OnDestroy();
            }
        }

        public void OnDestroy()
        {
            OnDeath?.Invoke();
            _health = 0;
            SpawnLoot();
            gameObject.SetActive(false);
        }

        public void SpawnLoot()
        {
            if (Random.value <= _dropPercentage && _lootPowerUpsManager.PowerUps.Length > 0)
            {
                _lootPoolBuffer.Clear();
                foreach (GameObject powerUp in _lootPowerUpsManager.PowerUps)
                {
                    if (powerUp != null && !powerUp.activeSelf)
                    {
                        _lootPoolBuffer.Add(powerUp);
                    }
                }

                if (_lootPoolBuffer.Count > 0)
                {
                    int index = Random.Range(0, _lootPoolBuffer.Count);
                    GameObject selected = _lootPoolBuffer[index];
                    selected.transform.position = transform.position;
                    selected.SetActive(true);
                }
            }
        }
    }
}