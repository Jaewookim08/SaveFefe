using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class MainCharacter : MonoBehaviour
{
    public enum JumpDirection
    {
        Left,
        Right
    }

    [SerializeField] private float _jumpImpulseXSize = 1f;
    [SerializeField] private float _jumpImpulseYSize = 1f;
    [Header("Gravity")] [SerializeField] private float _minGravity = 2;
    [SerializeField] private float _maxGravity = 5;
    [SerializeField] private float _timeUntilGravityIncreaseStart = 0.5f;
    [SerializeField] private float _timeForGravityChange = 0.2f;

    [Header("Movement")] [SerializeField] private float _verticalFriction = 32f; // 위로 가는 중에 아래쪽으로 작용하는 가속도
    [SerializeField] private float _horizontalFriction = 32f; // 좌우 방향 가속도
    [SerializeField] private float _maxUpwardSpeed = 2f;
    [SerializeField] private float _maxDownwardSpeed = 60f;
    [SerializeField] private float _maxHorizontalSpeed = 2f;

    [Header("Sprites")] [SerializeField] private Sprite[] _defaultSprites;
    [SerializeField] private Sprite _stunnedSprite;

    private int _defaultSpriteIndex = 0;
    private float _gravitySize;
    private float _lastGravityResetTime;
    private Queue<Vector2> _nextJumpImpulseBuffer;

    private float _stunnedUntil = 0f;


    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ResetGravity();
        _nextJumpImpulseBuffer = new Queue<Vector2>();
    }

    private void Update()
    {
        _spriteRenderer.sprite = GetSprite();
    }
    

    public void Jump(JumpDirection dir)
    {
        if (!CanJump()) return;
        ResetGravity();

        _defaultSpriteIndex = (_defaultSpriteIndex + 1) % _defaultSprites.Length;

        var xDir = dir == JumpDirection.Left ? -1 : 1;

        var jumpVec = new Vector2(xDir * _jumpImpulseXSize, _jumpImpulseYSize);

        _nextJumpImpulseBuffer.Enqueue(jumpVec);
    }

    public void Stun(float seconds)
    {
        _stunnedUntil = Time.time + seconds;
    }

    public void CollidedWithTrigger(Vector2 vec, float power, float stunTime)
    {
        vec.Normalize();
        //_rigidbody.AddForce(vec * power, ForceMode2D.Impulse);
        _rigidbody.velocity = vec * power;
        if (stunTime != 0)
            Stun(stunTime);
        return;
    }

    private void ResetGravity()
    {
        _gravitySize = _minGravity;
        _lastGravityResetTime = Time.fixedTime;
    }

    private void FixedUpdate()
    {
        UpdateGravitySize();
        _rigidbody.AddForce(Vector2.down * _gravitySize, ForceMode2D.Force);

        ApplyFriction();

        ApplyBufferedJumps();

        // Clamp velocity
        _rigidbody.velocity = ClampVelocity(_rigidbody.velocity);
    }

    private void UpdateGravitySize()
    {
        if (Time.fixedTime <= _lastGravityResetTime + _timeUntilGravityIncreaseStart) return;

        _gravitySize += (_maxGravity - _minGravity) / _timeForGravityChange * Time.fixedDeltaTime;
        _gravitySize = Math.Min(_gravitySize, _maxGravity);
    }

    private void ApplyFriction()
    {
        if (IsStunned())
            return;
        var vel = _rigidbody.velocity;

        if (vel.y > 0)
        {
            vel.y = Math.Max(vel.y - vel.y * _verticalFriction * Time.fixedDeltaTime, 0);
        }

        vel.x = Mathf.MoveTowards(vel.x, 0, Math.Abs(vel.x) * _horizontalFriction * Time.fixedDeltaTime);

        _rigidbody.velocity = vel;
    }

    private void ApplyBufferedJumps()
    {
        while (_nextJumpImpulseBuffer.Count > 0)
        {
            var impulse = _nextJumpImpulseBuffer.Dequeue();
            _rigidbody.AddForce(impulse, ForceMode2D.Impulse);
        }
    }


    private Vector2 ClampVelocity(Vector2 vec)
    {
        vec.x = Math.Min(_maxHorizontalSpeed, Math.Max(-_maxHorizontalSpeed, vec.x));
        vec.y = Math.Min(_maxUpwardSpeed, Math.Max(-_maxDownwardSpeed, vec.y));
        return vec;
    }

    private bool CanJump()
    {
        return !IsStunned();
    }

    private bool IsStunned()
    {
        return _stunnedUntil > Time.time;
    }

    private Sprite GetSprite()
    {
        if (IsStunned())
        {
            return _stunnedSprite;
        }

        return _defaultSprites[_defaultSpriteIndex];
    }
}