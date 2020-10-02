using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public bool localAxes = false;
    public bool lockToY = true;
    public bool flipFacingDirection = true;
    public bool lerpToDirection = false;
    public enum AimModes
    {
        LookAtCamera,
        CopyCameraForward
    }
    public AimModes aimMode = AimModes.LookAtCamera;

    void FixedUpdate()
    {
        float dir = (flipFacingDirection ? -1 : 1);
        switch(aimMode)
        {
            case AimModes.LookAtCamera:
            {
                Quaternion target =
                    Quaternion.LookRotation((transform.position - Camera.main.transform.position).normalized * dir);

                if (lerpToDirection) target = Quaternion.Lerp(transform.rotation, target, 10 * Time.deltaTime);

                transform.rotation = target;

                break;
            }
            case AimModes.CopyCameraForward:
            {
                Vector3 target = Camera.main.transform.forward * dir;

                if (lerpToDirection) target = Vector3.Slerp(transform.forward, target, 10 * Time.deltaTime);
                
                transform.forward = target;
                break;
            }
        }
        
        if (lockToY)
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        
        if (localAxes)
            transform.localRotation = Quaternion.Euler(new Vector3(0, transform.localRotation.eulerAngles.y, 0));
    }

}
