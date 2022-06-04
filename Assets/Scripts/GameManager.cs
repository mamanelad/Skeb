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
    [SerializeField] private bool inTutorial;

    [NonSerialized] public int roundNumber = 1;
    [NonSerialized] public int roundMonsterKillCounter = 0;
    [NonSerialized] public int roundMonsterTotalAmount;
    
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
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.LeftControl))
        //     SwitchState();

        // if (Input.GetKeyDown(KeyCode.Escape))
        //     SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
        if (!dontChangeStateByTime)
            UpdateStageTimer();

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
            CurrentState = WorldState.Fire;
        if (_stageTimer >= timeInStage)
            CurrentState = WorldState.Ice;

        var stagePercentage = timeInStage != 0 ? _stageTimer / timeInStage : 0;
        UIManager.Shared.SetStageStateBar(stagePercentage);
    }
    
    private void SwitchState()
    {
        if (stuckStage) return;
            var fireAffects = FindObjectsOfType<FireParticleEffect>();
        foreach (var fireAffect in fireAffects)
            fireAffect.CloseAndOpenBurningAffect(false);


        CurrentState = CurrentState switch
        {
            WorldState.Fire => WorldState.Ice,
            WorldState.Ice => WorldState.Fire,
            _ => CurrentState
        };
    }
}