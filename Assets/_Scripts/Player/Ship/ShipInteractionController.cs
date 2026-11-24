using UnityEngine;

public class ShipInteractionController : MonoBehaviour
{
    public Typewriter typewriter;
    public SpaceInteractable[] interactables;

    private SpaceInteractable currentTarget;
    private SpaceInteractable messageSource;

    private void Awake()
    {
        if (typewriter == null)
        {
            Debug.LogError("No typewriter assigned.", this);
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        HideMessageIfOutOfRange();
        FindNearestTarget();

        if (InputManager.interactPressed && currentTarget != null)
            InteractWithTarget();
    }

    private void HideMessageIfOutOfRange()
    {
        if (messageSource == null ||
            !messageSource.IsInRange(transform.position))
        {
            typewriter.hideText();
            messageSource = null;
        }
    }

    private void FindNearestTarget()
    {
        SpaceInteractable nearest = null;
        float nearestDistance = Mathf.Infinity;

        // Keep the current target if two objects are equally close.
        if (currentTarget != null &&
            currentTarget.IsInRange(transform.position))
        {
            nearest = currentTarget;
            nearestDistance = currentTarget.GetDistance(transform.position);
        }

        foreach (SpaceInteractable interactable in interactables)
        {
            if (interactable == null ||
                !interactable.IsInRange(transform.position))
            {
                continue;
            }

            float distance = interactable.GetDistance(transform.position);

            if (distance < nearestDistance)
            {
                nearest = interactable;
                nearestDistance = distance;
            }
        }

        if (nearest == currentTarget)
            return;

        if (currentTarget != null)
            currentTarget.SetHighlighted(false);

        currentTarget = nearest;

        if (currentTarget != null)
            currentTarget.SetHighlighted(true);
    }

    private void InteractWithTarget()
    {
        // E finishes typing only when interacting with the same object.
        if (currentTarget == messageSource && typewriter.IsTyping)
        {
            typewriter.completeTyping();
            return;
        }

        typewriter.hideText();
        messageSource = null;

        // Remember which object owns the message.
        if (currentTarget.levelTransition == null)
            messageSource = currentTarget;

        currentTarget.Interact(typewriter);
    }

    private void OnDisable()
    {
        if (currentTarget != null)
            currentTarget.SetHighlighted(false);

        if (typewriter != null)
            typewriter.hideText();

        currentTarget = null;
        messageSource = null;
    }
}
