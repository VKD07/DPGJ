using Code;
using UnityEngine;

public class DroneSpawner : Spawner<DroneEnemy>
{
    [SerializeField] private BuildingManager _buildingManger;
    
    protected override void OnProductSpawned(IProduct product, int randomPositionIndex)
    {
        product.SpawnSetup(ChooseATargetBuilding(), _spawnPoints[randomPositionIndex].position, Quaternion.identity);
    }
   
    private Vector3 ChooseATargetBuilding()
    {
        for (int i = 0; i < _buildingManger.AllBuildings.Count; i++)
        {
            if (!_buildingManger.AllBuildings[i].IsBurning)
            {
                return _buildingManger.AllBuildings[i].transform.position;
            }
        }
        return _buildingManger.AllBuildings[0].transform.position;
    }
}