using UnityEngine;

public class Table : Interactable
{
    [Header("Table Settings")]
    [SerializeField] private Disk currentDisk;
    [SerializeField] private Transform diskSlot;

    [Header("Processing Settings")]
    [SerializeField] private GameObject processingTimerPrefab;
    [SerializeField] private Color processingHighlightColor = new Color(1f, 0.3f, 0f); // Red/Orange

    [Header("UI References")]
    [SerializeField] private TimerSelectionUI timerSelectionUI;

    private DiskHoldingSystem holdingSystem;
    private TableProcessingTimer activeTimer;
    private bool isProcessing = false;

    public bool HasDisk => currentDisk != null;
    public bool IsProcessing => isProcessing;

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

        if (timerSelectionUI == null)
        {
            timerSelectionUI = FindFirstObjectByType<TimerSelectionUI>();
        }
    }

    void Update()
    {
        if (isProcessing && activeTimer != null && activeTimer.IsComplete())
        {
            OnProcessingComplete();
        }
    }

    public override bool CanInteract()
    {
        if (holdingSystem == null) return false;

        // Cannot interact while processing
        if (isProcessing) return false;

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
            // Pickup flow - UNCHANGED
            Disk diskToPickup = RemoveDisk();
            holdingSystem.PickUpDisk(diskToPickup);
        }
        else
        {
            // Place flow - show timer selection popup
            if (timerSelectionUI != null)
            {
                timerSelectionUI.ShowPopup(OnTimerSelected);
            }
            else
            {
                Debug.LogError("Table: TimerSelectionUI not found");
                // Fallback: place immediately with 3s default
                OnTimerSelected(3f);
            }
        }
    }

    private void OnTimerSelected(float duration)
    {
        // Call DiskHoldingSystem to physically place the disk
        holdingSystem.PlaceDisk(this, duration);

        // Start processing state
        StartProcessing(duration);
    }

    private void StartProcessing(float duration)
    {
        isProcessing = true;

        // Spawn timer bar
        if (processingTimerPrefab != null)
        {
            GameObject timerObj = Instantiate(processingTimerPrefab, transform);
            activeTimer = timerObj.GetComponent<TableProcessingTimer>();

            if (activeTimer != null)
            {
                activeTimer.Initialize(duration, transform);
            }
        }

        // Update highlight to processing color
        RefreshHighlight();
    }

    private void OnProcessingComplete()
    {
        isProcessing = false;

        if (activeTimer != null)
        {
            Destroy(activeTimer.gameObject);
            activeTimer = null;
        }

        // Restore normal highlight
        RefreshHighlight();
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
        if (isProcessing)
        {
            // Use red/orange processing color
            Color emission = highlighted ? processingHighlightColor * highlightIntersity : Color.black;
            objectRenderer.material.SetColor("_EmissionColor", emission);
        }
        else
        {
            // Normal white highlight
            base.SetHighlighted(highlighted);
        }

        // Highlight disk (but not during processing)
        if (currentDisk != null)
        {
            currentDisk.SetHighlighted(highlighted && !isProcessing);
        }
    }

    private void RefreshHighlight()
    {
        // Refresh the current highlight state
        InteractableDetector detector = FindFirstObjectByType<InteractableDetector>();
        if (detector != null && detector.GetCurrentHighlighted() == this)
        {
            SetHighlighted(true);
        }
    }

    public Transform GetDiskSlot()
    {
        return diskSlot;
    }
}
