using System;
using UnityEngine;

public class MainCharacterEye : MonoBehaviour
{
    [SerializeField] private Transform _watchFromPoint;
    [SerializeField] private float _eyeRadius;
    [SerializeField] private float _eyeMouseRatio;
    [SerializeField] private SpriteRenderer[] _pupils;
    [SerializeField] private Sprite _defaultEye;
    [SerializeField] private Sprite _stunnedEye;


    private Camera _targetCamera;


    private void Start()
    {
        _targetCamera = Camera.main;
    }

    private void Update()
    {
    }


    public void UpdateEye(bool isStunned, Vector2? lookingAt)
    {
        if (isStunned)
        {
            foreach (var p in _pupils)
            {
                p.sprite = _stunnedEye;
            }

            LookAt(Vector2.zero);
        }
        else
        {
            foreach (var p in _pupils)
            {
                p.sprite = _defaultEye;
            }

            if (!lookingAt.HasValue){
                LookAt(Vector2.zero);
                return;
            } 

            LookAt(lookingAt.Value - (Vector2) _watchFromPoint.position);
        }
    }


    private void LookAt(Vector2 vec)
    {
        vec /= _eyeMouseRatio;
        var pupilPos = vec.normalized * Math.Min(vec.magnitude, _eyeRadius);

        foreach (var pupil in _pupils)
        {
            var trans = pupil.transform;
            trans.position = trans.parent.transform.position +
                             new Vector3(pupilPos.x, pupilPos.y, trans.localPosition.z);
        }
    }

}