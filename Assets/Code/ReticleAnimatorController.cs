using UnityEngine;

namespace Code
{
    public class ReticleAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void SetBoolShootingReticle(bool val)
        {
            _animator.SetBool("ShootingReticle", val);
        }

        public void SetBoolEnemyDetectedReticle(bool val)
        {
            _animator.SetBool("TriggerReticle", val);
        }
    }
}