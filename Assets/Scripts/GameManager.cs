using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum WorldState {None, Ice, Fire};

    public WorldState CurrentState;
    
    public static GameManager Shared;
    private void Awake()
    {
        if (Shared == null)
            Shared = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            CurrentState = WorldState.Ice;
        
        if (Input.GetKeyDown(KeyCode.O))
            CurrentState = WorldState.Fire;
    }
}
