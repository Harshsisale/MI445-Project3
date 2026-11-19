using System;
using Unity.VisualScripting;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    [SerializeField] private TopDownPlayerController playerController;
    [SerializeField] private Transform torchTransform;

    void Awake()
    {
        playerController = GetComponent<TopDownPlayerController>();
    }

    void Update()
    {
        PositionTorch();
    }

    void PositionTorch()
    {
        if (playerController.FacingDirection == TopDownPlayerController.Facing.Side)
        {
            torchTransform.rotation = Quaternion.Euler(0f, 0f, playerController.FlipX ? -90f : 90f);
        }
        else if (playerController.FacingDirection == TopDownPlayerController.Facing.Up)
        {
            torchTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            torchTransform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }
}
