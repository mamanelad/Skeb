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
        Pause,
        Won,
        Lose
    }

    public static GameManager Shared;

    [SerializeField] public AudioManager PlayerAudioManager;
    [SerializeField] public AudioManagerStore StoreAudioManager;
    [SerializeField] public AudioManagerMonster monsterAudioManager;
    [SerializeField] public AudioManagerGeneral AudioManagerGeneral;


    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameWonScreenMenu;
    [SerializeField] private GameObject gameOverScreenMenu;


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
    private GameState _prevGameState = GameState.Arena;
    private float _stageTimer;
    
    [SerializeField] private float toIceDashDelayTime = .3f;
    [NonSerialized] public bool canDash;
    private float dashDelayTimer;

    public WorldState CurrentState;

    private void Awake()
    {
        //Cursor.visible = false;

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
        _gameControls.GameControl.Pause.performed += PauseGame;
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

    private void Start()
    {
        if (gameWonScreenMenu == null)
            print("Need To add game won screen");
        if (gameOverScreenMenu == null)
            print("Need To add game won screen");
        if (inTutorial)
            AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.TutorialSong);
        else
            AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.MainSong);
    }

    private void Update()
    {
        if (!canDash)
            DashDelay();
        
        if (!dontChangeStateByTime)
            UpdateStageTimer();

        if (!inTutorial)
            UpdateRoundText();

        triggerKillCamera = roundMonsterKillCounter + 1 == roundMonsterTotalAmount;
    }

    private void DashDelay()
    {
        dashDelayTimer -= Time.deltaTime;
        if (dashDelayTimer <= 0)
            canDash = true;
    } 
    private void UpdateRoundText()
    {
        UIManager.Shared.SetUIText(roundNumber, roundMonsterKillCounter, roundMonsterTotalAmount);
    }

    private void UpdateStageTimer()
    {
        if (CurrentGameState != GameState.Arena)
            return;
        
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
        if (CurrentState == WorldState.Fire)
        {
            canDash = false;
            dashDelayTimer = toIceDashDelayTime;
        }
            
        PlaySound(GeneralSound.SoundKindsGeneral.Bell);
        var fireAffects = FindObjectsOfType<FireParticleEffect>();
        if (fireAffects == null)
            return;
        foreach (var fireAffect in fireAffects)
            fireAffect.CloseAndOpenBurningAffect(false);
        if (_arenaParticles != null)
            _arenaParticles.StartParticles();
        
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        if (CurrentGameState == GameState.Pause || CurrentGameState == GameState.Won 
                                                || CurrentGameState == GameState.Lose)
            return;

        if (CurrentGameState == GameState.Arena)
            PauseSound(GeneralSound.SoundKindsGeneral.MainSong);
        if (CurrentGameState == GameState.Store)
            Shared.StoreAudioManager.PauseSound(StoreSounds.SoundKindsStore.Background);
        PlaySound(GeneralSound.SoundKindsGeneral.PauseScreenTheme);
        Time.timeScale = 0;
        _prevGameState = CurrentGameState;
        CurrentGameState = GameState.Pause;
        pauseMenu.SetActive(true);
    }
    
    public void WonGame()
    {
        if (CurrentGameState == GameState.Won)
            return;

        FindObjectOfType<PlayerController>().enabled = false;
        // Time.timeScale = 0;
        _prevGameState = CurrentGameState;
        CurrentGameState = GameState.Won;
        gameWonScreenMenu.SetActive(true);
    }
    
    public void GameOver()
    {
        if (CurrentGameState == GameState.Lose)
            return;
        
        AudioManagerGeneral.StopSound(GeneralSound.SoundKindsGeneral.MainSong);
        _prevGameState = CurrentGameState;
        CurrentGameState = GameState.Lose;
        gameOverScreenMenu.SetActive(true);
    }

    public void ResumeState()
    {
        CurrentGameState = _prevGameState;
        
        StopSound(GeneralSound.SoundKindsGeneral.PauseScreenTheme);
        
        if (CurrentGameState == GameState.Arena)
            UnPauseSound(GeneralSound.SoundKindsGeneral.MainSong);
        if (CurrentGameState == GameState.Store)
            Shared.StoreAudioManager.UnPauseSound(StoreSounds.SoundKindsStore.Background);
    }

    public void OpenStore()
    {
        PauseSound(GeneralSound.SoundKindsGeneral.MainSong);
        CurrentGameState = GameState.Store;
        store.SetActive(true);
        store.GetComponent<StoreManager>().EnableUpgrade();
    }

    public void CloseStore()
    {
        UnPauseSound(GeneralSound.SoundKindsGeneral.MainSong);
        CurrentGameState = GameState.Arena;
        store.SetActive(false);
        FindObjectOfType<EnemySpawnerDots>().StartBlockSpawn(true);
    }
    
    private void PlaySound(GeneralSound.SoundKindsGeneral sound)
    {
        Shared.AudioManagerGeneral.PlaySound(sound, transform.position);
    }
    
    private void StopSound(GeneralSound.SoundKindsGeneral sound)
    {
        Shared.AudioManagerGeneral.StopSound(sound);
    }
    
    private void PauseSound(GeneralSound.SoundKindsGeneral sound)
    {
        Shared.AudioManagerGeneral.PauseSound(sound);
    }
    
    private void UnPauseSound(GeneralSound.SoundKindsGeneral sound)
    {
        Shared.AudioManagerGeneral.UnPauseSound(sound);
    }
}