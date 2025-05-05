using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code
{
    public abstract class FactoryPool<T> : MonoBehaviour, IFactory<T> where T : Object
    {
        [SerializeField] private int _numberOfProducts;
        [SerializeField] private T _prefab;
        protected List<T> Products { get; } = new List<T>();

        protected void Awake()
        {
            for (int i = 0; i < _numberOfProducts; i++)
            {
               T obj = Spawn(_prefab, Vector3.zero, Quaternion.identity);
            }
        }

        public T Spawn(T item, Vector3 position, Quaternion rotation)
        {
            T obj = Instantiate(item, position, rotation);
            Products.Add(obj);
            return obj;
        }
        

        public void DestroyItem(T item)
        {
            Products.Remove(item);
            Destroy(item);
        }
    }
}