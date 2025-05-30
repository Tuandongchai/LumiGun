using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_AttackTarget : BTNode
{
    BehaviorTree tree;
    string targetKey;
    GameObject target;

    public BTTask_AttackTarget(BehaviorTree tree, string targetKey)
    {
        this.tree = tree;   
        this.targetKey = targetKey;
    }
    protected override NodeResult Execute()
    {
        if(!tree || tree.Blackboard == null || !tree.Blackboard.GetBlackboardData(targetKey, out target))
        {
            return NodeResult.Failure;
        }
        IBehaviorTreeInterface behaviorTreeInterface = tree.GetBehaviorTreeInterface();

        if(behaviorTreeInterface == null)
            return NodeResult.Failure;
        behaviorTreeInterface.AttackTarget(target);
        return NodeResult.Success;
    }
}
