using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "PixelDough/Font 3D")]
public class ScriptableObjectFont3D : ScriptableObject
{
    public GameObject fontModels;
    public string fontString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public bool allCaps = true;
}
