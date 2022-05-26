using System;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnerDots : MonoBehaviour
{
    #region Private Fields

    private int _dotIndexBolt; //Position for the bolt
    private int _dotIndexMonster; //Position for the monster
    private int _monsterIndex; //Which monster to initialize
    private float _timer;
    private int _monsterCounter; //Count how many monsters are alive.


    private Enemy mostWantedEnemy;
    private int _bigMonsterMaxAmount;
    private int _midMonsterMaxAmount;
    private int _smallMonsterMaxAmount;
    private int _bigMonsterCounter;
    private int _midMonsterCounter;
    private int _smallMonsterCounter;

    #endregion

    #region Inspector Control

    [SerializeField] private GameObject lightningStrike;
    // [SerializeField] private GameObject[] monsters;

    [Header("Position Setting")] [SerializeField]
    private Transform[] spawnerDots; //Positions for the monster initialization.


    [Header("Monster Game Objects")] [SerializeField]
    private Enemy monstersBig;

    [SerializeField] private Enemy monstersMiddle;
    [SerializeField] private Enemy monstersSmall;

    [Header("Monster Amount Setting")] [Range(0, 1)] [SerializeField]
    private float bigPercentage = .33f;

    [Range(0, 1)] [SerializeField] private float middlePercentage = .33f;
    [Range(0, 1)] [SerializeField] private float smallPercentage = .33f;
    [SerializeField] private int multMaxMonsterAmount = 3;
    [SerializeField] private int maxTotalMonsterAmount = 3; //Max amount of monster that can be alive.
    [SerializeField] private int monsterKindsAmount = 3;

    [Header("Timing Setting")] [SerializeField]
    private float maxTimeToSpawn = 4; //Amount of time between enemies initialization.

    #endregion

    private void Start()
    {
        _timer = maxTimeToSpawn;
        SetNewWave();
        CalculateMostWanted();
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

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
            SpawnLightNingBolt();
    }


    private void SpawnLightNingBolt()
    {
        if (_monsterCounter >= maxTotalMonsterAmount) return;
        _monsterCounter += 1;
        var lightning = Instantiate(this.lightningStrike,
            spawnerDots[_dotIndexBolt].position, Quaternion.identity);
        _dotIndexBolt = (_dotIndexBolt + 1) % spawnerDots.Length;
        lightning.transform.SetParent(gameObject.transform);
        _timer = maxTimeToSpawn;
    }

    public void DecreaseMonster()
    {
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
    }

    public void SetNewWave()
    {
        maxTotalMonsterAmount *= multMaxMonsterAmount;
        _bigMonsterMaxAmount = (int) Math.Floor(bigPercentage * maxTotalMonsterAmount);
        _midMonsterMaxAmount = (int) Math.Floor(middlePercentage * maxTotalMonsterAmount);
        _smallMonsterMaxAmount = (int) Math.Floor(smallPercentage * maxTotalMonsterAmount);
    }
}