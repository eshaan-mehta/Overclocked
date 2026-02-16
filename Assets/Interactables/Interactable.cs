using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Interactaqble : MonoBehaviour
{
    [Header("Highlight Settings")]
    private Color highlightColor = Color.white;

    [Range(0f, 1f)]
    public float highlightIntersity = 0.3f;
    
}