using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Controls")]
    public float playerSpeed = 5f;
    public float crouchSpeed = 3f;
    public float sprintSpeed = 7f;
    public float jumpHeight = 0.8f;
    public float gravityMultiplier = 2f;
    public float rotationSpeed = 5f;
    public float crouchColliderHeight = 1.35f;

    [Header("Animation Smoothing")]
    [Range(0f, 1f)]
    public float speedDampTime = 0.1f;
    [Range(0f, 1f)]
    public float velocityDampTime = 0.9f;
    [Range(0f, 1f)]
    public float rotationDampTime = 0.2f;
    [Range(0f, 1f)]
    public float airControl = 0.5f;
}
