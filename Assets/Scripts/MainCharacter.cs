using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour
{
    public enum JumpDirection
    {
        Left,
        Right
    }


    [SerializeField] private float _inertia = 1f;
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

    [Header("Sprites")] [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _defaultSprites;
    [SerializeField] private Sprite _stunnedSprite;
    [SerializeField] private MainCharacterEye _eyeController;

    [FormerlySerializedAs("_attackDropStartSpeed")] [Header("Others")] [SerializeField]
    private float _dropAttackStartSpeed = 40f;

    [FormerlySerializedAs("_attackDropParticleSystem")] [SerializeField]
    private ParticleSystem _dropAttackParticleSystem;


    private int _defaultSpriteIndex = 0;
    private float _gravitySize;
    private float _lastGravityResetTime;
    private Queue<Vector2> _nextJumpImpulseBuffer;

    private float _stunnedUntil = 0f;


    private Rigidbody2D _rigidbody;
    private SeeObject _seeObject;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _seeObject = GetComponent<SeeObject>();
        ResetGravity();
        _nextJumpImpulseBuffer = new Queue<Vector2>();
    }

    private void Start()
    {
        _rigidbody.inertia = _inertia;
    }


    private bool _lastIsDropAttacking = false;
    
    private void Update()
    {
        _spriteRenderer.sprite = GetSprite();
        Vector3? closestVec = _seeObject.GetClosestSeeObjTransform();
        _eyeController.UpdateEye(IsStunned(), (Vector2?)closestVec);


        // Todo: 

        if (_lastIsDropAttacking != IsDropAttacking)
        {
            foreach (var aa in FindObjectsOfType<Wtf>())
            {
                aa.Collider.isTrigger = IsDropAttacking;
                aa.Trigger.isTrigger = IsDropAttacking;
            }
        }
        

        if (!IsDropAttacking)
        {
            _dropAttackParticleSystem.Stop();
        }
        else
        {
            // gameObject.layer = 6;y
            if (!_dropAttackParticleSystem.isPlaying)
            {
                _dropAttackParticleSystem.transform.rotation = Quaternion.identity; // 땜빵코드.    Todo: 
                _dropAttackParticleSystem.Clear();
                _dropAttackParticleSystem.Play();
            }
        }
        
        _lastIsDropAttacking = IsDropAttacking;

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
        _stunnedUntil = Math.Max(_stunnedUntil, Time.time + seconds);
    }

    public void CollidedWithTrigger(Vector2 vec, float power, float stunTime, GameObject other)
    {
        CollidedWithTriggerAtPoint(_rigidbody.position, vec, power, stunTime, other);
    }

    public void CollidedWithTriggerAtPoint(Vector2 pos, Vector2 vec, float power, float stunTime, GameObject other)
    {
        if (IsDropAttacking)
        {
            Destroy(other);
        }
        else
        {
            vec.Normalize();
            _rigidbody.velocity = Vector2.zero;
            Debug.Log(vec * power);
            // _rigidbody.AddForceAtPosition(vec * power, pos, ForceMode2D.Impulse);
            _rigidbody.AddForce(vec * power, ForceMode2D.Impulse);
            Stun(stunTime);
        }
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
        if (!IsStunned())
        {
            _rigidbody.velocity = ClampVelocity(_rigidbody.velocity);
        }
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

    public bool IsDropAttacking => !IsStunned() && _rigidbody.velocity.y < -_dropAttackStartSpeed;
}