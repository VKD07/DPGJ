using System;
using UnityEngine;

namespace Code
{
    public class BulletPool : FactoryPool<Bullet>
    {
        private void OnEnable()
        {
            for (int i = 0; i < Products.Count; i++)
            {
                Products[i].gameObject.SetActive(false);
            }
        }

        public Bullet GetProductFromPool()
        {
            for (int i = 0; i < Products.Count; i++)
            {
                if (!Products[i].gameObject.activeSelf)
                {
                    Products[i].gameObject.SetActive(true);
                    return Products[i];
                }
            }
            return null;
        }
    }
}