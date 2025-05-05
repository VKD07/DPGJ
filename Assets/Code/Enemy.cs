using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Code
{
    public abstract class Enemy : MonoBehaviour, IDamageable, IDestroyable
    {
        [SerializeField] private float _health;

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
            Destroy(gameObject);
        }
    }
}
