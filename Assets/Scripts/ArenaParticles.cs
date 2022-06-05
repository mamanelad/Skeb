using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaParticles : MonoBehaviour
{
    [SerializeField] private bool show;
    [SerializeField] private bool showFire;
    [SerializeField] private bool showIce;
    [SerializeField] private GameObject fireArenaParticle;
    [SerializeField] private GameObject iceArenaParticle;
    private FireParticleEffect _fireParticleEffect;

    private void Start()
    {
        _fireParticleEffect = GetComponent<FireParticleEffect>();
    }

    public void StartParticles()
    {
        if (!show) return;
        switch (GameManager.Shared.CurrentState)
        {
            case GameManager.WorldState.Fire:
                if (!showFire) return;
                    _fireParticleEffect.ParticlePrefab = fireArenaParticle;
                break;
            
            case GameManager.WorldState.Ice:
                if (!showIce) return;
                _fireParticleEffect.ParticlePrefab = iceArenaParticle;
                break;
            
        }

        _fireParticleEffect.isOn = true;
    }
}
    
    
