using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerDots : MonoBehaviour
{
    [SerializeField] private GameObject _lightningStrike;
    [SerializeField] private GameObject[] monsters;
    private int monsterIndex = 0;


    [SerializeField] private Transform[] spawnerDots;

    private int dotIndexBolt = 0;
    private int dotIndexMonster = 0;


    [SerializeField] private float maxTimeToSpawn = 4;

    private float _timer;

    [SerializeField] private int maxMonsterAmount = 8;


    private int _monsterCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        _timer = maxTimeToSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
            SpawnLightNingBolt();
    }


    private void SpawnLightNingBolt()
    {
        if (_monsterCounter >= maxMonsterAmount) return;
        _monsterCounter += 1;
        var lightningStrike = Instantiate(_lightningStrike, spawnerDots[dotIndexBolt].position, Quaternion.identity);
        dotIndexBolt = (dotIndexBolt + 1) % spawnerDots.Length;
        lightningStrike.transform.SetParent(gameObject.transform);
        _timer = maxTimeToSpawn;
    }

    public void DecreaseMonster()
    {
        _monsterCounter -= 1;
    }

    public void CreatMonster()
    {
        var monster = Instantiate(monsters[monsterIndex], spawnerDots[dotIndexMonster].position, Quaternion.identity);
        monster.transform.SetParent(gameObject.transform);

        monsterIndex = (monsterIndex + 1) % monsters.Length;
        dotIndexMonster = (dotIndexMonster + 1) % spawnerDots.Length;
    }
}