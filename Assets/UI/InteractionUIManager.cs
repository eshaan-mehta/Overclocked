using UnityEngine;
using TMPro;

public class InteractionUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Prompt Text")]
    [SerializeField] private string pickUpText = "E - Pick Up";
    [SerializeField] private string placeText = "E - Place";

    void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogWarning("InteractionUIManager: CanvasGroup not found, adding one");
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        if (promptText == null)
        {
            Debug.LogError("InteractionUIManager: promptText not assigned");
        }

        // Start hidden
        HidePrompt();
    }

    public void ShowPrompt(string text)
    {
        if (promptText != null)
        {
            promptText.text = text;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
        }
    }

    public void HidePrompt()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }
    }

    public void UpdatePrompt(bool isHoldingDisk, bool canInteract)
    {
        if (!canInteract)
        {
            HidePrompt();
            return;
        }

        string text = isHoldingDisk ? placeText : pickUpText;
        ShowPrompt(text);
    }
}
