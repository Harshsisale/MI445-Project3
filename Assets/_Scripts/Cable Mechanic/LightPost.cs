using System.Collections.Generic;
using UnityEngine;

public class LightPost : MonoBehaviour
{
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private GameObject glowVisual; // We can implement this later with a glow sprite

    public static readonly List<LightPost> All = new List<LightPost>();

    public bool IsConnected { get; private set; }
    public Vector3 AnchorPosition => anchorPoint != null ? anchorPoint.position : transform.position;

    void OnEnable() => All.Add(this);
    void OnDisable() => All.Remove(this);

    public void SetConnected(bool connected)
    {
        IsConnected = connected;
        if (glowVisual != null) glowVisual.SetActive(connected);
    }
}