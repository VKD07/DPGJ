using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class EnemyUITracker : MonoBehaviour
    {
        private DroneSpawner _droneSpawner;
        public Camera cam;
        [SerializeField] private Canvas playerCanvas;
        [SerializeField] private GameObject _trackingUI;

        private readonly Dictionary<GameObject, GameObject> _enemyAndUIList = new Dictionary<GameObject, GameObject>();
        private readonly Plane[] _cameraFrustumPlanes = new Plane[6];

        public List<GameObject> VisibleEnemiesOnCam = new List<GameObject>();

        private void Awake()
        {
            _droneSpawner = FindAnyObjectByType<DroneSpawner>();
        }

        private void Update()
        {
            if (_droneSpawner == null || cam == null || playerCanvas == null) return;

            VisibleEnemiesOnCam.Clear();
            GeometryUtility.CalculateFrustumPlanes(cam, _cameraFrustumPlanes);

            foreach (DroneEnemy target in _droneSpawner.Products)
            {
                if (target == null) continue;

                GameObject enemyObj = target.gameObject;
                if (enemyObj == null) continue;

                if (!_enemyAndUIList.TryGetValue(enemyObj, out GameObject ui))
                {
                    ui = Instantiate(_trackingUI, playerCanvas.transform);
                    _enemyAndUIList.Add(enemyObj, ui);
                }

                if (!enemyObj.activeInHierarchy)
                {
                    if (ui.activeSelf) ui.SetActive(false);
                    continue;
                }

                Renderer renderer = enemyObj.GetComponent<Renderer>();
                if (renderer == null) continue;

                bool isVisible = GeometryUtility.TestPlanesAABB(_cameraFrustumPlanes, renderer.bounds);

                if (isVisible)
                {
                    VisibleEnemiesOnCam.Add(enemyObj);

                    Vector2 screenPoint = cam.WorldToScreenPoint(enemyObj.transform.position);
                    RectTransform rectTransform = ui.GetComponent<RectTransform>();
                    Vector2 localPoint;

                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        playerCanvas.GetComponent<RectTransform>(),
                        screenPoint,
                        cam,
                        out localPoint))
                    {
                        rectTransform.anchoredPosition = localPoint;
                    }

                    if (!ui.activeSelf) ui.SetActive(true);
                }
                else
                {
                    if (ui.activeSelf) ui.SetActive(false);
                }
            }
        }
    }
}
