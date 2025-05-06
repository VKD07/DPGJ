using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerInput))]
    public class Booster : MonoBehaviour
    {
        [Header("Boost Settings")]
        [SerializeField] private float _totalBoostPercent = 100f;
        [SerializeField] private float _depletionStrength = 10f;
        [SerializeField] private float _regenerationStrength = 5f;

        private float _currentBoostPercent;
        private bool _boostDepleted;

        [FormerlySerializedAs("_maxSpeed")] [Header("Movement Settings")]
        [SerializeField] private float _thrustMaxSpeed = 120f;
        [SerializeField] private float _yawMaxSpeed = 90f;
        [SerializeField] private PlayerMovement _playerMovement;

        [Header("Camera Settings")]
        [SerializeField] private float _cameraDistance = 23.71f;
        [SerializeField] private float _fovDistance = 80f;
        [SerializeField] private float _cameraLerpSpeed = 5f;
        [SerializeField] private CinemachineThirdPersonFollow _thirdPersonFollow;
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        [Header("UI")]
        [SerializeField] private Slider _slider;

        private float _initThurstSpeed;
        private float _initYawSpeed;
        private bool _isSprinting;

        private InputAction _sprintAction;

        private void Awake()
        {
            _initThurstSpeed = _playerMovement.GetThrustSpeed();
            _initYawSpeed = _playerMovement.GetYawSpeed();
            _currentBoostPercent = _totalBoostPercent;

            var playerInput = GetComponent<PlayerInput>();
            _sprintAction = playerInput.actions["Sprint"];

            UpdateUISlider();
        }

        private void Update()
        {
            UpdateUISlider();
            HandleBoostInput();
            UpdateMovement();
            UpdateCamera();
        }

        private void HandleBoostInput()
        {
            bool sprintingInput = _sprintAction.IsPressed();

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
            float thrustSpeed = _isSprinting ? _thrustMaxSpeed : _initThurstSpeed;
            float yawSpeed = _isSprinting ? _yawMaxSpeed : _initYawSpeed;
            _playerMovement.SetMoveSpeed(thrustSpeed);
            _playerMovement.SetYawSpeed(yawSpeed);
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

        private void UpdateUISlider()
        {
            _slider.value = _currentBoostPercent / 100f;
        }
    }
}
