using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour
{
    [SerializeField] private SenseComponent[] senses;
    LinkedList<PerceptionStimuli> currentlyPerceivedStimulis=new LinkedList<PerceptionStimuli>();

    PerceptionStimuli targetStimuli;

    public delegate void OnPerceptionTargetChanged(GameObject target, bool sensed);
    public event OnPerceptionTargetChanged onPerceptionTargetChanged;

    private void Start()
    {
        foreach (SenseComponent sense in senses)
        {
            sense.onPerceptionUpdate += SenseUpdated;
        }
    }

    private void SenseUpdated(PerceptionStimuli stimuli, bool successfulySensed)
    {
        var nodeFound = currentlyPerceivedStimulis.Find(stimuli);
        if(successfulySensed)
        {
            if(nodeFound != null)
            {
                currentlyPerceivedStimulis.AddAfter(nodeFound, stimuli);
            }
            else
            {
                currentlyPerceivedStimulis.AddLast(stimuli);
            }
        }
        else
        {
            currentlyPerceivedStimulis.Remove(nodeFound);
        }
        if(currentlyPerceivedStimulis.Count != 0)
        {
            PerceptionStimuli hightestStimuli = currentlyPerceivedStimulis.First.Value;
            if(targetStimuli==null|| targetStimuli != hightestStimuli)
            {
                targetStimuli = hightestStimuli;
                onPerceptionTargetChanged?.Invoke(targetStimuli.gameObject, true);
            }
        }
        else
        {
            if(targetStimuli != null)
            {
                onPerceptionTargetChanged?.Invoke(targetStimuli.gameObject, false);
                targetStimuli = null;
            }
        }
    }
}
