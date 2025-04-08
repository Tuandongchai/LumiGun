using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    BTNode Root;
    Blackboard blackboard = new Blackboard();
    IBehaviorTreeInterface behaviorTreeInterface;
    /*BTNode prevNode;*/

    public Blackboard Blackboard
    {
        get { return blackboard; }
    }

    protected abstract void ConstructTree(out BTNode rootNode);
    private void Start()
    {
        behaviorTreeInterface = GetComponent<IBehaviorTreeInterface>();
        ConstructTree(out Root);
        SortTree();
    }
    internal IBehaviorTreeInterface GetBehaviorTreeInterface()
    {
        return behaviorTreeInterface;
    }

    private void SortTree()
    {
        int priortyCouter = 0;
        Root.Inityalize();
        Root.SortPriority(ref priortyCouter);
    }

    private void Update()
    {
        Root.UpdateNode();
        /*BTNode currentNode = Root.Get();
        if(prevNode != currentNode)
        {
            prevNode=currentNode;
            Debug.Log($"current node to: {currentNode}");
        }*/
    }
    public void AbortLowerThan(int priority)
    {
        BTNode currentNode = Root.Get();
        if (currentNode.GetPriority()> priority)
        {
            Root.Abort();
        }
    }

    
}
