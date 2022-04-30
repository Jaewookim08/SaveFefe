using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour
{
    [SerializeField] private float _jumpImpulseSize = 1f;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void JumpToward(Vector2 targetPosition)
    {
        var dir = targetPosition - (Vector2)transform.position;
        
        dir = dir.normalized * _jumpImpulseSize;
        
        _rigidbody.AddForce(dir, ForceMode2D.Impulse);
    }
}