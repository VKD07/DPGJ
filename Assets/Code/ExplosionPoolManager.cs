using UnityEngine;

namespace Code
{
    public class ExplosionPoolManager : FactoryPool<ParticleSystem>
    {
        private void OnEnable()
        {
            for (int i = 0; i < Products.Count; i++)
            {
                Products[i].gameObject.SetActive(false);
            }
        }

        public GameObject GetProductFromPool()
        {
            for (int i = 0; i < Products.Count; i++)
            {
                if (!Products[i].gameObject.activeSelf)
                {
                    Products[i].gameObject.SetActive(true);
                    return Products[i].gameObject;
                }
            }
            return null;
        }
    }
}