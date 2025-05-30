using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IBehaviorTreeInterface, ITeamInterface, ISpawnInterface
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] Animator animator;
    [SerializeField] PerceptionComponent perceptionComp;
    [SerializeField] BehaviorTree behaviorTree;
    [SerializeField] MovementComponent movementComponent;
    [SerializeField] int TeamID = 2;

    Vector3 prevPos;
    [SerializeField] Reward killReward;
    public int GetTeamID()
    {
        return TeamID;
    }
    public  Animator Animator
    {
        get { return animator; }
        private set { animator = value; }
    }
    private void Awake()
    {
        perceptionComp.onPerceptionTargetChanged += TargetChanged;
        
    }
    private void OnEnable()
    {
        if (healthComponent == null)
            return;
        healthComponent.onHealthEmpty += StartDeath;
        healthComponent.onTakeDamage += TakenDamage;

    }
    private void OnDisable()
    {
        healthComponent.onHealthEmpty -= StartDeath;
        healthComponent.onTakeDamage -= TakenDamage;
        perceptionComp.onPerceptionTargetChanged -= TargetChanged;
        
    }
    protected virtual void Start()
    {
        prevPos = transform.position;
    }
    private void Update()
    {
        CalculateSpeed();
    }

    private void CalculateSpeed()
    {
        if (movementComponent == null) return;

        Vector3 posDelta = transform.position - prevPos;
        float speed = posDelta.magnitude / Time.deltaTime;

        Animator.SetFloat("Speed", speed);
        prevPos = transform.position;
    }

    private void TargetChanged(GameObject target, bool sensed)
    {
        if(sensed)
        {
            behaviorTree.Blackboard.SetOrAddData("Target", target);
        }
        else
        {
            behaviorTree.Blackboard.SetOrAddData("LastSeenLoc", target.transform.position);
            behaviorTree.Blackboard.RemoveBlackboardData("Target");
        }
    }
    private void TakenDamage(float health, float delta, float maxHealth, GameObject instigagor)
    {

    }

    private void StartDeath(GameObject killer)
    {
        TriggerDeathAnimation();
        IRewardListener[] RewardListeners = killer.GetComponents<IRewardListener>();
        foreach(IRewardListener listener in RewardListeners)
        {
            listener.Reward(killReward);
        }
    }
    private void TriggerDeathAnimation()
    {
        if (animator == null)
            return;
        animator.SetTrigger("Dead");
    }
    public void OnDeathAnimationFinished()
    {
        Dead();
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if(behaviorTree && behaviorTree.Blackboard.GetBlackboardData("Target", out GameObject target))
        {
            Vector3 drawTargetPos = target.transform.position + Vector3.up;
            Gizmos.DrawWireSphere(drawTargetPos, 0.7f);

            Gizmos.DrawLine(transform.position + Vector3.up, drawTargetPos);
        }
    }

    public void RotateTowards(GameObject target, bool vertialAim = false)
    {
        Vector3 AimDir = target.transform.position - transform.position;
        AimDir.y=vertialAim?AimDir.y:0;
        AimDir = AimDir.normalized;

        movementComponent.RotateTowards(AimDir);
    }

    public virtual void AttackTarget(GameObject target)
    {
        
    }

    public void SpawnedBy(GameObject spawnerGameobject)
    {
        BehaviorTree spawnerBehaviorTree = spawnerGameobject.GetComponent<BehaviorTree>();
        if(spawnerBehaviorTree != null && spawnerBehaviorTree.Blackboard.GetBlackboardData<GameObject>("Target", out GameObject spawnerTarget))
        {
            PerceptionStimuli targetStimuli = spawnerTarget.GetComponent<PerceptionStimuli>();
            if(perceptionComp && targetStimuli)
            {
                perceptionComp.AssignPercievedStimuli(targetStimuli);
            }
        }
    }
    protected virtual void Dead()
    {

    }
}
