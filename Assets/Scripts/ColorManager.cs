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
        BLUE,
        WHITE
    }

    [SerializeField] private LayerMask _InvisibleLayer;

    public static ColorManager Instance;

    private List<ColorObject> _colorObjects = new List<ColorObject>();
    [SerializeField] private ColorObject[] _MoreColorObjects;
    [SerializeField] public ColorGame currColorHidden = ColorGame.NONE;

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

        //Adding all the children that are color objects to the list
        ColorObject[] obj = GameObject.FindObjectsOfType<ColorObject>();
        foreach (var o in obj)
            _colorObjects.Add(o);
        
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
                colorObject.gameObject.layer = colorObject.GetMyLayerMask(); //LayerMask.NameToLayer("Default");
                colorObject.spriteRenderer.enabled = true;
            }

            if (colorObject.GetMyColor() == currColorHidden)
            {
                colorObject.gameObject.layer = _InvisibleLayer; //LayerMask.NameToLayer("No Physics");
                colorObject.spriteRenderer.enabled = false;
            }
        }

        lastColorHidden = currColorHidden;
    }
}