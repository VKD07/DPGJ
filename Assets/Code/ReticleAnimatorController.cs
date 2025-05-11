using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    public class ReticleAnimatorController : MonoBehaviour
    {
        public Animator Animator;

        public void SetBoolShootingReticle(bool val)
        {
            Animator.SetBool("ShootingReticle", val);
        }

        public void SetBoolEnemyDetectedReticle(bool val)
        {
            Animator.SetBool("TriggerReticle", val);
        }
    }
}