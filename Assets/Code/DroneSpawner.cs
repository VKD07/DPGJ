using System;
using System.Collections;
using Code;
using UnityEngine;
using Random = UnityEngine.Random;

public class DroneSpawner : FactoryPool<DroneEnemy>
{
    [SerializeField] private BuildingManager _buildingManger;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Vector2 _timeBetweenSpawns;
    private float randomTime;
    private int randomPositionIndex;

    public Action<Transform> OnSpawned;

    private void OnEnable()
    {
        for (int i = 0; i < Products.Count; i++)
        {
            Products[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnDraw());
    }

    private IEnumerator SpawnDraw()
    {
        while (true)
        {
            randomTime = Random.Range(_timeBetweenSpawns.x, _timeBetweenSpawns.y);
            randomPositionIndex = Random.Range(0, _spawnPoints.Length);
            yield return new WaitForSeconds(randomTime);

            for (int i = 0; i < Products.Count; i++)
            {
                if (!Products[i].gameObject.activeSelf)
                {
                    Products[i].Setup(ChooseATargetBuilding(), _spawnPoints[randomPositionIndex].position, Quaternion.identity);
                    Products[i].gameObject.SetActive(true);
                    OnSpawned?.Invoke(Products[i].transform);
                    break;
                }
            }
        }
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