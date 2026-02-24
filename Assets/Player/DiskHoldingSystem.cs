using UnityEngine;

public class DiskHoldingSystem : MonoBehaviour
{
    [Header("Hold Settings")]
    [SerializeField] private Transform holdPosition;
    [SerializeField] private float heldScale = 0.5f;

    private Disk heldDisk;

    public bool IsHoldingDisk()
    {
        return heldDisk != null;
    }

    public Disk GetHeldDisk()
    {
        return heldDisk;
    }

    public void PickUpDisk(Disk disk)
    {
        if (disk == null)
        {
            Debug.LogWarning("DiskHoldingSystem: Attempted to pick up null disk");
            return;
        }

        if (holdPosition == null)
        {
            Debug.LogError("DiskHoldingSystem: holdPosition not assigned");
            return;
        }

        // Store reference
        heldDisk = disk;

        // Parent to hold position
        disk.transform.SetParent(holdPosition);
        disk.transform.localPosition = Vector3.zero;
        disk.transform.localRotation = Quaternion.identity;

        // Scale down
        Vector3 originalScale = disk.GetOriginalScale();
        disk.transform.localScale = originalScale * heldScale;

        // Disable physics
        disk.EnablePhysics(false);

        // Clear parent table reference
        disk.SetParentTable(null);

        // Turn off highlight while held
        disk.SetHighlighted(false);
    }

    public void PlaceDisk(Table targetTable)
    {
        if (heldDisk == null)
        {
            Debug.LogWarning("DiskHoldingSystem: No disk to place");
            return;
        }

        if (targetTable == null)
        {
            Debug.LogWarning("DiskHoldingSystem: Target table is null");
            return;
        }

        Transform diskSlot = targetTable.GetDiskSlot();
        if (diskSlot == null)
        {
            Debug.LogError("DiskHoldingSystem: Target table has no disk slot");
            return;
        }

        // Position at disk slot
        heldDisk.transform.SetParent(targetTable.transform);
        heldDisk.transform.position = diskSlot.position;
        heldDisk.transform.rotation = diskSlot.rotation;

        // Restore original scale
        heldDisk.transform.localScale = heldDisk.GetOriginalScale();

        // Enable physics
        heldDisk.EnablePhysics(true);

        // Update references
        targetTable.PlaceDisk(heldDisk);
        heldDisk.SetParentTable(targetTable);

        // Refresh highlighting - ensure the newly placed disk is highlighted if table is highlighted
        targetTable.SetHighlighted(true);

        // Clear held disk
        heldDisk = null;
    }
}
