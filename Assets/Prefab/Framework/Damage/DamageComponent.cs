using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageComponent : MonoBehaviour, ITeamInterface
{
    [SerializeField] protected bool DamageFriendly;
    [SerializeField] protected bool DamageEnemy;
    [SerializeField] protected bool DamageNeutral;
    ITeamInterface teamInterface;

    public int GetTeamID()
    {
        if(teamInterface != null)
            return teamInterface.GetTeamID();
        return -1;
    }
    public void SetTeamInterfaceSrc(ITeamInterface teamInterface)
    {
        this.teamInterface = teamInterface;
    }
    public bool ShouldDamage(GameObject others)
    {
        if(teamInterface == null)
        {
            return false;
        }
        ETeamRelation relation = teamInterface.GetRelationTowards(others);
        if(DamageFriendly && relation ==ETeamRelation.Friendly)
            return true;
        if(DamageEnemy && relation == ETeamRelation.Enemy)
            return true;
        if(DamageNeutral && relation == ETeamRelation.Neutral)
            return true;

        return false;
    }
}
