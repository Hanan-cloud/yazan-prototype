using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;

public class InputDeviceDetector : MonoBehaviour
{


    [SerializeField] UnityEvent OnGamePadInput;
    [SerializeField] UnityEvent OnKeyBoardInput;


    public enum InputType
    {
        None,
        Gamepad,
    }

    public InputType CurrentInputType { get; private set; } = InputType.None;
    public static event Action<InputType> OnInputTypeChanged;

    private InputType lastInputType = InputType.None;

    void Update()
    {
        InputType detectedInput = DetectCurrentInput();

        if (detectedInput != lastInputType)
        {
            lastInputType = detectedInput;
            CurrentInputType = detectedInput;
            OnInputTypeChanged?.Invoke(CurrentInputType);
        }
    }

    private InputType DetectCurrentInput()
    {
        //if (Touchscreen.current != null && Touchscreen.current.wasUpdatedThisFrame)
        //{
        //    OnTouchInput?.Invoke();
        //    return InputType.Touchscreen;
        //}
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            OnGamePadInput?.Invoke();

            return InputType.Gamepad;
        }
        else if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame))
        {

            OnKeyBoardInput?.Invoke();

            return InputType.Gamepad;
        }

        return CurrentInputType;
    }
}
