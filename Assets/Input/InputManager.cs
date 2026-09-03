using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class InputManager : MonoBehaviour, GameInput.IPlayerActions, GameInput.IStoryPanelsActions
{
    public static InputManager Instance;


    private GameInput _gameInput;

    [SerializeField] Controllers currentController = Controllers.player;


    Vector2 dir = Vector2.zero;

    bool isRunning = false;

    public bool IsRunning { get => isRunning; }
    public Vector2 Dir { get => dir; }



    public event Action NextEvent;
    public event Action SkipEvent;
    public event Action DollEvent;
    public event Action PauseEvent;
    public event Action SkipEventCancel;
    public event Action InteractionEvent;



    private void Awake()
    {
        Instance = this;
        if (_gameInput == null)
            _gameInput = new GameInput();

        SwitchControllerMap(currentController);



    }


    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            InteractionEvent?.Invoke();


    }

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


    //================================================================
    public void OnNext(InputAction.CallbackContext context)
    {
        NextEvent?.Invoke();
    }

    public void OnSkip(InputAction.CallbackContext context)
    {

        switch (context.phase)
        { case InputActionPhase.Performed:
                SkipEvent?.Invoke();
                break;

            case InputActionPhase.Canceled:
                SkipEventCancel?.Invoke();
                break;

        }
    }

    public void SwitchControllerMap(Controllers c)
    {
        _gameInput.StoryPanels.Disable();
        _gameInput.Player.Disable();
        _gameInput.UI.Disable();


        switch (c)
        {

            case Controllers.story:
                _gameInput.StoryPanels.SetCallbacks(this);
                _gameInput.StoryPanels.Enable();
                break;
            
            case Controllers.UI:
                //_gameInput.UI.SetCallbacks(this);
                _gameInput.UI.Enable();
                break;

            //case Controllers.UI:

            //    _gameInput.UI.Enable();
            //    break;

            default:
                _gameInput.Player.SetCallbacks(this);
                _gameInput.Player.Enable();
                break;

        }



    }

    public void OnPause(InputAction.CallbackContext context)
    {
        PauseEvent?.Invoke();
    }

    public void OnDoll(InputAction.CallbackContext context)
    {
        DollEvent?.Invoke();
    }

    public enum Controllers { player, story, UI }

}
