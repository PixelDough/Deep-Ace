using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UIWindowController : MonoBehaviour
{

    public bool animateOnActivate = false;
    public bool isModule = false;
    [SerializeField] private RectTransform content;
    [SerializeField] private UIWindowController[] _windowsToOpenWithThisOne; 
    
    public RectTransform rectTransform => (RectTransform) transform;

    private void Start()
    {
        Disable(false);
    }

    private void OnEnable()
    {
        /*foreach (var wind in _windowsToOpenWithThisOne)
        {
            wind.gameObject.SetActive(true);
            wind.enabled = true;
        }*/
        if (!animateOnActivate)
            rectTransform.localScale = Vector3.one;
        else
        {
            rectTransform.localScale = new Vector3(0, 1, 1);
            rectTransform.LeanCancel();
            rectTransform.LeanScaleX(1f, .1f).setEaseLinear();
        }
    }

    public void Disable(bool doAnimation = false)
    {
        if (doAnimation)
        {
            rectTransform.LeanCancel();
            rectTransform.LeanScaleX(0f, .1f).setEaseLinear().setOnComplete(() => {gameObject.SetActive(false);});
        }
        else
        {
            gameObject.SetActive(false);
        }
        
        enabled = false;
    }
}
