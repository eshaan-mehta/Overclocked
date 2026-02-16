using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    [Header("Interaction Settings")] 
    [Tooltip("Player interaction reach radius")]
    public float interactionRadius = 0.5f;

    [Tooltip("How wide the detection cone is (0 = directly ahead, 1 = full 180 degrees)")]
    [Range(0f, 1f)]
    public float detectionAngle = 0.3f; 
    private Interactable currentHighlighted;

    void Update()
    {
        Interactable closest = FindClosestFacingInteractable();

        if (closest != currentHighlighted)
        {
            // Unhighlight the current highlighted interactable
            if (currentHighlighted != null)
            {
                currentHighlighted.SetHighlighted(false);
            }

            // Highlight the new closest interactable
            currentHighlighted = closest;
            if (currentHighlighted != null)
            {
                currentHighlighted.SetHighlighted(true);
            }
        }
    }

    private Interactable FindClosestFacingInteractable()
    {
        // Find all interactables in the interaction radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);

        Interactable closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Collider col in colliders)
        {
            Interactable interactable = col.GetComponent<Interactable>();
            if (interactable == null) continue;
            
            // Compute angle between player and collider
            Vector3 directionToTarget = (col.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, directionToTarget);
            if (dot < detectionAngle) continue;
            
            // Compute distance between player and collider
            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }
        return closestInteractable;
    }

    // Draw the detection range in the Scene view for debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);

        // Draw forward direction line
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * interactionRadius);
    }
}
