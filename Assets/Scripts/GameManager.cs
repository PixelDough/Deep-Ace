using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            string date = System.DateTime.Now.ToString(CultureInfo.InvariantCulture);
            date = date.Replace("/","-");
            date = date.Replace(" ","_");
            date = date.Replace(":","-");
            ScreenCapture.CaptureScreenshot("Assets/Screenshots/Screenshot_" + date + ".png");
            #if (UNITY_EDITOR)
                if (Application.isEditor)
                    AssetDatabase.Refresh();
            #endif
        }
    }
}
