using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTrans;
    [SerializeField] float turnSpeed=200;
    
    private void LateUpdate()
    {
        transform.position= followTrans.position;
    }
    public void AddYawInput(float amt)
    {
        transform.Rotate(Vector3.up, amt * Time.deltaTime*turnSpeed);
    }
}
