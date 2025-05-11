using System;
using UnityEngine;

namespace Code
{
    public class DroneEnemy : Enemy, IProduct
    {
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private Rigidbody _rigidbody;
        private ExplosionPoolManager _explosionPoolManager;
        private Vector3 _target;
        private GameObject _explosion;

        protected override void Awake()
        {
            base.Awake();
            _explosionPoolManager = FindAnyObjectByType<ExplosionPoolManager>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnDeath += SpawnExplosion;
        }

        protected override void OnDisable()
        {
            base.OnEnable();
            OnDeath -= SpawnExplosion;
        }

        public void SpawnSetup(Vector3 target, Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
            _target = target;
            transform.LookAt(_target);
        }

        private void Update()
        {
            _rigidbody.linearVelocity = (_target - transform.position).normalized * _moveSpeed;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out IBurnable burnable))
            {
                burnable.Ignite();
            }

            OnDestroy();
        }

        private void SpawnExplosion()
        {
            if (_explosionPoolManager != null)
            {
                _explosion = _explosionPoolManager.GetProductFromPool();
            }

            if (_explosion != null)
            {
                _explosion.transform.position = transform.position;
            }
        }
    }
}