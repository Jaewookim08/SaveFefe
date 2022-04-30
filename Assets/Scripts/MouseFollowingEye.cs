using System;
using Unity.VisualScripting;
using UnityEngine;

public class MouseFollowingEye : MonoBehaviour
{
    [SerializeField] private Transform _watchFromPoint;
    [SerializeField] private float _eyeRadius;
    [SerializeField] private float _eyeMouseRatio;
    [SerializeField] private Transform[] _pupils;

    private Camera _targetCamera;


    private void Start()
    {
        _targetCamera = Camera.main;
    }

    private void Update()
    {
        LookAt(GetMouseWorldPosition() - (Vector2)_watchFromPoint.position);
    }

    private void LookAt(Vector2 at)
    {
        at /= _eyeMouseRatio;
        var pupilPos = at.normalized * Math.Min(at.magnitude, _eyeRadius);

        foreach (var pupil in _pupils)
        {
            pupil.localPosition = new Vector3(pupilPos.x, pupilPos.y, pupil.localPosition.z);
        }
        
    }

    private Vector2 GetMouseWorldPosition()
    {
        return (Vector2)_targetCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}