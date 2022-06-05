using UnityEngine;

public class ArenaParticles : MonoBehaviour
{
    #region Private Fields

    private FireParticleEffect _fireParticleEffect;

    #endregion

    #region Inspector Control

    [SerializeField] private bool show;
    [SerializeField] private bool showFire;
    [SerializeField] private bool showIce;
    [SerializeField] private GameObject fireArenaParticle;
    [SerializeField] private GameObject iceArenaParticle;

    #endregion

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
                    _fireParticleEffect.particlePrefab = fireArenaParticle;
                break;
            
            case GameManager.WorldState.Ice:
                if (!showIce) return;
                _fireParticleEffect.particlePrefab = iceArenaParticle;
                break;
            
        }

        _fireParticleEffect.isOn = true;
    }
}
    
    
