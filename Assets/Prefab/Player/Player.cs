using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] JoyStick moveStick;
    [SerializeField] JoyStick aimStick;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float turnSpeed = 30f;
    [SerializeField] float animTurnSpeed = 15f;
    private CharacterController characterController;
    Vector2 moveInput;
    Vector2 aimInput;

    [SerializeField] Camera mainCam;
    CameraController cameraController;
    Animator animator;

    float animatorTurnSpeed;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCam = Camera.main;
        cameraController = FindObjectOfType<CameraController>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        moveStick.onStickInputValueUpdated += MoveStickUpdated;
        aimStick.onStickInputValueUpdated += AimStickUpdated;

    }


    private void OnDisable()
    {
        
        moveStick.onStickInputValueUpdated -= MoveStickUpdated;
        aimStick.onStickInputValueUpdated -= AimStickUpdated;
    }

    private void AimStickUpdated(Vector2 inputVal)
    {
        aimInput = inputVal;
    }
    private void MoveStickUpdated(Vector2 inputValue)
    {
        moveInput = inputValue;
    }

    Vector3 StickInputToWorldDir(Vector2 inputVal)
    {
        Vector3 rightDir = mainCam.transform.right;
        Vector3 upDir = Vector3.Cross(rightDir, Vector3.up);
        return rightDir * inputVal.x + upDir * inputVal.y;
    }
    private void Update()
    {
        PerformMoveAndAim();
        UpdateCamera();
    }

    private void PerformMoveAndAim()
    {
        Vector3 moveDir = StickInputToWorldDir(moveInput);
        characterController.Move(moveDir * Time.deltaTime * moveSpeed);

        UpdateAim(moveDir);

        float forward = Vector3.Dot(moveDir, transform.forward);
        float right = Vector3.Dot(moveDir, transform.right);

        animator.SetFloat("forwardSpeed", forward);
        animator.SetFloat("rightSpeed", right);
    }

    private void UpdateAim(Vector3 moveDir)
    {
        Vector3 aimDir = moveDir;
        if (aimInput.magnitude != 0)
        {
            aimDir = StickInputToWorldDir(aimInput);
        }
        RotateTowards(aimDir);
    }

    private void UpdateCamera()
    {
        if (moveInput.magnitude != 0 && aimInput.magnitude ==0)
        {
            cameraController?.AddYawInput(moveInput.x);

        }
    }

    private void RotateTowards(Vector3 aimDir)
    {
        float currentTurnSpeed = 0;
        if (aimDir.magnitude != 0)
        {
            Quaternion prevRot = transform.rotation;

            float turnLerpAlpha = turnSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(aimDir, Vector3.up), turnLerpAlpha);

            Quaternion currentRot = transform.rotation;
            float Dir = Vector3.Dot(aimDir, transform.right)>0 ? 1: -1;
            float rotationDelta = Quaternion.Angle(prevRot, currentRot)*Dir;
            currentTurnSpeed = rotationDelta / Time.deltaTime;
        }
        animatorTurnSpeed = Mathf.Lerp(animatorTurnSpeed, currentTurnSpeed, Time.deltaTime * animTurnSpeed);
        animator.SetFloat("turnSpeed", animatorTurnSpeed);
        Debug.Log(animatorTurnSpeed);
    }
}
