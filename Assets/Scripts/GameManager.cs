using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum WorldState {None ,Ice, Fire};

    public WorldState CurrentState;
    
    public static GameManager Shared;
    private void Awake()
    {
        if (Shared == null)
            Shared = this;

        CurrentState = WorldState.Fire;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            CurrentState = CurrentState switch
            {
                WorldState.Fire => WorldState.Ice,
                WorldState.Ice => WorldState.Fire,
                _ => CurrentState
            };
        }
    }
}
