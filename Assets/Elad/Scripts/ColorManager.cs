using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public enum ColorGame
    {
        NONE,
        RED,
        YELLOW,
        GREEN,
        BLUE
    }


    public static ColorManager Instance;

    [SerializeField] private ColorObject[] _colorObjects;
    [SerializeField] private ColorGame currColorHidden = ColorGame.NONE;

    public ColorGame lastColorHidden;
    // Start is called before the first frame update

    private void OnValidate()
    {
        ColorGame currColorHidden;
    }

    private void Awake()
    {
        Instance = this;
        lastColorHidden = currColorHidden;
    }

    void Start()
    {
    }

// Update is called once per frame
    void Update()
    {
        if (lastColorHidden != currColorHidden)
        {
            ChangeColor();
        }
    }


    void ChangeColor()
    {
        foreach (var colorObject in _colorObjects)
        {
            if (colorObject.GetMyColor() == lastColorHidden)
            {
                colorObject.gameObject.layer = LayerMask.NameToLayer("Default");
                colorObject.spriteRenderer.enabled = true;
            }
            
            if (colorObject.GetMyColor() == currColorHidden)
            {
                colorObject.gameObject.layer = LayerMask.NameToLayer("No Physics");
                colorObject.spriteRenderer.enabled = false;
            }
            
        }

        lastColorHidden = currColorHidden;
    }
}