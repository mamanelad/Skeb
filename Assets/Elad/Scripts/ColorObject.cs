using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    [SerializeField] private ColorManager.ColorGame myColorGame;

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    

    public ColorManager.ColorGame GetMyColor()
    {
        return myColorGame;
    }
}
