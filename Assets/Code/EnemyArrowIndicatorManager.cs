using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code
{
    public class EnemyArrowIndicatorManager : FactoryPool<GameObject>
    {
        [SerializeField] private Transform _uiParent;
        [SerializeField] private Transform _player;

        private DroneSpawner _droneSpawner;

        private readonly Dictionary<Transform, RectTransform> _enemiesToTrack =
            new Dictionary<Transform, RectTransform>();


        private void OnEnable()
        {
            _droneSpawner = FindAnyObjectByType<DroneSpawner>(); //Not satisfied
        }

        private void Start()
        {
            InitProducts();
        }

        private void Update()
        {
            TrackAllEnemiesWithArrow();
        }

        private void TrackAllEnemiesWithArrow()
        {
            foreach (var enemy in _enemiesToTrack)
            {
                if (enemy.Key.gameObject.activeSelf)
                {
                    TrackEnemyWithArrow(enemy.Key, enemy.Value);
                    return;
                }

                enemy.Value.gameObject.SetActive(false);
            }
        }

        private void TrackEnemyWithArrow(Transform toTrack, RectTransform arrowUI)
        {
            if (!_player) return;

            arrowUI.gameObject.SetActive(true);

            Vector3 enemy = toTrack.position - _player.position;
            Vector3 forward = _player.forward;
            Vector3 right = _player.right;

            float x = Vector3.Dot(enemy, right);
            float y = Vector3.Dot(enemy, forward);
            float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;

            arrowUI.localEulerAngles = new Vector3(0, 0, -angle);
        }

        private void InitProducts()
        {
            for (int i = 0; i < Products.Count; i++)
            {
                Products[i].transform.SetParent(_uiParent);
                Products[i].GetComponent<RectTransform>().position = Vector3.zero;
                Products[i].gameObject.SetActive(false);

                //Creating a dictionary to assign each enemy with their own arrow ui
                _enemiesToTrack.Add(_droneSpawner.Products[i].transform, Products[i].GetComponent<RectTransform>());
            }
        }
    }
}