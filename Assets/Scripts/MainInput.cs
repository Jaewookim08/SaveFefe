using UnityEngine;


public class MainInput : MonoBehaviour
{
    [SerializeField] private MainCharacter _mainCharacter;


    private static readonly KeyCode[] LeftJumpKeys = { KeyCode.D };
    private static readonly KeyCode[] RightJumpKeys = { KeyCode.K };
    private Camera _targetCamera;


    private void Start()
    {
        _targetCamera = Camera.main;
    }

    private void Update()
    {
        foreach (var key in LeftJumpKeys)
        {
            if (Input.GetKeyDown(key))
            {
                _mainCharacter.Jump(MainCharacter.JumpDirection.Left);
            }
        }

        foreach (var key in RightJumpKeys)
        {
            if (Input.GetKeyDown(key))
            {
                _mainCharacter.Jump(MainCharacter.JumpDirection.Right);
            }
        }


        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _mainCharacter.Jump(
                    touch.position.x < (float)Screen.width / 2
                        ? MainCharacter.JumpDirection.Left
                        : MainCharacter.JumpDirection.Right);
            }
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        return (Vector2)_targetCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}