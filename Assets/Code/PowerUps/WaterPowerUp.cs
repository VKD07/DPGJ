using System;
using UnityEngine;

namespace Code.PowerUps
{
    public class WaterPowerUp : MonoBehaviour, IProduct, IPowerUp
    {
        [SerializeField] private float _fallSpeed = 1f;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void SpawnSetup(Vector3 target, Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public void Update()
        {
            _rb.linearVelocity = Vector3.down * _fallSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Gun playerGun))
            {
                playerGun.RefillWater();
            }
            
            _rb.linearVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}