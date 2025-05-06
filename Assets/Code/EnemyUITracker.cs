using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class EnemyUITracker : MonoBehaviour
    {
        public DroneSpawner droneSpawner;
        public Camera cam;
        [SerializeField] private Canvas playerCanvas;
        [SerializeField] private GameObject _trackingUI;

        private readonly Dictionary<GameObject, GameObject> _enemyAndUIList = new Dictionary<GameObject, GameObject>();
        private readonly Plane[] _cameraFrustumPlanes = new Plane[6];

        public List<GameObject> VisibleEnemiesOnCam = new List<GameObject>();

        private void Update()
        {
            if (droneSpawner == null || cam == null) return;

            VisibleEnemiesOnCam.Clear();
            GeometryUtility.CalculateFrustumPlanes(cam, _cameraFrustumPlanes);

            foreach (DroneEnemy target in droneSpawner.Products)
            {
                if (target == null) continue;

                GameObject enemyObj = target.gameObject;
                if (enemyObj == null) continue;

                Renderer renderer = enemyObj.GetComponent<Renderer>();
                if (renderer == null) continue;

                bool isVisible = GeometryUtility.TestPlanesAABB(_cameraFrustumPlanes, renderer.bounds);

                if (!_enemyAndUIList.TryGetValue(enemyObj, out GameObject ui))
                {
                    ui = Instantiate(_trackingUI, playerCanvas.transform);
                    _enemyAndUIList.Add(enemyObj, ui);
                }

                if (isVisible && enemyObj.activeInHierarchy)
                {
                    VisibleEnemiesOnCam.Add(enemyObj);
                    ui.transform.position = cam.WorldToScreenPoint(enemyObj.transform.position);
                    if (!ui.activeSelf)
                    {
                        ui.SetActive(true);
                    }
                }
                else
                {
                    if (ui.activeSelf)
                    {
                        ui.SetActive(false);
                    }
                }
            }
        }
    }
}
