using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, GameInput.IPlayerActions
{
    public static InputManager Instance;


    private GameInput _gameInput;


    private void Awake()
    {
        Instance = this;
        if (_gameInput == null)
            _gameInput = new GameInput();

        _gameInput.Player.SetCallbacks(this);
        _gameInput.Player.Enable();

    }


    Vector2 dir = Vector2.zero;

    bool isRunning = false;

    public bool IsRunning { get => isRunning; }
    public Vector2 Dir { get => dir;  }

    public void OnMovement(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();


    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }



    private void OnDisable()
    {
        _gameInput.Player.Disable();

    }
}
