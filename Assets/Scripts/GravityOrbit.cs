using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOrbit : MonoBehaviour
{

    public float gravity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GravityController>())
        {
            // if this object has a gravity script, set this as the planet
            other.GetComponent<GravityController>().gravity = this;
        }
    }
}
