using UnityEngine;

namespace Code.PowerUps
{
    public class HomingMisslePowerUp : PowerUpLimitedTime, IPowerUp
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out HomingMissleWeapon homingMissle))
            {
                homingMissle.IsActivated = true;
                homingMissle.ReloadMissles();
                gameObject.SetActive(false);
            }
        }
    }
}