using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class TimerSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Button button1s;
    [SerializeField] private Button button3s;
    [SerializeField] private Button button5s;
    [SerializeField] private Button button10s;

    [Header("Player References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UnityEngine.InputSystem.PlayerInput playerInput;

    private Action<float> onTimerSelected;
    private bool isFrozen = false;

    void Awake()
    {
        // Find player references if not assigned
        if (playerController == null)
            playerController = FindFirstObjectByType<PlayerController>();

        if (playerInput == null)
            playerInput = FindFirstObjectByType<UnityEngine.InputSystem.PlayerInput>();

        // Wire up button callbacks
        button1s?.onClick.AddListener(() => OnButtonClicked(1f));
        button3s?.onClick.AddListener(() => OnButtonClicked(3f));
        button5s?.onClick.AddListener(() => OnButtonClicked(5f));
        button10s?.onClick.AddListener(() => OnButtonClicked(10f));

        // Start hidden
        HidePopup();
    }

    void Update()
    {
        // Listen for ESC key to cancel
        if (isFrozen && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OnCancelClicked();
        }
    }

    public void ShowPopup(Action<float> callback)
    {
        onTimerSelected = callback;
        popupPanel?.SetActive(true);
        FreezePlayer();
    }

    private void OnButtonClicked(float duration)
    {
        HidePopup();
        UnfreezePlayer();
        onTimerSelected?.Invoke(duration);
        onTimerSelected = null;
    }

    private void OnCancelClicked()
    {
        HidePopup();
        UnfreezePlayer();
        onTimerSelected = null;
    }

    private void HidePopup()
    {
        popupPanel?.SetActive(false);
    }

    private void FreezePlayer()
    {
        if (isFrozen) return;

        if (playerController != null)
            playerController.enabled = false;

        if (playerInput != null)
            playerInput.DeactivateInput();

        isFrozen = true;
    }

    private void UnfreezePlayer()
    {
        if (!isFrozen) return;

        if (playerController != null)
            playerController.enabled = true;

        if (playerInput != null)
            playerInput.ActivateInput();

        isFrozen = false;
    }

    void OnDestroy()
    {
        // Ensure player is unfrozen if UI is destroyed
        UnfreezePlayer();
    }
}
