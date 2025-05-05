using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class Gun : MonoBehaviour
    {
        [Header("Gun Settings")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _attackInterval = 0.3f;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletSpawnPoint;
        private float _lastAttackTime;
        private Vector3 _targetPoint;
        private Vector3 _direction;

        [Header("Other Settings")]
        [SerializeField] private InputActionReference _attackInput;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _rayDistance = 50f;
        private Ray _ray;
        private void Update()
        {
            if (_attackInput.action.IsPressed() && Time.time >= _lastAttackTime + _attackInterval)
            {
                Shoot();
                _lastAttackTime = Time.time;
            }
        }

        private void Shoot()
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

                // Optional: Add tracer or hit effect here
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_ray.origin , _ray.direction * _rayDistance);
        }
    }
}