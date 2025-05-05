
using UnityEngine;

namespace Code
{
    public interface IFactory<T>
    {
        public T Spawn(T item, Vector3 position, Quaternion rotation);
        public void DestroyItem(T item);
    }
}
