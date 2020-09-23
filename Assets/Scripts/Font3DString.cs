using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Font3DString : MonoBehaviour
{
    
    [SerializeField] private ScriptableObjectFont3D font3D;
    [SerializeField] private string text;
    [SerializeField] private float fontSizeInUnits = 1;
    [SerializeField] private float letterSpacingPercent = 1;
    [SerializeField] private Color textColor = Color.white;

    private enum HorizontalAlignments
    {
        Left,
        Center,
        Right
    }
    private enum VerticalAlignments
    {
        Bottom,
        Center,
        Top
    }
    [SerializeField] private HorizontalAlignments horizontalAlignments;
    [SerializeField] private VerticalAlignments verticalAlignments;

    private List<MeshFilter> _meshFilters = new List<MeshFilter>();
    private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();

    private void Start()
    {
        float x = 0;

        switch (horizontalAlignments)
        {
            case HorizontalAlignments.Left:
                x = 0;
                break;
            case HorizontalAlignments.Center:
                x = ((text.Length - 1) / 2f) * fontSizeInUnits * letterSpacingPercent * -1;
                break;
            case HorizontalAlignments.Right:
                x = (text.Length - 1) * fontSizeInUnits * letterSpacingPercent * -1;
                break;
        }
        
        if (font3D.allCaps) text = text.ToUpper();
        
        foreach (char c in text)
        {
            if (char.IsWhiteSpace(c))
            {
                x += fontSizeInUnits * letterSpacingPercent;
                continue;
            }
            
            if (font3D.fontString.Contains(c.ToString()))
            {
                int id = font3D.fontString.IndexOf(c);
                Transform letter = Instantiate(font3D.fontModels.transform.GetChild(id), transform);
                letter.localPosition = new Vector3(x, 0);
                letter.localRotation = Quaternion.identity;
                letter.localScale = Vector3.one * fontSizeInUnits;
                
                _meshFilters.Add(letter.GetComponent<MeshFilter>());
                _meshRenderers.Add(letter.GetComponent<MeshRenderer>());
                letter.GetComponent<MeshRenderer>().material.SetColor("_MainColor", textColor);
                x += fontSizeInUnits * letterSpacingPercent;
            }
        }
    }
}
