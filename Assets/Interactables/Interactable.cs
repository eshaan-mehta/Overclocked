using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Interactable : MonoBehaviour
{
    [Header("Highlight Settings")]
    private Color highlightColor = Color.white;

    [Range(0f, 1f)]
    public float highlightIntersity = 0.3f;

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material.EnableKeyword("_EMISSION");
    }

    public void SetHighlighted(bool highlighted) 
    {
        Color emission = highlighted ? highlightColor * highlightIntersity : Color.black;
        objectRenderer.material.SetColor("_EmissionColor", emission);
    }
}