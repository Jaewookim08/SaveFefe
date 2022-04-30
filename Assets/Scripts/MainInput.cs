using UnityEngine;


public class MainInput : MonoBehaviour
{
    [SerializeField] private MainCharacter _mainCharacter;


    private static readonly KeyCode[] JumpKeys = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F };
    private Camera _targetCamera;


    private void Start()
    {
        _targetCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mainCharacter.JumpToward(GetMouseWorldPosition());
        }
        foreach (var key in JumpKeys)
        {
            if (Input.GetKeyDown(key))
            {
                _mainCharacter.JumpToward(GetMouseWorldPosition());
            }
        }
        if (Input.GetKey(KeyCode.Q))
        {
            _mainCharacter.JumpToward(GetMouseWorldPosition());
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        return (Vector2)_targetCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}