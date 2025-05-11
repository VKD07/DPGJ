using UnityEngine;

namespace Code
{
    public interface IDamageable
    {
        public void OnDamageTaken(float value, PlayerDroneKillHandler damageTakenHandler);
    }
}