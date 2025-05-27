using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

        
    private void Start()
    {
        if(target == null)
        {
            Debug.LogWarning("Target is not set for ThirdPersonCamera.");
            return;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;
            
        transform.position = target.position;
        transform.rotation = target.rotation;

    }
}
