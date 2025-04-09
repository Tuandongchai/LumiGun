using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_Spawn : BTNode
{
    SpawnerComponent spawnComponent;
    
    public BTTask_Spawn(BehaviorTree tree)
    {
        spawnComponent = tree.GetComponent<SpawnerComponent>();
    }

    protected override NodeResult Execute()
    {
        if(spawnComponent == null || !spawnComponent.StartSpawn())
        {
            return NodeResult.Failure;
        }
        return NodeResult.Success;
    }
}
