using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour
{
    [SerializeField] private float _jumpImpulseSize = 1f;
    [Header("Gravity")]
    [SerializeField] private float _minGravity = 2;
    [SerializeField] private float _maxGravity = 5;
    [SerializeField] private float _timeUntilGravityIncreaseStart = 0.5f;
    [SerializeField] private float _timeForGravityChange = 0.2f;

    private float _gravitySize;
    private float _lastGravityResetTime;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _gravitySize = _maxGravity;
    }

    public void JumpToward(Vector2 targetPosition)
    {
        // 중력 초기화
        ResetGravity();

        
        var dir = targetPosition - (Vector2)transform.position;

        dir = dir.normalized * _jumpImpulseSize;

        _rigidbody.AddForce(dir, ForceMode2D.Impulse);
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
    }

    private void UpdateGravitySize()
    {
        if (!(Time.fixedTime > _lastGravityResetTime + _timeUntilGravityIncreaseStart)) return;
        
        _gravitySize += (_maxGravity - _minGravity) / _timeForGravityChange * Time.fixedDeltaTime;
        _gravitySize = Math.Min(_gravitySize, _maxGravity);
    }
    
}