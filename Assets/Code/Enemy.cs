using System;
using System.Collections.Generic;
using Code.PowerUps;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code
{
    public abstract class Enemy : MonoBehaviour, IDamageable, IDestroyable, ILootable
    {
        [SerializeField] protected float _health;

        [Header("Dropped Loot Settings")] [SerializeField]
        private float _dropPercentage = 0.5f;

        private int _randomIndex;
        private LootPowerUpsManager _lootPowerUpsManager;
        private readonly List<GameObject> _lootPoolBuffer = new List<GameObject>();

        private void Awake()
        {
            _lootPowerUpsManager = FindAnyObjectByType<LootPowerUpsManager>();
        }

        public void OnDamageTaken(float value)
        {
            if (_health <= 0)
            {
                OnDestroy();
                return;
            }

            _health -= value;
        }

        public void OnDestroy()
        {
            _health = 0;
            gameObject.SetActive(false);
            SpawnLoot();
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