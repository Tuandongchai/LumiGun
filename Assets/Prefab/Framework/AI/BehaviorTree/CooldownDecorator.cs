using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownDecorator : Decorator
{
    float cooldownTime;
    float lastExecutionTime = -1;
    bool failOnCooldown;

    public CooldownDecorator(BehaviorTree tree,BTNode child,float cooldownTime, bool failOnCooldown =false) : base(child)
    {
        this.cooldownTime = cooldownTime;
        this.failOnCooldown = failOnCooldown;
    }

    protected override NodeResult Execute()
    {
        if (cooldownTime == 0)
            return NodeResult.Inprogress;

        if(lastExecutionTime == -1) {
            lastExecutionTime = Time.timeSinceLevelLoad;
            return NodeResult.Inprogress;
        }

        if(Time.timeSinceLevelLoad - lastExecutionTime < cooldownTime)
        {
            if(failOnCooldown)
            {
                return NodeResult.Failure;
            }
            else
            {
                return NodeResult.Success;
            }
        }

        lastExecutionTime = Time.timeSinceLevelLoad;
        return NodeResult.Inprogress;
    }
    protected override NodeResult Update()
    {
        return GetChild().UpdateNode();
    }
}
