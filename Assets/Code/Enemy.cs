using Code.PowerUps;
using UnityEngine;

namespace Code
{
    public abstract class Enemy : MonoBehaviour, IDamageable, IDestroyable, ILootable
    {
        [SerializeField] protected float _health;
        
        [Header("Dropped Loot Settings")]
        [SerializeField] private GameObject [] _powerUps;
        [SerializeField] private float _dropPercentage = 0.5f;
        private int _randomIndex;
            
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
            if (Random.value <= _dropPercentage && _powerUps.Length > 0)
            {
                _randomIndex = Random.Range(0, _powerUps.Length);
                Instantiate(_powerUps[_randomIndex], transform.position, Quaternion.identity);
            }
        }
    }
}