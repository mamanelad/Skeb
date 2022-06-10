using System;
using UnityEngine;

public class EnemySpawnerDots : MonoBehaviour
{
    #region Private Fields

    private bool gameStarted;

    private bool spawnIsOn;
    [NonSerialized] public bool wonLevel;
    private PlayerController _playerController;

    private int _dotIndexBolt; //Position for the bolt
    private int _dotIndexMonster; //Position for the monster
    private int _monsterIndex; //Which monster to initialize
    private float _timer;
    private float _wonScreenTimer;
    private int _monsterCounter; //Count how many monsters are alive.
    private int currentWaveMonsterCounter;


    private Enemy mostWantedEnemy;
    private int _bigMonsterMaxAmount;
    private int _midMonsterMaxAmount;
    private int _smallMonsterMaxAmount;
    private int _bigMonsterCounter;
    private int _midMonsterCounter;
    private int _smallMonsterCounter;


    private int maxTotalMonsterAmount = 9; //Max amount of monster that can be alive.
    private float bigPercentage = .33f;
    private float middlePercentage = .33f;
    private float smallPercentage = .33f;
    private int waveIndex = -1;
    private float openShopTimer;
    private bool openShop;
    private bool wonScreen;
    private bool wonScreenOpen;

    #endregion

    #region Inspector Control

    [SerializeField] private float gameWonScreenDelay = 0.5f;
    [SerializeField] private UpgradeShop upgradeShop;

    [SerializeField] private GameObject lightningStrike;
    // [SerializeField] private GameObject[] monsters;

    [Header("Position Setting")] [SerializeField]
    private Transform[] spawnerDots; //Positions for the monster initialization.


    [Header("Monster Game Objects")] [SerializeField]
    private Enemy monstersBig;

    [SerializeField] private Enemy monstersMiddle;
    [SerializeField] private Enemy monstersSmall;

    [Header("Monster Amount Setting")] [SerializeField]
    private Wave[] _waves;

    [SerializeField] private int monsterKindsAmount = 3;

    [Header("Timing Setting")] [SerializeField]
    private float maxTimeToSpawn = 4; //Amount of time between enemies initialization.

    [SerializeField] private float timeToOpenTheShopDeley = 3f;

    #endregion

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _timer = maxTimeToSpawn;
        SetNewWave();
    }

    private void CalculateMostWanted()
    {
        if (bigPercentage >= middlePercentage && bigPercentage >= smallPercentage)
            mostWantedEnemy = monstersBig;

        else if (middlePercentage >= bigPercentage && middlePercentage >= smallPercentage)
            mostWantedEnemy = monstersMiddle;

        else if (smallPercentage >= bigPercentage && smallPercentage >= middlePercentage)
            mostWantedEnemy = monstersSmall;
    }

    private void Update()
    {
        if (wonScreen && !wonScreenOpen)
        {
            _wonScreenTimer -= Time.deltaTime;
            if (_wonScreenTimer <= 0)
            {
                GameWon();
                wonScreen = false;
                wonScreenOpen = true;
            }
                
        }
        if (_playerController.IsPlayerDead) return;

        
        if (openShop)
        {
            openShopTimer -= Time.deltaTime;
            if (openShopTimer <= 0)
            {
                GameManager.Shared.OpenStore();
                openShop = false;
            }
        }


        if (!spawnIsOn) return;


        if (currentWaveMonsterCounter < maxTotalMonsterAmount - 1)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
                SpawnLightNingBolt();
        }
        else if (_monsterCounter <= 0 && currentWaveMonsterCounter >= maxTotalMonsterAmount)
        {
            spawnIsOn = false;
            SetNewWave();
        }
    }


    private void SpawnLightNingBolt()
    {
        var lightning = Instantiate(lightningStrike,
            spawnerDots[_dotIndexBolt].position, Quaternion.identity);
        _dotIndexBolt = (_dotIndexBolt + 1) % spawnerDots.Length;
        lightning.transform.SetParent(gameObject.transform);
        _timer = maxTimeToSpawn;
    }

    public void DecreaseMonster()
    {
        GameManager.Shared.roundMonsterKillCounter += 1;
        if (_monsterCounter > 0)
            _monsterCounter -= 1;
    }


    public void CreatMonster()
    {
        var monsterToSpawn = mostWantedEnemy;

        if (_monsterIndex == 0)
        {
            if (_bigMonsterCounter < _bigMonsterMaxAmount)
            {
                monsterToSpawn = monstersBig;
                _bigMonsterCounter += 1;
            }
            else
            {
                _monsterIndex += 1;
            }
        }


        if (_monsterIndex == 1)
        {
            if (_midMonsterCounter < _midMonsterMaxAmount)
            {
                monsterToSpawn = monstersMiddle;
                _midMonsterCounter += 1;
            }

            else
            {
                _monsterIndex += 1;
            }
        }

        if (_monsterIndex == 2)
            if (_smallMonsterCounter < _smallMonsterMaxAmount)
            {
                monsterToSpawn = monstersSmall;
                _smallMonsterCounter += 1;
            }


        if (monsterToSpawn)
        {
            var monster = Instantiate(monsterToSpawn.gameObject, spawnerDots[_dotIndexMonster].position,
                Quaternion.identity);
            monster.transform.SetParent(gameObject.transform);
        }


        _monsterIndex = (_monsterIndex + 1) % monsterKindsAmount;
        _dotIndexMonster = (_dotIndexMonster + 1) % spawnerDots.Length;

        currentWaveMonsterCounter += 1;
        _monsterCounter += 1;
    }

    public void SetNewWave()
    {
        
        currentWaveMonsterCounter = 0;
        waveIndex += 1;
        //print("initiating wave number: " + waveIndex);
        if (waveIndex >= _waves.Length && !wonScreen)
        {
            wonScreen = true;
            _wonScreenTimer = gameWonScreenDelay;
            return;
        }
            

        var curWave = _waves[waveIndex];


        maxTotalMonsterAmount = curWave.monsterAmount;
        bigPercentage = curWave.bigPercentage;
        middlePercentage = curWave.middlePercentage;
        smallPercentage = curWave.smallPercentage;
        maxTimeToSpawn = curWave.timeToSpawnStep;

        

        _bigMonsterMaxAmount = (int) Math.Floor(bigPercentage * maxTotalMonsterAmount);
        _midMonsterMaxAmount = (int) Math.Floor(middlePercentage * maxTotalMonsterAmount);
        _smallMonsterMaxAmount = (int) Math.Floor(smallPercentage * maxTotalMonsterAmount);
        CalculateMostWanted();
        if (!gameStarted)
        {
            StartBlockSpawn(true);
            gameStarted = true;
        }
        else
        {
            wonLevel = true;
            openShop = true;
            openShopTimer = timeToOpenTheShopDeley;
        }
    }

    public void StartBlockSpawn(bool mode)
    {
        wonLevel = false;
        spawnIsOn = mode;
        GameManager.Shared.roundMonsterTotalAmount = maxTotalMonsterAmount;
        GameManager.Shared.roundNumber += 1;
        GameManager.Shared.roundMonsterKillCounter = 0;
    }

    private void GameWon()
    {
        spawnIsOn = false;
        GameManager.Shared.WonGame();
    }
}