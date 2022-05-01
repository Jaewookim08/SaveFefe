using UnityEngine;

public class Wtf : MonoBehaviour
{
    public Collider2D Collider => _collider;
    public TriggerObject Trigger => _trigger;
    
    private Collider2D _collider;
    private TriggerObject _trigger;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _trigger = GetComponent<TriggerObject>();
    }
}