using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Disk : Interactable
{
    private Table parentTable;
    private Collider diskCollider;
    private Vector3 originalScale;
    private Color currentHighlightColor = Color.white;

    protected override void Start()
    {
        base.Start();
        diskCollider = GetComponent<Collider>();
        originalScale = transform.localScale;
    }

    public override bool CanBeHighlighted()
    {
        // Disks are never directly highlighted by the detector
        // The parent table handles highlighting the disk
        return false;
    }

    public override bool CanInteract()
    {
        // Disks are never directly interactable
        // Players interact with tablees to pick up/place disks
        return false;
    }

    public override void SetHighlighted(bool highlighted)
    {
        Color emission = highlighted ? currentHighlightColor * highlightIntersity : Color.black;
        objectRenderer.material.SetColor("_EmissionColor", emission);
    }

    public void SetHighlightColor(Color color)
    {
        currentHighlightColor = color;
    }

    public void SetParentTable(Table table)
    {
        parentTable = table;
    }

    public Table GetParentTable()
    {
        return parentTable;
    }

    public void EnablePhysics(bool enabled)
    {
        if (diskCollider != null)
        {
            diskCollider.enabled = enabled;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = !enabled;
        }
    }

    public Vector3 GetOriginalScale()
    {
        return originalScale;
    }
}
