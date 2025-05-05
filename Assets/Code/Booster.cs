using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Code
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Booster : MonoBehaviour
    {
        [SerializeField] private float _maxSpeed = 120f;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private InputActionReference _sprintInput;

        [Header("Camera Settings")]
        [SerializeField]
        private float _cameraDistance = 23.71f;
        [SerializeField]
        private float _fovDistance = 80f;

        [SerializeField] private float _cameraLerpSpeed = 5f;
        [SerializeField] private CinemachineThirdPersonFollow _thirdPersonFollow;
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        private float _initSpeed;
        private float _targetCameraDistance;
        private bool _isSprinting;

        private void Awake()
        {
            _initSpeed = _playerMovement.GetMoveSpeed();
            _targetCameraDistance = _thirdPersonFollow.CameraDistance;
        }

        private void Update()
        {
            bool sprinting = _sprintInput.action.IsPressed();
            if (sprinting != _isSprinting)
            {
                _isSprinting = sprinting;
                _targetCameraDistance = _isSprinting ? _cameraDistance : 4f;
                _playerMovement.SetMoveSpeed(_isSprinting ? _maxSpeed : _initSpeed);
            }

            _playerMovement.SetAssistedMovement(_isSprinting);
            
            _thirdPersonFollow.CameraDistance = Mathf.Lerp(
                _thirdPersonFollow.CameraDistance,
                _targetCameraDistance,
                Time.deltaTime * _cameraLerpSpeed
            );
            
            _cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(
                _cinemachineCamera.Lens.FieldOfView,
                _isSprinting ? _fovDistance : 60f,
                Time.deltaTime * _cameraLerpSpeed
            );
        }
    }
}