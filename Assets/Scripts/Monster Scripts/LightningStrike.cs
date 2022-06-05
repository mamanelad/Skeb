using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    private enum Mode
    {
        Show,
        Fade,
    }

    #region Private Fields

    private Material _material;
    private Mode _mode = Mode.Show;
    private Color _color;
    private EnemySpawnerDots _enemySpawner;
    
    #endregion

    #region Public Fields
    
    public bool lockMovement = true;

    #endregion
    
    #region Inspector Control

    [SerializeField] private GameObject father;
    [SerializeField] private float speed = 0.1f;
    [Range(0, 1)] [SerializeField] private float fade = 1f;
    [Range(0, 1)] [SerializeField] private float show;

    #endregion
    
    #region Animator Labels

    private static readonly int Show = Shader.PropertyToID("Show");
    private static readonly int Mode1 = Shader.PropertyToID("Mode");
    private static readonly int Fade = Shader.PropertyToID("Fade");

    #endregion

    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
        _material.SetInt(Mode1, 1);

        _color = GetComponent<SpriteRenderer>().color;
        _color.a = 0;
        GetComponent<SpriteRenderer>().color = _color;
    }

    private void Start()
    {
        _enemySpawner = GetComponentInParent<EnemySpawnerDots>();
        GameManager.Shared.AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.Lightning);
    }


    void FixedUpdate()
    {
        if (lockMovement) return;

        _color.a = 1;
        GetComponent<SpriteRenderer>().color = _color;
        switch (_mode)
        {
            case Mode.Show:
                _material.SetFloat(Show, show);
                show += speed;
                if (show >= 1)
                {
                    _mode = Mode.Fade;
                    _material.SetInt(Mode1, 0);
                }

                break;

            case Mode.Fade:
                _material.SetFloat(Fade, fade);
                fade -= speed;
                if (fade <= 0)
                {
                    _enemySpawner.CreatMonster();
                    Destroy(father);
                }

                break;
        }
    }
}