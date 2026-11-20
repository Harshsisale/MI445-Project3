using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewThrustStats", menuName = "Ship/Thrust Stats")]
public class ThrustStats : ScriptableObject
{
    [Header("Movement")]
    public float MaxWalkSpeed = 12.5f;
    public float MaxRunSpeed = 20f;
    public float SlideFactor = 0.3f;

    [Header("Physics")]
    public float Acceleration = 15f;
    public float Drag = 3f;
    
    [Header("Pull - Currently Not Used")]
    public float PullStrength = 3f;
    public float GrowthExponent = 2f;
    public float MinPullDistance = 1f;
}
