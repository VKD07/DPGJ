using UnityEngine;

namespace Code
{
    public class DebrisSpawner : MonoBehaviour
    {
        public GameObject cubePrefab;
        public int debrisCount = 20;
        public float spawnRadius = 1f;
        public float explosionForce = 500f;
        public float explosionRadius = 5f;

        public void SpawnDebris()
        {
            for (int i = 0; i < debrisCount; i++)
            {
                Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
                GameObject debris = Instantiate(cubePrefab, randomPos, Random.rotation);
                Rigidbody rb = debris.GetComponent<Rigidbody>();
                Vector3 explosionDir = (randomPos - transform.position).normalized;
                rb.AddForce(explosionDir * explosionForce);
            }
        }
    }
}