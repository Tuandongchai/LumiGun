using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeResult
{
    Success,
    Failure,
    Inprogress
}
public abstract class BTNode
{
    
    public NodeResult UpdateNode()
    {
        if (!started)
        {
            started = true;
            NodeResult execResult = Execute();
            if(execResult != NodeResult.Inprogress)
            {
                EndNode();
                return execResult;
            }
        }
        NodeResult updateResult = Update();
        if(updateResult != NodeResult.Inprogress) {
            EndNode();
        }
        return updateResult;
    }
    protected virtual NodeResult Execute()
    {
        return NodeResult.Success;
    }
    protected virtual NodeResult Update()
    {
        return NodeResult.Success;
    }
    protected virtual void End()
    {

    }
    private void EndNode()
    {
        started=false;
        End();
    }
    public void Abort()
    {
        EndNode();
    }
    bool started = false;
    int priority;
    
    public int GetPriority()
    {
        return priority;
    }
    public virtual void SortPriority(ref int priorityConter)
    {
        priority = priorityConter++;
        Debug.Log($"{this} has priorty{priority}");
    }
    public virtual void Inityalize()
    {

    }
    public virtual BTNode Get()
    {
        return this;
    }
}
