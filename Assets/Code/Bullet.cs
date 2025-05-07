using System;
using UnityEngine;

namespace Code
{
    public class Bullet : MonoBehaviour, IDestroyable
    {
        [SerializeField] private float _moveSpeed = 500f;
        [SerializeField] private ParticleSystem _explodeParticles;
        private Vector3 _targetPosition;
        
        public void SetupTarget(Vector3 target, Vector3 initPos, Quaternion rotation)
        {
            transform.position = initPos;
            transform.rotation = rotation;
            _targetPosition = target;    
        }
        
        private void Update()
        {
            MoveTowardsTarget();
        }

        private void MoveTowardsTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.GetComponent<PlayerMovement>() == null)
            {
                OnDestroy();
            }
        }

        public void OnDestroy()
        {
            gameObject.SetActive(false);
        }
    }
}