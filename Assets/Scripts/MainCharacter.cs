using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour
{
    [SerializeField] private float _jumpImpulseSize = 1f;
    [Header("Gravity")] [SerializeField] private float _minGravity = 2;
    [SerializeField] private float _maxGravity = 5;
    [SerializeField] private float _timeUntilGravityIncreaseStart = 0.5f;
    [SerializeField] private float _timeForGravityChange = 0.2f;

    [Header("Movement")] [SerializeField] private float _verticalReverseAcceleration = 32f; // 위로 가는 중에 아래쪽으로 작용하는 가속도
    [SerializeField] private float _horizontalReverseAcceleration = 32f; // 좌우 방향 가속도
    [SerializeField] private float _maxUpwardSpeed = 2f;
    [SerializeField] private float _maxDownwardSpeed = 60f;
    [SerializeField] private float _maxHorizontalSpeed = 2f;


    private float _gravitySize;
    private float _lastGravityResetTime;
    private Vector2? _nextJumpImpulse;


    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        ResetGravity();
    }


    public void JumpToward(Vector2 targetPosition)
    {
        // 중력 초기화
        ResetGravity();

        var dir = targetPosition - (Vector2)transform.position;

        _nextJumpImpulse = dir.normalized * _jumpImpulseSize;
    }

    private void ResetGravity()
    {
        _gravitySize = _minGravity;
        _lastGravityResetTime = Time.fixedTime;
    }

    private void FixedUpdate()
    {
        // Apply gravity
        UpdateGravitySize();
        _rigidbody.AddForce(Vector2.down * _gravitySize, ForceMode2D.Force);

        // Apply accelerations
        ApplyReverseAccelerations();

        // Jump if any jump request exists.
        ApplyBufferedJump();

        // Clamp velocity
        _rigidbody.velocity = ClampVelocity(_rigidbody.velocity);
    }

    private void UpdateGravitySize()
    {
        if (!(Time.fixedTime > _lastGravityResetTime + _timeUntilGravityIncreaseStart)) return;

        _gravitySize += (_maxGravity - _minGravity) / _timeForGravityChange * Time.fixedDeltaTime;
        _gravitySize = Math.Min(_gravitySize, _maxGravity);
    }

    private void ApplyReverseAccelerations()
    {
        var vel = _rigidbody.velocity;

        if (vel.y > 0)
        {
            vel.y = Math.Max(vel.y - _verticalReverseAcceleration*Time.fixedDeltaTime, 0);
        }

        vel.x = Mathf.MoveTowards(vel.x, 0, _horizontalReverseAcceleration*Time.fixedDeltaTime);

        _rigidbody.velocity = vel;
    }

    private void ApplyBufferedJump()
    {
        if (!_nextJumpImpulse.HasValue) return;
        _rigidbody.AddForce(_nextJumpImpulse.Value, ForceMode2D.Impulse);
        _nextJumpImpulse = null;
    }


    private Vector2 ClampVelocity(Vector2 vec)
    {
        vec.x = Math.Min(_maxHorizontalSpeed, Math.Max(-_maxHorizontalSpeed, vec.x));
        vec.y = Math.Min(_maxUpwardSpeed, Math.Max(-_maxDownwardSpeed, vec.y));
        return vec;
    }
}