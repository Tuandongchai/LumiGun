using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SenseComponent : MonoBehaviour
{
    [SerializeField] float forgettingTime = 5f;
    static List<PerceptionStimuli> registeredStimulis=new List<PerceptionStimuli>();// list chua tat ca stimuli dc dang ky
    List<PerceptionStimuli> perceptionStimulis = new List<PerceptionStimuli>();// ap dung cho tung sensecomponent dang cam nhan dc

    Dictionary<PerceptionStimuli, Coroutine> forgettingRoutines =new Dictionary<PerceptionStimuli, Coroutine>();

    public delegate void OnPerceptionUpdate(PerceptionStimuli stimuli, bool successfulySensed);
    public event OnPerceptionUpdate onPerceptionUpdate;
    public static void RegisterStimuli(PerceptionStimuli stimuli)
    {
        if(registeredStimulis.Contains(stimuli)) return;
        registeredStimulis.Add(stimuli);
    }
    public static void UnRegisterStimuli(PerceptionStimuli stimuli)
    {
        registeredStimulis.Remove(stimuli);
    }

    protected abstract bool IsStimuliSensable(PerceptionStimuli stimuli);

    private void Update()
    {
        foreach(var stimuli in registeredStimulis)
        {
            if (IsStimuliSensable(stimuli))
            {
                if (!perceptionStimulis.Contains(stimuli))
                {
                    perceptionStimulis.Add (stimuli);
                    if(forgettingRoutines.TryGetValue(stimuli, out Coroutine routine))
                    {
                        StopCoroutine(routine);
                        forgettingRoutines.Remove(stimuli);
                    }
                    else
                    {
                        onPerceptionUpdate?.Invoke(stimuli, true);
                        Debug.Log("i see player");
                    }
                }
            }
            else
            {
                if (perceptionStimulis.Contains(stimuli))
                {
                    perceptionStimulis.Remove(stimuli);
                    forgettingRoutines.Add(stimuli, StartCoroutine(ForgetStimuli(stimuli))) ;
                }
            }
        }
    }
    IEnumerator ForgetStimuli(PerceptionStimuli stimuli)
    {
        yield return new WaitForSeconds(forgettingTime);
        forgettingRoutines.Remove(stimuli);
        onPerceptionUpdate?.Invoke(stimuli, false);
        Debug.Log("i lost player");
    }
    protected virtual void DrawDebug()
    {

    }
    private void OnDrawGizmos()
    {
        DrawDebug();
    }
}
