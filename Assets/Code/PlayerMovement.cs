using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Code
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Input")] [SerializeField] private InputActionReference _move;
        [SerializeField] private InputActionReference _look;

        [FormerlySerializedAs("_moveSpeed")] [Header("Movement Settings")] [SerializeField]
        private float _thrustSpeed = 50f;

        [SerializeField] private float _strafeSpeed = 40f;
        [SerializeField] private float _yawSpeed = 60f;
        [SerializeField] private float _pitchSpeed = 60f;
        [SerializeField] private float _maxPitchAngle = 90f;

        [Header("Camera Settings")] [SerializeField]
        private CinemachineThirdPersonFollow _thirdPersonFollow;

        [SerializeField] private Vector3 _cameraOffsetUp = new Vector3(1.32f, 4.91f, 1.24f);
        [SerializeField] private Vector3 _cameraOffsetNeutral = new Vector3(1.32f, 1.01f, -3.90f);
        [SerializeField] private Vector3 _cameraOffsetDown = new Vector3(1.32f, -3.32f, -2.14f);
        [SerializeField] private float _cameraFollowSmoothing = 5f;

        [Header("Visual Settings")] [SerializeField]
        private Transform visuals;

        [SerializeField] private float _maxRollAngle = 25f;
        [SerializeField] private float _rollSmoothing = 5f;
        [SerializeField] private float _thrustTiltAngle = 10f;
        [SerializeField] private float _thrustTiltSmoothing = 5f;

        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private float _currentPitch;
        private float _currentYaw;
        private float _currentThrustTilt;
        private float _currentRollVisual;
        private Vector3 _currentShoulderOffset;
        private bool _isAssisted;

        private void Update()
        {
            _moveInput = _move.action.ReadValue<Vector2>();
            _lookInput = _look.action.ReadValue<Vector2>();
            Move();
            Rotate();
            UpdateVisuals();
        }

        private void Move()
        {
            if (_isAssisted)
            {
                return;
            }

            Vector3 movement =
                (transform.forward * _moveInput.y * _thrustSpeed + transform.right * _moveInput.x * _strafeSpeed) *
                Time.deltaTime;
            transform.position += movement;
        }

        public void SetAssistedMovement(bool val)
        {
            _isAssisted = val;
            if (val)
            {
                Vector3 movement =
                    (transform.forward * 1 * _thrustSpeed + transform.right * _moveInput.x * _strafeSpeed) *
                    Time.deltaTime;
                transform.position += movement;
            }
        }

        private void Rotate()
        {
            _currentPitch = Mathf.Clamp(_currentPitch - _lookInput.y * _pitchSpeed * Time.deltaTime, -_maxPitchAngle,
                _maxPitchAngle);
            _currentYaw += _lookInput.x * _yawSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(_currentPitch, _currentYaw, 0f);

            float normalizedPitch = _currentPitch / _maxPitchAngle;
            Vector3 targetOffset = _currentPitch > 0
                ? Vector3.Lerp(_cameraOffsetNeutral, _cameraOffsetUp, normalizedPitch)
                : Vector3.Lerp(_cameraOffsetNeutral, _cameraOffsetDown, -normalizedPitch);
            _currentShoulderOffset =
                Vector3.Lerp(_currentShoulderOffset, targetOffset, Time.deltaTime * _cameraFollowSmoothing);
            _thirdPersonFollow.ShoulderOffset = _currentShoulderOffset;
        }

        private void UpdateVisuals()
        {
            if (!visuals) return;

            float targetRoll = -_lookInput.x * _maxRollAngle;
            _currentRollVisual = Mathf.Lerp(_currentRollVisual, targetRoll, Time.deltaTime * _rollSmoothing);

            float targetTilt = _moveInput.y > 0f ? -_thrustTiltAngle : _moveInput.y < 0f ? _thrustTiltAngle : 0f;
            _currentThrustTilt = Mathf.Lerp(_currentThrustTilt, targetTilt, Time.deltaTime * _thrustTiltSmoothing);

            visuals.localRotation = Quaternion.Euler(-_currentThrustTilt, 0f, _currentRollVisual);
        }

        public float GetThrustSpeed()
        {
            return _thrustSpeed;
        }

        public float GetYawSpeed()
        {
            return _yawSpeed;
        }

        public void SetMoveSpeed(float newSpeed)
        {
            _thrustSpeed = newSpeed;
        }

        public void SetYawSpeed(float newSpeed)
        {
            _yawSpeed = newSpeed;
        }
    }
}