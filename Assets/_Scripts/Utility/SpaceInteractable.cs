using UnityEngine;

public class SpaceInteractable : MonoBehaviour
{
    [Header("Interaction")]
    public Collider2D interactionCollider;
    public float interactionDistance = 2;

    [TextArea]
    public string message = "You can't land here."; //Message if no level transition
    public LevelTransition levelTransition;

    [Header("Aura")]
    public GameObject aura;
    public float pulseSpeed = 3f;
    public float pulseSize = 0.04f;
    
    private Vector3 originalAuraScale;
    private bool highlighted; 
    private void Awake()
    {
        if (aura != null)
        {
            originalAuraScale = aura.transform.localScale;
            aura.SetActive(false);
        }
    }

    private void Update()
    {
        if (highlighted == true && aura != null)
        {
            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseSize; // Equation for the pulsing effect
            aura.transform.localScale = originalAuraScale * pulse; // Apply the pulsing effect to the aura's scale
        }
    }

    public float GetDistance(Vector2 shipPosition)
    {
        if (interactionCollider == null ||
            !interactionCollider.enabled ||
            !interactionCollider.gameObject.activeInHierarchy)
        {
            return Mathf.Infinity;
        }

        Vector2 closestPoint =
            interactionCollider.ClosestPoint(shipPosition);

        return Vector2.Distance(shipPosition, closestPoint);
    }

    public bool IsInRange(Vector2 shipPosition)
    {
        return isActiveAndEnabled &&
               GetDistance(shipPosition) <= interactionDistance;
    }

    public void SetHighlighted(bool value)
    {
        highlighted = value;

        if (aura != null)
        {
            aura.SetActive(value);

            if (!value)
                aura.transform.localScale = originalAuraScale;
        }
    }

    public void Interact(Typewriter typewriter)
    {
        if (levelTransition != null)
            levelTransition.loadLevel();
        else
            typewriter.showMessage(message);
    }

    private void OnDisable()
    {
        SetHighlighted(false);
    }
}
