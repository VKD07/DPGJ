using System;
using System.Collections;
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
        [SerializeField] private Image _boostInnerImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeSpeed = 0.2f;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _overHeatColor;

        [Header("VFX")] 
        [SerializeField] private VfxHandler _boosterVfx;

        [Header("SFX")] [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _windAudioSource;
        [SerializeField] private AudioClip _boostSound;
        private bool _hasSoundPlayed;
        private float _initThurstSpeed;
        private float _initYawSpeed;
        private bool _isSprinting;
        private bool _hasShownUI;
        private Coroutine _fadeCoroutine;

        private InputAction _sprintAction;

        private void Awake()
        {
            _initThurstSpeed = _playerMovement.GetThrustSpeed();
            _initYawSpeed = _playerMovement.GetYawSpeed();
            _currentBoostPercent = _totalBoostPercent;
            _canvasGroup.alpha = 0;

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
            UpdateBoosterUICanvasGroup();
        }

        private void HandleBoostInput()
        {
            bool sprintingInput = _sprintAction.IsPressed();

            if (_boostDepleted)
            {
                _isSprinting = false;
                RegenerateBoost();
                _boosterVfx.SetActiveBooster(false);
                _windAudioSource.Stop();
                _hasSoundPlayed = false;

                if (_currentBoostPercent >= _totalBoostPercent)
                {
                    _currentBoostPercent = _totalBoostPercent;
                    _boostInnerImage.color = _normalColor;
                    _boostDepleted = false;
                }

                return;
            }

            if (sprintingInput && _currentBoostPercent > 0f)
            {
                if (!_hasSoundPlayed)
                {
                    _windAudioSource.Play();
                    _hasSoundPlayed = true;
                    _audioSource.PlayOneShot(_boostSound, 0.7f);
                }
                
                _isSprinting = true;
                _currentBoostPercent -= Time.deltaTime * _depletionStrength;
                _boosterVfx.SetActiveBooster(true);
                _hasShownUI = true;
                if (_currentBoostPercent <= 0f)
                {
                    _boostInnerImage.color = _overHeatColor;
                    _currentBoostPercent = 0f;
                    _boostDepleted = true;
                    _isSprinting = false;
                }
            }
            else
            {
                _windAudioSource.Stop();
                _hasSoundPlayed = false;
                _isSprinting = false;
                _boosterVfx.SetActiveBooster(false);
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

        private void UpdateBoosterUICanvasGroup()
        {
            if (_hasShownUI)
            {
                _hasShownUI = false;
                _canvasGroup.alpha = 1f;

                if (_fadeCoroutine != null)
                    StopCoroutine(_fadeCoroutine);

                _fadeCoroutine = StartCoroutine(FadeOutCanvasGroup());
            }
        }

        private IEnumerator FadeOutCanvasGroup()
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime * _fadeSpeed;
                yield return null;
            }

            _canvasGroup.alpha = 0f;
        }
        private void UpdateUISlider()
        {
            _boostInnerImage.fillAmount = _currentBoostPercent / 100;
        }

        private void OnDisable()
        {
            _windAudioSource.Stop();
            _audioSource.Stop();
        }
    }
}
