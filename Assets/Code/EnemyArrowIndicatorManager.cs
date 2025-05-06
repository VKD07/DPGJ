using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class EnemyArrowIndicatorManager : FactoryPool<GameObject>
    {
        [SerializeField] private Transform _uiParent;
        [SerializeField] private Transform _player;

        private DroneSpawner _droneSpawner;
        private Dictionary<Transform, RectTransform> _trackedArrows = new();

        private void OnEnable()
        {
            _droneSpawner = FindAnyObjectByType<DroneSpawner>();
        }

        private void Start()
        {
            InitProducts();
        }

        private void Update()
        {
            UpdateTrackedEnemies();
        }

        private void InitProducts()
        {
            foreach (GameObject product in Products)
            {
                product.transform.SetParent(_uiParent, false);
                RectTransform rect = product.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
                product.SetActive(false);
            }
        }

        private void UpdateTrackedEnemies()
        {
            if (_droneSpawner == null || _player == null) return;

            foreach (DroneEnemy enemy in _droneSpawner.Products)
            {
                if (enemy == null || !enemy.gameObject.activeSelf) continue;

                if (!_trackedArrows.ContainsKey(enemy.transform))
                {
                    RectTransform arrow = GetInactiveArrowFromPool();
                    if (arrow != null)
                    {
                        _trackedArrows.Add(enemy.transform, arrow);
                    }
                }

                if (_trackedArrows.TryGetValue(enemy.transform, out RectTransform arrowUI))
                {
                    TrackEnemyWithArrow(enemy.transform, arrowUI);
                }
            }

            List<Transform> toRemove = new();
            foreach (var kvp in _trackedArrows)
            {
                if (kvp.Key == null || !kvp.Key.gameObject.activeSelf)
                {
                    kvp.Value.gameObject.SetActive(false);
                    toRemove.Add(kvp.Key);
                }
            }

            foreach (var dead in toRemove)
            {
                _trackedArrows.Remove(dead);
            }
        }

        private RectTransform GetInactiveArrowFromPool()
        {
            foreach (var obj in Products)
            {
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                    return obj.GetComponent<RectTransform>();
                }
            }
            return null;
        }

        private void TrackEnemyWithArrow(Transform toTrack, RectTransform arrowUI)
        {
            Vector3 toEnemy = toTrack.position - _player.position;
            Vector3 forward = _player.forward;
            Vector3 right = _player.right;

            float x = Vector3.Dot(toEnemy, right);
            float y = Vector3.Dot(toEnemy, forward);
            float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;

            arrowUI.localEulerAngles = new Vector3(0, 0, -angle);
        }
    }
}
