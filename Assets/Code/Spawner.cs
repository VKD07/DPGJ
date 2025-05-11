using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Code
{
    public abstract class Spawner<T> : FactoryPool<T> where T : MonoBehaviour
    {
        [SerializeField] protected Transform[] _spawnPoints;
        [SerializeField] protected Vector2 _timeBetweenSpawns;
        [SerializeField] protected bool _hideOnSpawn = true;
        private float _randomTime;
        private int _randomPositionIndex;

        public Action<IProduct, int> OnSpawned;

        protected override void Awake()
        {
            base.Awake();
            if (_hideOnSpawn == false)
            {
                return;
            }

            for (int i = 0; i < Products.Count; i++)
            {
                Products[i].gameObject.SetActive(false);
            }
        }

        protected virtual void OnEnable()
        {
            OnSpawned += OnProductSpawned;
        }

        protected virtual void OnDisable()
        {
            OnSpawned -= OnProductSpawned;
        }

        protected virtual void Start()
        {
            StartCoroutine(SpawnDraw());
        }

        private IEnumerator SpawnDraw()
        {
            while (true)
            {
                _randomTime = Random.Range(_timeBetweenSpawns.x, _timeBetweenSpawns.y);
                _randomPositionIndex = Random.Range(0, _spawnPoints.Length);
                yield return new WaitForSeconds(_randomTime);

                for (int i = 0; i < Products.Count; i++)
                {
                    if (!Products[i].gameObject.activeSelf)
                    {
                        if (Products[i].TryGetComponent(out IProduct product))
                        {
                            OnSpawned?.Invoke(product, _randomPositionIndex);
                            Products[i].gameObject.SetActive(true);
                        }

                        break;
                    }
                }
            }
        }

        protected abstract void OnProductSpawned(IProduct product, int randomPositionIndex);
    }
}