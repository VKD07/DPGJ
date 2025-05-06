using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class Gun : MonoBehaviour
    {
        [Header("Attack Gun Settings")]
        [SerializeField] public float _gunDamage = 5f;
        public bool DisableGun;

        [SerializeField] private bool _disableFriendlyFire;
        [SerializeField] private float _attackInterval = 0.3f;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] public Transform _bulletSpawnPoint;

        private float _lastAttackTime;
        private Vector3 _targetPoint;
        private Vector3 _direction;

        [Header("Water Gun Settings")]
        [SerializeField] private float _waterStoragePercent = 100f;
        [SerializeField] private float _waterGunReductionStr = 10f;
        [SerializeField] private float _fireExtinguishStr = 20f;
        [SerializeField] private float _waterRange = 50f;

        [Header("Other Settings")]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _rayDistance = 50f;

        private Ray _attackGunRay;
        private Ray _waterGunRay;

        private PlayerInput _playerInput;
        private InputAction _attackAction;
        private InputAction _waterGunAction;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _attackAction = _playerInput.actions["Attack"];
            _waterGunAction = _playerInput.actions["WaterGun"];
        }

        private void Update()
        {
            if (!DisableGun)
            {
                if (_attackAction.IsPressed() && Time.time >= _lastAttackTime + _attackInterval)
                {
                    ShootEnemies();
                    _lastAttackTime = Time.time;
                }
            }

            if (_waterGunAction.IsPressed())
            {
                ShootWater();
            }
        }

        private void ShootEnemies()
        {
            Vector2 camCenter = _playerCamera.pixelRect.center;
            _attackGunRay = _playerCamera.ScreenPointToRay(camCenter);

            Bullet bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);

            _targetPoint = _bulletSpawnPoint.position + _playerCamera.transform.forward * _rayDistance;

            bullet.SetupTarget(_targetPoint);

            if (Physics.Raycast(_attackGunRay, out RaycastHit hit, _rayDistance))
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    damageable.OnDamageTaken(_gunDamage);
                }

                if (!_disableFriendlyFire && hit.transform.TryGetComponent(out PlayerDeathHandler playerDeathHandler))
                {
                    playerDeathHandler.KillPlayer();
                }
            }
        }

        private void ShootWater()
        {
            Vector2 camCenter = _playerCamera.pixelRect.center;
            _waterGunRay = _playerCamera.ScreenPointToRay(camCenter);
            _waterStoragePercent -= _waterGunReductionStr * Time.deltaTime;

            if (Physics.Raycast(_waterGunRay, out RaycastHit hit, _waterRange))
            {
                if (hit.transform.TryGetComponent(out IBurnable burnable))
                {
                    if (_waterStoragePercent > 0)
                    {
                        burnable.Extinguish(_fireExtinguishStr);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_attackGunRay.origin, _attackGunRay.direction * _rayDistance);
        }

        public void RefillWater()
        {
            _waterStoragePercent = 100;
        }
    }
}
