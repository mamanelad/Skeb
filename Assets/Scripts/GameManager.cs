using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    public enum WorldState
    {
        None,
        Ice,
        Fire
    };

    public enum GameState
    {
        Arena,
        Store,
        Pause
    }

    public static GameManager Shared;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject store;
    [SerializeField] private bool stuckStage;
    [SerializeField] private int timeInStage = 15;
    [NonSerialized] public bool StageDamage;
    private bool dontChangeStateByTime;
    public bool inTutorial;
    [NonSerialized] public bool triggerKillCamera; 

    [NonSerialized] public int roundNumber = 0;
    [NonSerialized] public int roundMonsterKillCounter = 0;
    [NonSerialized] public int roundMonsterTotalAmount;

    private ArenaParticles _arenaParticles;
    private GameControls _gameControls;
    [NonSerialized] public GameState CurrentGameState;
    private GameState _prevGameState;
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
        _gameControls = new GameControls();
        CurrentGameState = GameState.Arena;
        _prevGameState = GameState.Arena;
        InitializeControls();
        Time.timeScale = 1f;
    }

    #region Input Actions

    private void InitializeControls()
    {
        _gameControls.GameControl.Pause.performed +=  PauseGame;
    }
    
    
    private void OnEnable()
    {
        _gameControls.GameControl.Enable();
    }

    private void OnDisable()
    {
        _gameControls.GameControl.Disable();
    }

    #endregion

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
        if (fireAffects == null)
            return;
        foreach (var fireAffect in fireAffects)
            fireAffect.CloseAndOpenBurningAffect(false);
        if (_arenaParticles != null)
        {
            _arenaParticles.StartParticles(); 
        }
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        if (CurrentGameState == GameState.Pause)
            return;
        
        Time.timeScale = 0;
        _prevGameState = CurrentGameState;
        CurrentGameState = GameState.Pause;
        pauseMenu.SetActive(true);
    }

    public void ResumeState()
    {
        CurrentGameState = _prevGameState;
    }

    public void OpenStore()
    {
        CurrentGameState = GameState.Store;
        store.SetActive(true);
        store.GetComponent<StoreManager>().EnableUpgrade();
    }

    public void CloseStore()
    {
        CurrentGameState = GameState.Arena;
        store.SetActive(false);
        FindObjectOfType<EnemySpawnerDots>().StartBlockSpawn(true);
    }
}