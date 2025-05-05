using System;
using UnityEngine;

namespace Code
{
    public class Bullet : MonoBehaviour, IDestroyable
    {
        [SerializeField] private float _moveSpeed = 500f;
        private Vector3 _targetPosition;
        
        public void SetupTarget(Vector3 target)
        {
            _targetPosition = target;    
        }
        
        private void Update()
        {
            MoveForward();
        }

        private void MoveForward()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
        }

        public void OnDestroy()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            OnDestroy();
        }
    }
}