using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionScript : MonoBehaviour
{
    [SerializeField] private GameObject reflection;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _objectSpriteRenderer;
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _objectSpriteRenderer = reflection.GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (_objectSpriteRenderer == null)
            return;

        _spriteRenderer.sprite = _objectSpriteRenderer.sprite;

    }
}
