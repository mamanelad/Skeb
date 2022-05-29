using System;
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

    [SerializeField] private bool stateSwitchAutomatically;
    [SerializeField] private float timeInStage = 4;
    [NonSerialized] public bool StageDamage;

    private float _stageTimer;

    [HideInInspector] public int fireCoins;
    [HideInInspector] public int iceCoins;

    public WorldState CurrentState;

    private void Awake()
    {
        if (Shared == null)
            Shared = this;

        CurrentState = WorldState.Fire;
        _stageTimer = 4;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.LeftControl))
            SwitchState();

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);

        UpdateStageTimer();
    }

    private void UpdateStageTimer()
    {
        var timeScaler = Time.deltaTime;

        _stageTimer = CurrentState switch
        {
            WorldState.Fire => Mathf.Max(_stageTimer - timeScaler, 0),
            WorldState.Ice => Mathf.Min(_stageTimer + timeScaler, timeInStage),
            _ => 1
        };

        if (_stageTimer >= timeInStage || _stageTimer <= 0)
        {
            if (stateSwitchAutomatically)
                SwitchState();
            else
                StageDamage = true;
        }
        else
        {
            StageDamage = false;
        }

        UIManager.Shared.SetStageStateBar(1 - _stageTimer / timeInStage);
    }


    private void SwitchState()
    {
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