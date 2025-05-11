using System;
using UnityEngine;

namespace Code.PowerUps
{
    public class DamagePowerUp : PowerUpLimitedTime
    {
        [SerializeField] private float _IncreaseDamageTime = 10f;
        private float _currentTime;
        private float _initialDamage;
        private bool _isActivated;
        private Gun _gun;

        private void Update()
        {
            RunIncreaseDamageTimer();
        }

        private void RunIncreaseDamageTimer()
        {
            if (!_isActivated || _gun == null)
            {
                return;
            }
            
            if (_currentTime < _IncreaseDamageTime)
            {
                _currentTime += Time.deltaTime;
                return;
            }

            _gun._gunDamage = _initialDamage;
            _isActivated = false;
            _currentTime = 0;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out _gun))
            {
                _initialDamage = _gun._gunDamage;
                _gun._gunDamage = 1000f;
                _isActivated = true;
                gameObject.SetActive(false);
            }
        }
    }
}