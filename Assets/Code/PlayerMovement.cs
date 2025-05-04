using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _look;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _strafeSpeed = 40f;
    [SerializeField] private float _yawSpeed = 60f;
    [SerializeField] private float _pitchSpeed = 60f;
    [SerializeField] private float _maxPitchAngle = 90f;

    [Header("Visual Settings")]
    [SerializeField] private Transform visuals;
    [SerializeField] private float _maxRollAngle = 25f;
    [SerializeField] private float _rollSmoothing = 5f;
    [SerializeField] private float _thrustTiltAngle = 10f;
    [SerializeField] private float _thrustTiltSmoothing = 5f;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private float _currentPitch = 0f;
    private float _currentYaw = 0f;
    private float _currentThrustTilt = 0f;
    private float _currentRollVisual = 0f;

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
        Vector3 forwardMovement = transform.forward * _moveInput.y * _moveSpeed;
        Vector3 strafeMovement = transform.right * _moveInput.x * _strafeSpeed;

        Vector3 combined = (forwardMovement + strafeMovement) * Time.deltaTime;
        transform.position += combined;
    }

    private void Rotate()
    {
        float pitchDelta = -_lookInput.y * _pitchSpeed * Time.deltaTime;
        float yawDelta = _lookInput.x * _yawSpeed * Time.deltaTime;

        _currentPitch = Mathf.Clamp(_currentPitch + pitchDelta, -_maxPitchAngle, _maxPitchAngle);
        _currentYaw += yawDelta;

        Quaternion newRotation = Quaternion.Euler(_currentPitch, _currentYaw, 0f);
        transform.rotation = newRotation;
    }

    private void UpdateVisuals()
    {
        if (visuals == null) return;

        float targetRoll = -_lookInput.x * _maxRollAngle;
        _currentRollVisual = Mathf.Lerp(_currentRollVisual, targetRoll, Time.deltaTime * _rollSmoothing);

        float targetTilt = _moveInput.y > 0f ? -_thrustTiltAngle :
                           _moveInput.y < 0f ? _thrustTiltAngle : 0f;
        _currentThrustTilt = Mathf.Lerp(_currentThrustTilt, targetTilt, Time.deltaTime * _thrustTiltSmoothing);

        visuals.localRotation = Quaternion.Euler(-_currentThrustTilt, 0f, _currentRollVisual);
    }
}
