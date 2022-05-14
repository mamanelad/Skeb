using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    enum Mode
    {
        SHOW,
        FADE,
    }

    // Start is called before the first frame update
    private Material material;
    private Mode _mode = Mode.SHOW;
    private SpriteRenderer _spriteRenderer;
    private Color _color;

    [SerializeField] private float speed = 0.1f;
    [SerializeField] private EnemySpawnerDots _enemySpawner;
    
    [Range(0, 1)] [SerializeField] float fade = 1f;
    [Range(0, 1)] [SerializeField] float show = 0f;


    public bool Lock = true;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        material.SetInt("Mode", 1);
        
        _color = GetComponent<SpriteRenderer>().color;
        _color.a = 0;
        GetComponent<SpriteRenderer>().color = _color;

    }

    

    void FixedUpdate()
    {
        if (Lock) return;

        _color.a = 1;
        GetComponent<SpriteRenderer>().color = _color;
        switch (_mode)
        {
            case Mode.SHOW:
                material.SetFloat("Show", show);
                show += speed;
                if (show >= 1)
                {
                    _mode = Mode.FADE;
                    material.SetInt("Mode", 0);

                }

                break;
            
            case Mode.FADE:
                material.SetFloat("Fade", fade);
                fade -= speed;
                if (fade <= 0)
                    Destroy(gameObject);
                break;
        }
        
    }
}