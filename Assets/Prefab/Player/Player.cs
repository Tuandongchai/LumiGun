using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ITeamInterface
{
    [SerializeField] JoyStick moveStick;
    [SerializeField] JoyStick aimStick;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float maxMoveSpeed = 80f;
    [SerializeField] float minMoveSpeed = 5f;
    [SerializeField] float animTurnSpeed = 15f;
    private CharacterController characterController;
    [SerializeField] MovementComponent movementComponent;
    [SerializeField] int TeamID = 1;

    

    [Header("Inventory")]
    [SerializeField] InventoryComponent inventoryComponent;

    [Header("HeathAndDamage")] 
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] PlayerValueGauge healthBar;


    [Header("AbilityAndStamina")]
    [SerializeField] AbilityComponent abilityComponent;
    [SerializeField] PlayerValueGauge staminaBar;


    [Header("UI")]
    [SerializeField] UIManager uiManager;

    Vector2 moveInput;
    Vector2 aimInput;

    [SerializeField] Camera mainCam;
    CameraController cameraController;
    Animator animator;

    float animatorTurnSpeed;

    public int GetTeamID()
    {
        return TeamID;
    }
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCam = Camera.main;
        cameraController = FindObjectOfType<CameraController>();
        animator = GetComponent<Animator>();
        healthComponent.BroadcastHealthValueImmediately();


    }
    private void OnEnable()
    {
        moveStick.onStickInputValueUpdated += MoveStickUpdated;
        aimStick.onStickInputValueUpdated += AimStickUpdated;
        aimStick.onStickTaped += StartSwichWeapon;
        healthComponent.onHealthChange += HealthChanged;
        healthComponent.onHealthEmpty += StartDeathSequence;

        abilityComponent.onStaminaChange += StaminaChanged;
        abilityComponent.BroadcastStaminaChangeImmediately();

    }
    private void OnDisable()
    {   
        
        moveStick.onStickInputValueUpdated -= MoveStickUpdated;
        aimStick.onStickInputValueUpdated -= AimStickUpdated;
        aimStick.onStickTaped -= StartSwichWeapon;

        healthComponent.onHealthChange -= HealthChanged;
        healthComponent.onHealthEmpty -= StartDeathSequence;

        abilityComponent.onStaminaChange += StaminaChanged;
    }

   
    private void StaminaChanged(float newAmount, float maxAmout)
    {
        staminaBar.UpdateValue(newAmount, 0, maxAmout);
    }

    private void StartDeathSequence(GameObject killer)
    {
        animator.SetLayerWeight(2, 1);
        animator.SetTrigger("Death");
        uiManager.SetGameplayControlEnabled(false);
    }

    private void HealthChanged(float health, float delta, float maxHealth)
    {
        healthBar.UpdateValue(health,delta, maxHealth);
    }

    public void AttackPoint()
    {
        if (inventoryComponent.HasWeapon())
        {
            inventoryComponent.GetActiveWeapon().Attack();

        }
    }

    private void StartSwichWeapon()
    {
        if(inventoryComponent.HasWeapon())
        {
            animator.SetTrigger("switchWeapon");

        }

    }
    public void SwichWeapon()
    {
        inventoryComponent.NextWeapon();
    }

    private void AimStickUpdated(Vector2 inputVal)
    {

        aimInput = inputVal;
        if (inventoryComponent.HasWeapon())
        {
            if (aimInput.magnitude > 0)
            {
                animator.SetBool("attacking", true);
            }
            else
                animator.SetBool("attacking", false);

        }

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

        characterController.Move(Vector3.down * Time.deltaTime * 10f);
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
        float currentTurnSpeed = movementComponent.RotateTowards(aimDir);
        
        animatorTurnSpeed = Mathf.Lerp(animatorTurnSpeed, currentTurnSpeed, Time.deltaTime * animTurnSpeed);
        animator.SetFloat("turnSpeed", animatorTurnSpeed);
    }

    internal void AddMoveSpeed(float boostAmt)
    {
        moveSpeed += boostAmt;
        moveSpeed = Mathf.Clamp(moveSpeed, minMoveSpeed, maxMoveSpeed);
    }
    public void DeathFinished()
    {
        uiManager.SwithToDeathMenu();
    }
}
