using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TargetBolt : MonoBehaviour
{
    // Start is called before the first frame update
    private LightningStrike _lightningStrike;
    void Start()
    {
        _lightningStrike = GetComponentInParent<LightningStrike>();
    }

    public void StartLightNing()
    {
        _lightningStrike.Lock = false;

    }
    
    
    
}
