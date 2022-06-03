using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIndicatorScript : MonoBehaviour
{
    private SpriteRenderer _sp;
    [SerializeField] private Sprite iceIndicator;
    [SerializeField] private Sprite fireIndicator;
    void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _sp.sprite = GameManager.Shared.CurrentState switch
        {
            GameManager.WorldState.Fire => fireIndicator,
            GameManager.WorldState.Ice => iceIndicator,
            _ => _sp.sprite
        };
    }
}
