using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

namespace Code
{
    public class Gun : MonoBehaviour
    {
        [Header("Attack Gun Settings")] [SerializeField]
        public float _gunDamage = 5f;

        public bool DisableGun;
        [SerializeField] private bool _disableFriendlyFire;
        [SerializeField] private float _attackInterval = 0.3f;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] public Transform _bulletSpawnPoint;

        private float _lastAttackTime;

        [Header("Water Gun Settings")] [SerializeField]
        private float _waterStoragePercent = 100f;

        [SerializeField] private float _waterGunReductionStr = 10f;
        [SerializeField] private float _fireExtinguishStr = 20f;
        [SerializeField] private float _waterRange = 50f;

        [Header("Other Settings")] [SerializeField]
        private Camera _playerCamera;

        [SerializeField] private float _rayDistance = 50f;

        [Header("VFX Settings")] [SerializeField]
        private VfxHandler _vfxHandler;

        public ReticleAnimatorController _reticleAnimatorController;

        [Header("UI Settings")] [SerializeField]
        private Image _waterBarOuter;

        [SerializeField] private CanvasGroup _waterBarCanvas;

        private Coroutine _fadeCoroutine;

        private Ray _attackGunRay;
        private Ray _waterGunRay;

        private PlayerInput _playerInput;
        private InputAction _attackAction;
        private InputAction _waterGunAction;
        private BulletPool _bulletPool;
        private RaycastHit _hitAttackGun;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _bulletPool = FindAnyObjectByType<BulletPool>();
            _attackAction = _playerInput.actions["Attack"];
            _waterGunAction = _playerInput.actions["WaterGun"];
        }

        private void Update()
        {
            if (!DisableGun)
            {
                Vector2 camCenter = _playerCamera.pixelRect.center;
                _attackGunRay = _playerCamera.ScreenPointToRay(camCenter);

                if (Physics.Raycast(_attackGunRay, out _hitAttackGun, _rayDistance))
                {
                    if (_hitAttackGun.transform.GetComponent<Enemy>() != null)
                    {
                        _reticleAnimatorController.SetBoolEnemyDetectedReticle(true);
                    }
                }
                else
                {
                    _reticleAnimatorController.SetBoolEnemyDetectedReticle(false);
                }

                if (_attackAction.IsPressed() && Time.time >= _lastAttackTime + _attackInterval)
                {
                    _vfxHandler.SetEnableLazerWeapon(true);
                    if (Physics.Raycast(_attackGunRay, out _hitAttackGun, _rayDistance))
                    {
                        ShootEnemies(_hitAttackGun);
                        _lastAttackTime = Time.time;
                    }
                }

                if (_attackAction.WasReleasedThisFrame())
                {
                    _reticleAnimatorController.SetBoolShootingReticle(false);
                }
            }

            if (_waterGunAction.IsPressed())
            {
                ShootWater();
                _waterBarOuter.fillAmount = _waterStoragePercent / 100f;
                if (_fadeCoroutine != null)
                    StopCoroutine(_fadeCoroutine);
                _waterBarCanvas.alpha = 1f;
            }

            if (_waterGunAction.WasReleasedThisFrame())
            {
                _vfxHandler.SetEnableWaterGun(false);
                if (_fadeCoroutine != null)
                {
                    StopCoroutine(_fadeCoroutine);
                }

                _fadeCoroutine = StartCoroutine(FadeOutWaterBar(0));
            }
        }

        private void ShootEnemies(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                _reticleAnimatorController.SetBoolShootingReticle(true);
                _reticleAnimatorController.SetBoolEnemyDetectedReticle(false);

                damageable.OnDamageTaken(_gunDamage);
            }

            if (!_disableFriendlyFire && hit.transform.TryGetComponent(out PlayerDeathHandler playerDeathHandler))
            {
                playerDeathHandler.KillPlayer();
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
                        _vfxHandler.SetEnableWaterGun(true);
                        burnable.Extinguish(_fireExtinguishStr);
                    }
                }
            }
        }

        private IEnumerator FadeOutWaterBar(float fadeDelay)
        {
            float fadeSpeed = 2f;
            yield return new WaitForSeconds(fadeDelay);
            while (_waterBarCanvas.alpha > 0)
            {
                _waterBarCanvas.alpha -= Time.deltaTime * fadeSpeed;
                yield return null;
            }

            _waterBarCanvas.alpha = 0f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_attackGunRay.origin, _attackGunRay.direction * _rayDistance);
        }

        public void RefillWater()
        {
            _waterBarOuter.fillAmount = _waterStoragePercent / 100f;
            _waterBarCanvas.alpha = 1;
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }

            _fadeCoroutine = StartCoroutine(FadeOutWaterBar(3));
            _waterStoragePercent = 100;
        }
    }
}