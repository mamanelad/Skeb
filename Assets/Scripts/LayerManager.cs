using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    
    private SpriteRenderer _spriteRenderer;

    private float epsilon = .01f;
    private float yPos;
    private int multConst = 10;
    private int layerPos;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
            Destroy(gameObject);
    }

    void Update()
    {
        if (yPos > transform.position.y + epsilon || yPos < transform.position.y - epsilon)
        {
            yPos = transform.position.y;
            layerPos = (int) math.floor(yPos * multConst);
            _spriteRenderer.sortingOrder = -layerPos;
        }
    }
}