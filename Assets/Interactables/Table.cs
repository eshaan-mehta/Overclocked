using UnityEngine;

public class Table : Interactable
{
    [Header("Table Settings")]
    [SerializeField] private Disk currentDisk;
    [SerializeField] private Transform diskSlot;

    private DiskHoldingSystem holdingSystem;

    public bool HasDisk => currentDisk != null;

    protected override void Start()
    {
        base.Start();
        // Find the DiskHoldingSystem on the player
        holdingSystem = FindFirstObjectByType<DiskHoldingSystem>();

        if (holdingSystem == null)
        {
            Debug.LogError("Table: Could not find DiskHoldingSystem in scene");
        }

        if (diskSlot == null)
        {
            Debug.LogWarning("Table: diskSlot not assigned");
        }
    }

    public override bool CanInteract()
    {
        if (holdingSystem == null) return false;

        // Can pick up if player is NOT holding and this table HAS a disk
        if (!holdingSystem.IsHoldingDisk() && HasDisk)
        {
            return true;
        }

        // Can place if player IS holding and this table has NO disk
        if (holdingSystem.IsHoldingDisk() && !HasDisk)
        {
            return true;
        }

        return false;
    }

    public override void OnInteract()
    {
        if (holdingSystem == null) return;

        if (HasDisk)
        {
            // Pickup flow
            Disk diskToPickup = RemoveDisk();
            holdingSystem.PickUpDisk(diskToPickup);
        }
        else
        {
            // Place flow
            holdingSystem.PlaceDisk(this);
        }
    }

    public void PlaceDisk(Disk disk)
    {
        if (disk == null) return;

        currentDisk = disk;
        disk.SetParentTable(this);
    }

    public Disk RemoveDisk()
    {
        Disk disk = currentDisk;
        currentDisk = null;
        return disk;
    }

    public override void SetHighlighted(bool highlighted)
    {
        // Highlight the table
        base.SetHighlighted(highlighted);

        // Also highlight the disk if present
        if (currentDisk != null)
        {
            currentDisk.SetHighlighted(highlighted);
        }
    }

    public Transform GetDiskSlot()
    {
        return diskSlot;
    }
}
