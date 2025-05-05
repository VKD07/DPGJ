using System.Collections;
using Code;
using UnityEngine;
using Random = UnityEngine.Random;

public class DroneSpawner : FactoryPool<DroneEnemy>
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform [] _spawnPoints;
    [SerializeField] private Vector2 _timeBetweenSpawns;
    private float randomTime;
    private int randomPositionIndex;
    

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
                    Products[i].Setup(target.position, _spawnPoints[randomPositionIndex].position, Quaternion.identity);
                    Products[i].gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}