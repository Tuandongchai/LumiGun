using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimComponent : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] float aimRange = 1000f;
    [SerializeField] LayerMask aimMask;

    public GameObject GetAimTarget()
    {
        Vector3 aimStart = muzzle.position;
        Vector3 aimDir = muzzle.forward;

        if(Physics.Raycast(aimStart, aimDir, out RaycastHit hitInfo, aimRange, aimMask))
        {
            return hitInfo.collider.gameObject;
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(muzzle.position, muzzle.position+muzzle.forward*aimRange);
    }

}
