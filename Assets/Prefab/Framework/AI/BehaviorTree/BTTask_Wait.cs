using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_Wait : BTNode
{
    float waitTime = 2f;
    float timeElapsed = 0f;
    public BTTask_Wait(float waitTime)
    {
        this.waitTime = waitTime;
    }
    protected override void End()
    {
        base.End();
    }

    protected override NodeResult Execute()
    {
        if(waitTime <= 0){
            return NodeResult.Success;
        }
        Debug.Log($"wait: {waitTime}");
        timeElapsed = 0f;
        return NodeResult.Inprogress;
    }

    protected override NodeResult Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed > waitTime)
        {
            Debug.Log("Wait");
            return NodeResult.Success;
        }
        return NodeResult.Inprogress;
    }
}
