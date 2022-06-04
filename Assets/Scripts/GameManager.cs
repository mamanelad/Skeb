using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public enum WorldState
    {
        None,
        Ice,
        Fire
    };

    public static GameManager Shared;

    [SerializeField] private bool stuckStage;
    [SerializeField] private int timeInStage = 15;
    [NonSerialized] public bool StageDamage;
    private bool dontChangeStateByTime;
    public bool inTutorial;
    [NonSerialized] public bool triggerKillCamera; 

    [NonSerialized] public int roundNumber = 1;
    [NonSerialized] public int roundMonsterKillCounter = 0;
    [NonSerialized] public int roundMonsterTotalAmount;

    private ArenaParticles _arenaParticles;
    private float _stageTimer;


    public WorldState CurrentState;
    
    private void Awake()
    {
        if (Shared == null)
            Shared = this;

        CurrentState = WorldState.Fire;
        _stageTimer = 0;

        if (inTutorial)
            dontChangeStateByTime = true;

        _arenaParticles = FindObjectOfType<ArenaParticles>();
    }

    private void Update()
    {
        if (!dontChangeStateByTime)
            UpdateStageTimer();

        if (!inTutorial)
            UpdateRoundText();
        
        triggerKillCamera = roundMonsterKillCounter + 1 == roundMonsterTotalAmount;
    }

    private void UpdateRoundText()
    {
        UIManager.Shared.SetUIText(roundNumber, roundMonsterKillCounter, roundMonsterTotalAmount);
    }

    private void UpdateStageTimer()
    {
        if (CurrentState == WorldState.Fire)
        {
            _stageTimer += Time.deltaTime;
            _stageTimer = math.min(_stageTimer, timeInStage);
        }
        if (CurrentState == WorldState.Ice)
        {
            _stageTimer -= Time.deltaTime;
            _stageTimer = math.max(_stageTimer, 0);
        }

        if (_stageTimer <= 0)
        {
            CurrentState = WorldState.Fire;
            SwitchState();
        }

        if (_stageTimer >= timeInStage)
        {
            CurrentState = WorldState.Ice;
            SwitchState();
        }
            

        var stagePercentage = timeInStage != 0 ? _stageTimer / timeInStage : 0;
        UIManager.Shared.SetStageStateBar(stagePercentage);

        
    }
    
    private void SwitchState()
    {
        var fireAffects = FindObjectsOfType<FireParticleEffect>();
        foreach (var fireAffect in fireAffects)
            fireAffect.CloseAndOpenBurningAffect(false);
        _arenaParticles.StartParticles();
        
    }
}