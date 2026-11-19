using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private TopDownPlayerController playerController;
    private Animator animator;

    private int currentFacing = -1;
    private bool wasMoving;

    void Awake()
    {
        playerController = GetComponent<TopDownPlayerController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        SetFacing();
        SetMoving();
    }

        void SetFacing()
    {
        int facing = (int)playerController.FacingDirection;

        if (currentFacing == facing)
            return;

        currentFacing = facing;
        animator.SetInteger("Facing", facing);
    }

    void SetMoving()
    {
        bool isMoving = playerController.IsMoving;

        if (wasMoving == isMoving)
            return;

        wasMoving = isMoving;
        animator.SetBool("isMoving", isMoving);
    }
}
