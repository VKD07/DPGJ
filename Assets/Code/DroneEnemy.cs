using UnityEngine;

namespace Code
{
    public class DroneEnemy : Enemy
    {
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private Rigidbody _rigidbody;
        private Vector3 _target;
        
        public void Setup(Vector3 target, Vector3 position, Quaternion rotation)
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
            gameObject.SetActive(false);
        }
    }
}
