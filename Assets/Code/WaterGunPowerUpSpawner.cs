using Code.PowerUps;
using UnityEngine;

namespace Code
{
    public class WaterGunPowerUpSpawner : Spawner<WaterPowerUp>
    {
        protected override void OnProductSpawned(IProduct product, int randomPositionIndex)
        {
            product.SpawnSetup(Vector3.zero, _spawnPoints[randomPositionIndex].position, Quaternion.identity);
        }
    }
}