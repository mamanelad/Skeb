using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    }


// Update is called once per frame
    void Update()
    {
        if (lastColorHidden != currColorHidden)
        {
            ChangeColor();
        }
    }


    private void ChangeColor()
    {
        foreach (var colorObject in _colorObjects)
        {
            var bridgeComp = colorObject.gameObject.GetComponent<Bridge>();
            if (colorObject.GetMyColor() == lastColorHidden)
            {
                colorObject.gameObject.layer = colorObject.GetMyLayerMask();
                colorObject.spriteRenderer.enabled = true;

                if (bridgeComp != null)
                {
                    bridgeComp.OnShow();
                }
            }

            if (colorObject.GetMyColor() == currColorHidden)
            {
                colorObject.gameObject.layer = (int) Mathf.Log(_InvisibleLayer, 2);
                colorObject.spriteRenderer.enabled = false;

                if (bridgeComp != null)
                {
                    bridgeComp.OnHide();
                }
            }
        }

        lastColorHidden = currColorHidden;
    }

    public static void AddColorObject(ColorObject colorObject)
    {
        Instance._colorObjects.Add(colorObject);
    }
}