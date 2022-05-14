using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum WorldState {Ice, Fire};

    [NonSerialized]public WorldState GameState;
    
    private static GameManager _shared;
    private void Awake()
    {
        if (_shared == null)
            _shared = this;
    }

    
}
