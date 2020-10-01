using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Font3DString))]
public class TimeText3D : MonoBehaviour
{
    private Font3DString _text3D;

    private DateTime _timeSinceStart = new DateTime();
    
    private void Start()
    {
        _text3D = GetComponent<Font3DString>();
    }

    private void Update()
    {
        _text3D.Text = _timeSinceStart.ToString("mm:ss:fff");
        _timeSinceStart = _timeSinceStart.AddSeconds(Time.deltaTime);
    }
}
