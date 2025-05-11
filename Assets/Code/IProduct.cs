using UnityEngine;

namespace Code
{
    public interface IProduct
    {
        public void SpawnSetup(Vector3 target, Vector3 position, Quaternion rotation);
    }
}