using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class Gun : MonoBehaviour
    {
        [Header("Attack Gun Settings")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _attackInterval = 0.3f;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletSpawnPoint;
        private float _lastAttackTime;
        private Vector3 _targetPoint;
        private Vector3 _direction;

        [Header("Attack Gun Settings")] [SerializeField]
        private float _waterGunStrength = 15f;
        
        
        [Header("Other Settings")]
        [SerializeField] private InputActionReference _attackInput;

        [SerializeField] private InputActionReference _waterGunInput;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _rayDistance = 50f;
        private Ray _ray;
        
        private void Update()
        {
            if (_attackInput.action.IsPressed() && Time.time >= _lastAttackTime + _attackInterval)
            {
                ShootEnemies();
                _lastAttackTime = Time.time;
            }

            if (_waterGunInput.action.IsPressed())
            {
                ShootWater();
            }
            
        }

        private void ShootEnemies()
        {
            _ray = _playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            
            Bullet bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
            
            _targetPoint = _bulletSpawnPoint.position + _playerCamera.transform.forward * _rayDistance;

            bullet.SetupTarget(_targetPoint);
            
            if (Physics.Raycast(_ray, out RaycastHit hit, _rayDistance))
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    damageable.OnDamageTaken(_damage);
                }
            }
        }

        private void ShootWater()
        {
            _ray = _playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            
            if (Physics.Raycast(_ray, out RaycastHit hit, _rayDistance))
            {
                if (hit.transform.TryGetComponent(out IBurnable burnable))
                {
                    burnable.Extinguish(_waterGunStrength);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_ray.origin , _ray.direction * _rayDistance);
        }
    }
}