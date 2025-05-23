﻿using System;
using UnityEngine;

namespace Code
{
    public class HomingMissle : MonoBehaviour
    {
        private float _damage;
        private float _speed;
        private Transform _target;
        public ParticleSystem ExplosionParticle;
        private ExplosionPoolManager _explosionPoolManager;
        private GameObject _explosion;
        private PlayerDroneKillHandler _killHandler;

        private void Awake()
        {
            _explosionPoolManager = FindAnyObjectByType<ExplosionPoolManager>();
        }

        public void OnSpawn(Vector3 spawnPoint, Transform target, float speed, float damage,
            PlayerDroneKillHandler killHandler)
        {
            transform.position = spawnPoint;
            _killHandler = killHandler;
            _speed = speed;
            _target = target;
            _damage = damage;
        }

        public void Update()
        {
            if (_explosionPoolManager == null)
            {
                _explosionPoolManager = FindAnyObjectByType<ExplosionPoolManager>();
                return;
            }

            if (_target != null)
            {
                if (!_target.gameObject.activeSelf)
                {
                    Destroy();
                    return;
                }

                transform.LookAt(_target.position);
                transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.OnDamageTaken(_damage, _killHandler);
            }

            if (other.transform.TryGetComponent(out PlayerDeathHandler playerDeathHandler))
            {
                return;
            }

            Destroy();
        }

        private void Destroy()
        {
            if (_explosionPoolManager.GetProductFromPool() == null)
            {
                return;
            }

            _explosion = _explosionPoolManager.GetProductFromPool();

            if (_explosion == null)
            {
                return;
            }

            _explosion.transform.position = transform.position;
            gameObject.SetActive(false);
        }
    }
}