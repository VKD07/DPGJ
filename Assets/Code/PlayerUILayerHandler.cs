using UnityEngine;

namespace Code
{
    public class PlayerUILayerHandler : MonoBehaviour
    {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private GameObject[] _objectsToChangeLayer;
        private PlayerSpawn _playerSpawn;

        private void Awake()
        {
            _playerSpawn = FindAnyObjectByType<PlayerSpawn>();
        }

        private void OnEnable()
        {
            if (_playerSpawn == null)
                return;

            int screenUILayer = LayerMask.NameToLayer("ScreenUI");
            int targetLayer = -1;

            if (_playerSpawn._playerCount == 0)
                targetLayer = LayerMask.NameToLayer("Player1UI");
            else if (_playerSpawn._playerCount == 2)
                targetLayer = LayerMask.NameToLayer("Player2UI");

            if (targetLayer == -1 || screenUILayer == -1)
                return;

            _uiCamera.cullingMask = (1 << screenUILayer) | (1 << targetLayer);

            foreach (var obj in _objectsToChangeLayer)
            {
                if (obj != null)
                    obj.layer = targetLayer;
            }
        }
    }
}