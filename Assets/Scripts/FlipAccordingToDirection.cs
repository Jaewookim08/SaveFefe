using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlipAccordingToDirection : MonoBehaviour
{
    private Vector3 _lastPos;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        var pos = transform.position;

        if ((pos - _lastPos).sqrMagnitude < 0.001) return;

        _spriteRenderer.flipX = pos.x > _lastPos.x;

        _lastPos = transform.position;
    }
}