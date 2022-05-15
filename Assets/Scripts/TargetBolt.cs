using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TargetBolt : MonoBehaviour
{
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite IceSprite;
    private SpriteRenderer _spriteRenderer;

    private GameManager.WorldState currState;
    // Start is called before the first frame update
    private LightningStrike _lightningStrike;
    void Start()
    {
        _lightningStrike = GetComponentInParent<LightningStrike>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SwitchState();
    }

    public void StartLightNing()
    {
        _lightningStrike.Lock = false;

    }

    private void Update()
    {
        if (currState != GameManager.Shared.CurrentState)
        {
           SwitchState(); 
        }
    }

    private void SwitchState()
    {
        currState = GameManager.Shared.CurrentState;

        switch (currState)
        {
            case GameManager.WorldState.Fire:
                _spriteRenderer.sprite = fireSprite;
                break;
            
            case GameManager.WorldState.Ice:
                _spriteRenderer.sprite = IceSprite;
                break;
        }
    }
}
