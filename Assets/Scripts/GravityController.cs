using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class GravityController : MonoBehaviour
{

    public Transform gravityGizmo;
    public GravityOrbit gravity;
    public float rotationSpeed = 20;
    public CinemachineVirtualCamera optionalVirtualCamera;
    
    private Rigidbody _rigidbody;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (gravity)
        {
            var gravityUp = (transform.position - gravity.transform.position).normalized;
            
            //Vector3 localUp = transform.up;
            
            //Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;
            
            gravityGizmo.up = Vector3.Lerp(gravityGizmo.up, gravityUp, rotationSpeed * Time.deltaTime);
            
            // push down for gravity
            _rigidbody.AddForce(-gravityUp * (gravity.gravity * _rigidbody.mass));
        }
    }

    private void LateUpdate()
    {
        if (optionalVirtualCamera)
        {
            optionalVirtualCamera.transform.rotation = Quaternion.Euler(new Vector3(gravityGizmo.rotation.eulerAngles.x + 25, 0, 0));
        }
    }
}
