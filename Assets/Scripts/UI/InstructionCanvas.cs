using UnityEngine;

public class InstructionCanvas : MonoBehaviour
{

    [SerializeField] GameObject instructionCanvas;
    
    void Start()
    {
        InputManager.Instance.InstructionEvent += ToggleInstruction;
    }

    private void OnDestroy()
    {
        InputManager.Instance.InstructionEvent -= ToggleInstruction;

    }


    void ToggleInstruction()
    {
        instructionCanvas.SetActive(!instructionCanvas.activeSelf);
    }

}
