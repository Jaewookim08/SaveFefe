using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowingFish : MonoBehaviour
{
    [SerializeField] private Transform _following;
    [SerializeField] private Vector2 _aroundBox; // 타겟으로부터 이 범위만큼의 거리만 목표지점으로 찍힘 
    [SerializeField] private float _timeToChangeDest = 3f;
    [SerializeField] private float _maxMoveDist = 10f;
    
    private float _setNextDestTime = 0f;
    private Vector3 _targetPosition;
    
    private void Awake()
    {
    }
    
    private void FixedUpdate()
    {
        if (Time.fixedTime > _setNextDestTime)
        {
            _targetPosition = GetRandomDestAroundTarget();
            _setNextDestTime = Time.fixedTime + _timeToChangeDest;
        }

        var pos = (Vector3)Vector2.Lerp(transform.position, _targetPosition, 0.01f);
        pos.z = transform.position.z;
        transform.position = pos;
    }

    private Vector2 GetRandomDestAroundTarget()
    {
        for (var i = 0; i < 30; i++)
        {
            var dx = Random.Range(-_aroundBox.x, _aroundBox.x);
            var dy = Random.Range(-_aroundBox.y, _aroundBox.y);

            var p = (Vector2)_following.position + new Vector2(dx, dy);
            if ((p - (Vector2)transform.position).magnitude <= _maxMoveDist)
            {
                return p;
            }
        }

        return _targetPosition; // Todo: 못 찾은 경우 처리.
    }
    
}