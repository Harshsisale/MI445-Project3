using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private TopDownPlayerController playerController;
    private Animator animator;

    void Awake()
    {
        playerController = GetComponent<TopDownPlayerController>();
        animator = GetComponentInChildren<Animator>();
    }
}
