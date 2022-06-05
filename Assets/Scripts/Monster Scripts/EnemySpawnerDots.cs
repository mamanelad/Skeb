using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawnerDots : MonoBehaviour
{
    #region Private Fields

    private bool _gameStarted;
    private bool _spawnIsOn;

    private PlayerController _playerController;

    private int _dotIndexBolt; //Position for the bolt
    private int _dotIndexMonster; //Position for the monster
    private int _monsterIndex; //Which monster to initialize
    private float _timer;
    private int _monsterCounter; //Count how many monsters are alive.
    private int _currentWaveMonsterCounter;
    
    private Enemy _mostWantedEnemy;
    private int _bigMonsterMaxAmount;
    private int _midMonsterMaxAmount;
    private int _smallMonsterMaxAmount;
    private int _bigMonsterCounter;
    private int _midMonsterCounter;
    private int _smallMonsterCounter;
    
    private int _maxTotalMonsterAmount = 9; //Max amount of monster that can be alive.
    private float _bigPercentage = .33f;
    private float _middlePercentage = .33f;
    private float _smallPercentage = .33f;
    private int _waveIndex = -1;
    private float _openShopTimer;
    private bool _openShop;

    #endregion

    #region Inspector Control

    [SerializeField] private UpgradeShop upgradeShop;

    [SerializeField] private GameObject lightningStrike;
    // [SerializeField] private GameObject[] monsters;

    [Header("Position Setting")] [SerializeField]
    private Transform[] spawnerDots; //Positions for the monster initialization.


    [Header("Monster Game Objects")] [SerializeField]
    private Enemy monstersBig;

    [SerializeField] private Enemy monstersMiddle;
    [SerializeField] private Enemy monstersSmall;

    [FormerlySerializedAs("_waves")] [Header("Monster Amount Setting")] [SerializeField]
    private Wave[] waves;

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
        if (_bigPercentage >= _middlePercentage && _bigPercentage >= _smallPercentage)
            _mostWantedEnemy = monstersBig;

        else if (_middlePercentage >= _bigPercentage && _middlePercentage >= _smallPercentage)
            _mostWantedEnemy = monstersMiddle;

        else if (_smallPercentage >= _bigPercentage && _smallPercentage >= _middlePercentage)
            _mostWantedEnemy = monstersSmall;
    }

    private void Update()
    {
        if (_playerController.IsPlayerDead) return;
        if (upgradeShop != null)
        {
            if (_openShop)
            {
                _openShopTimer -= Time.deltaTime;
                if (_openShopTimer <= 0)
                {
                    upgradeShop.OpenShop();
                    _openShop = false;
                }
            }
        }

        if (!_spawnIsOn) return;
        
        if (_currentWaveMonsterCounter < _maxTotalMonsterAmount - 1)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
                SpawnLightNingBolt();
        }
        else if (_monsterCounter <= 0 && _currentWaveMonsterCounter >= _maxTotalMonsterAmount)
        {
            _spawnIsOn = false;
            SetNewWave();
        }
    }


    private void SpawnLightNingBolt()
    {
        var lightning = Instantiate(this.lightningStrike,
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
        var monsterToSpawn = _mostWantedEnemy;

        if (_monsterIndex == 0)
        {
            if (_bigMonsterCounter < _bigMonsterMaxAmount)
            {
                monsterToSpawn = monstersBig;
                _bigMonsterCounter += 1;
            }
            else
                _monsterIndex += 1;
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
        {
            if (_smallMonsterCounter < _smallMonsterMaxAmount)
            {
                monsterToSpawn = monstersSmall;
                _smallMonsterCounter += 1;
            }
        }


        if (monsterToSpawn)
        {
            var monster = Instantiate(monsterToSpawn.gameObject, spawnerDots[_dotIndexMonster].position,
                Quaternion.identity);
            monster.transform.SetParent(gameObject.transform);
        }


        _monsterIndex = (_monsterIndex + 1) % monsterKindsAmount;
        _dotIndexMonster = (_dotIndexMonster + 1) % spawnerDots.Length;

        _currentWaveMonsterCounter += 1;
        _monsterCounter += 1;
    }

    public void SetNewWave()
    {
        _currentWaveMonsterCounter = 0;
        _waveIndex += 1;
        if (_waveIndex >= waves.Length)
            return;
        var curWave = waves[_waveIndex];


        _maxTotalMonsterAmount = curWave.monsterAmount;
        _bigPercentage = curWave.bigPercentage;
        _middlePercentage = curWave.middlePercentage;
        _smallPercentage = curWave.smallPercentage;
        maxTimeToSpawn = curWave.timeToSpawnStep;

        GameManager.Shared.roundMonsterTotalAmount = _maxTotalMonsterAmount;
        GameManager.Shared.roundNumber += 1;
        GameManager.Shared.roundMonsterKillCounter = 0;
        
        _bigMonsterMaxAmount = (int) Math.Floor(_bigPercentage * _maxTotalMonsterAmount);
        _midMonsterMaxAmount = (int) Math.Floor(_middlePercentage * _maxTotalMonsterAmount);
        _smallMonsterMaxAmount = (int) Math.Floor(_smallPercentage * _maxTotalMonsterAmount);
        CalculateMostWanted();
        if (!_gameStarted)
        {
            StartBlockSpawn(true);
            _gameStarted = true;
        }
        else
        {
            _openShop = true;
            _openShopTimer = timeToOpenTheShopDeley;
        }
    }

    public void StartBlockSpawn(bool mood)
    {
        _spawnIsOn = mood;
    }
}