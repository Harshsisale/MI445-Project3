using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CableController : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactRange = 1.5f;
    [SerializeField] private Transform cableOrigin;

    [Header("Cable Look")]
    [SerializeField] private Material cableMaterial;
    [SerializeField] private float cableWidth = 0.08f;
    [SerializeField] private Color cableColor = new Color(0.45f, 0.35f, 1f, 1f);
    [SerializeField] private int sortingOrder = 10;
    [SerializeField] private string sortingLayerName = "Player";

    private class Segment
    {
        public LightPost a;
        public LightPost b;
        public LineRenderer line;
    }

    private readonly List<Segment> lockedSegments = new List<Segment>();

    private LightPost heldFromPost;
    private LineRenderer heldLine;
    private Material runtimeMaterial;

    void Awake()
    {
        runtimeMaterial = cableMaterial != null
            ? cableMaterial
            : new Material(Shader.Find("Sprites/Default"));
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame) TryConnect();
        if (Keyboard.current.qKey.wasPressedThisFrame) DropHeldCable();
        UpdateHeldLine();
    }

    private void TryConnect()
    {
        LightPost nearest = FindNearestPost();
        if (nearest == null) return;

        if (heldFromPost == null)
        {
            StartHolding(nearest);
        }
        else if (nearest != heldFromPost)
        {
            LockHeldCableTo(nearest);
        }
    }

    private void StartHolding(LightPost post)
    {
        heldFromPost = post;
        heldLine = CreateLine();
        post.SetConnected(true);
    }

    private void LockHeldCableTo(LightPost post)
    {
        heldLine.SetPosition(0, heldFromPost.AnchorPosition);
        heldLine.SetPosition(1, post.AnchorPosition);

        lockedSegments.Add(new Segment { a = heldFromPost, b = post, line = heldLine });
        post.SetConnected(true);

        heldFromPost = post;
        heldLine = CreateLine();
    }

    private void DropHeldCable()
    {
        if (heldFromPost == null) return;

        Destroy(heldLine.gameObject);
        heldLine = null;

        LightPost post = heldFromPost;
        heldFromPost = null;

        if (!IsPostUsed(post)) post.SetConnected(false);
    }

    private bool IsPostUsed(LightPost post)
    {
        foreach (Segment s in lockedSegments)
            if (s.a == post || s.b == post) return true;
        return false;
    }

    private LightPost FindNearestPost()
    {
        LightPost best = null;
        float bestDist = interactRange;

        foreach (LightPost post in LightPost.All)
        {
            float d = Vector2.Distance(transform.position, post.AnchorPosition);
            if (d <= bestDist) { bestDist = d; best = post; }
        }
        return best;
    }

    private void UpdateHeldLine()
    {
        if (heldLine == null) return;
        heldLine.SetPosition(0, heldFromPost.AnchorPosition);
        heldLine.SetPosition(1, cableOrigin.position);
    }

    private LineRenderer CreateLine()
    {
        GameObject go = new GameObject("CableSegment");
        LineRenderer lr = go.AddComponent<LineRenderer>();

        lr.useWorldSpace = true;
        lr.positionCount = 2;
        lr.startWidth = cableWidth;
        lr.endWidth = cableWidth;
        lr.numCornerVertices = 4;
        lr.numCapVertices = 4;
        lr.sharedMaterial = runtimeMaterial;
        lr.startColor = cableColor;
        lr.endColor = cableColor;
        lr.sortingLayerName = sortingLayerName;
        lr.sortingOrder = sortingOrder;

        return lr;
    }
}