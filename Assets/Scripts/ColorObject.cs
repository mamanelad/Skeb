using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    private LayerMask _defaultLayer;
    [SerializeField] private ColorManager.ColorGame myColorGame;

    [NonSerialized] public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update

    private void Awake()
    {
        _defaultLayer = gameObject.layer;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ColorManager.AddColorObject(this);
    }

    public ColorManager.ColorGame GetMyColor()
    {
        return myColorGame;
    }

    public LayerMask GetMyLayerMask()
    {
        return _defaultLayer;
    }
}