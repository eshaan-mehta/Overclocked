using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionsInitializer : MonoBehaviour
{
    void Start()
    {
        // Disable all action maps by default
        InputSystem.actions.Disable();

        // Enable only the Player action map
        if (InputSystem.actions.FindActionMap("Player") != null)
        {
            InputSystem.actions.FindActionMap("Player").Enable();
        }
    }
}
