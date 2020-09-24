using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public Space space = Space.World;
    public Vector3 axis = new Vector3(0, 1, 0);
    
    public float speed = 5f;

    private void Update()
    {
        switch (space)
        {
            case Space.World:
                transform.Rotate(axis, speed * Time.deltaTime, Space.World);
                break;
            case Space.Self:
                Vector3 val = transform.localRotation.eulerAngles;
                val.x += axis.x * speed * Time.deltaTime;
                val.y += axis.y * speed * Time.deltaTime;
                val.z += axis.z * speed * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(val);
                break;
        }
    }
}
