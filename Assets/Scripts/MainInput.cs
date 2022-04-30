using UnityEngine;


public class MainInput : MonoBehaviour
{
    [SerializeField] private MainCharacter _mainCharacter;


    private Camera _targetCamera;

    private void Start()
    {
        _targetCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown )
        {
            _mainCharacter.JumpToward(GetMouseWorldPosition());
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        return (Vector2)_targetCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}