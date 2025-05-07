using UnityEditor.Animations;
using UnityEngine;

namespace Code
{
    public class LocalPlayerHandler : MonoBehaviour
    {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private GameObject[] _objectsToChangeLayer;
        [SerializeField] private GameObject[] _reticleImageControllers;
        [SerializeField] private Gun _playerGun;
        
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
            {
                _reticleImageControllers[0].SetActive(true);
                _playerGun._reticleAnimatorController = _reticleImageControllers[0].GetComponent<ReticleAnimatorController>();
                _reticleImageControllers[1].SetActive(false);
                targetLayer = LayerMask.NameToLayer("Player1UI");
            }
            else if (_playerSpawn._playerCount == 2)
            {
                _reticleImageControllers[1].SetActive(true);
                _playerGun._reticleAnimatorController = _reticleImageControllers[1].GetComponent<ReticleAnimatorController>();
                _reticleImageControllers[0].SetActive(false);
                targetLayer = LayerMask.NameToLayer("Player2UI");
            }

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