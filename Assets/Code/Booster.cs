using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Code
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Booster : MonoBehaviour
    {
        [Header("Boost Settings")]
        [SerializeField] private float _totalBoostPercent = 100f;
        [SerializeField] private float _depletionStrength = 10f;
        [SerializeField] private float _regenerationStrength = 5f;

        private float _currentBoostPercent;
        private bool _boostDepleted;

        [Header("Movement Settings")]
        [SerializeField] private float _maxSpeed = 120f;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private InputActionReference _sprintInput;

        [Header("Camera Settings")]
        [SerializeField] private float _cameraDistance = 23.71f;
        [SerializeField] private float _fovDistance = 80f;
        [SerializeField] private float _cameraLerpSpeed = 5f;
        [SerializeField] private CinemachineThirdPersonFollow _thirdPersonFollow;
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        private float _initSpeed;
        private bool _isSprinting;

        private void Awake()
        {
            _initSpeed = _playerMovement.GetMoveSpeed();
            _currentBoostPercent = _totalBoostPercent;
        }

        private void Update()
        {
            HandleBoostInput();
            UpdateMovement();
            UpdateCamera();
        }

        private void HandleBoostInput()
        {
            bool sprintingInput = _sprintInput.action.IsPressed();

            if (_boostDepleted)
            {
                _isSprinting = false;
                RegenerateBoost();

                if (_currentBoostPercent >= _totalBoostPercent)
                {
                    _currentBoostPercent = _totalBoostPercent;
                    _boostDepleted = false;
                }

                return;
            }

            if (sprintingInput && _currentBoostPercent > 0f)
            {
                _isSprinting = true;
                _currentBoostPercent -= Time.deltaTime * _depletionStrength;

                if (_currentBoostPercent <= 0f)
                {
                    _currentBoostPercent = 0f;
                    _boostDepleted = true;
                    _isSprinting = false;
                }
            }
            else
            {
                _isSprinting = false;
                RegenerateBoost();
            }

            _currentBoostPercent = Mathf.Clamp(_currentBoostPercent, 0f, _totalBoostPercent);
        }

        private void RegenerateBoost()
        {
            _currentBoostPercent += Time.deltaTime * _regenerationStrength;
        }

        private void UpdateMovement()
        {
            float speed = _isSprinting ? _maxSpeed : _initSpeed;
            _playerMovement.SetMoveSpeed(speed);
            _playerMovement.SetAssistedMovement(_isSprinting);
        }

        private void UpdateCamera()
        {
            float targetDistance = _isSprinting ? _cameraDistance : 4f;
            float targetFOV = _isSprinting ? _fovDistance : 60f;

            _thirdPersonFollow.CameraDistance = Mathf.Lerp(
                _thirdPersonFollow.CameraDistance,
                targetDistance,
                Time.deltaTime * _cameraLerpSpeed
            );

            _cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(
                _cinemachineCamera.Lens.FieldOfView,
                targetFOV,
                Time.deltaTime * _cameraLerpSpeed
            );
        }
    }
}
