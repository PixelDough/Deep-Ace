using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIFadeController : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Animator animator;
    // private Material _material;
    // private readonly Color _baseColor = Color.HSVToRGB(.90f, 1f, 0f);
    // private readonly Color _fullColor = Color.HSVToRGB(.90f, 0f, 1f);
    // private Color _color = Color.HSVToRGB(0, 0, 0);

    private void Update()
    {
        //image.color = _color;

        // if (Input.GetKeyDown(KeyCode.Space))
        //     FadeIn();
        // if (Input.GetKeyDown(KeyCode.P))
        //     FadeOut();
    }

    public void FadeOut()
    {
        //LeanTween.color(image.rectTransform, _baseColor, 1f).setEaseInOutCirc();
        animator.CrossFade("Fade Out", 0.2f);
    }

    public void FadeIn()
    {
        //LeanTween.color(image.rectTransform, _fullColor, 1f).setEaseInOutCirc();
        animator.CrossFade("Fade In", 0.2f);
    }
    
}
