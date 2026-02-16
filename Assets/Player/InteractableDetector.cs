using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
   [Header("Interaction Settings")] 
   [Tooltip("Player interaction reach radius")]
   public float interactionRadius = 0.5f;

   private Interactable currentHighlighted;

    void Update()
    {
        InteractableDetector closest = FindClosestFacingInteractable();
    }
}
