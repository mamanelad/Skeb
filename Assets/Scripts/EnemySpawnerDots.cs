using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerDots : MonoBehaviour
{
    [SerializeField] private GameObject[] monsters;
    private int monsterIndex = 0;

    [SerializeField] private Transform[] spawnerDots;
    private int dotIndex = 0;

    [SerializeField] private float maxTimeToSpawn = 4;

    private float _timer;

    [SerializeField] private int maxMonsterAmount = 8;
    [SerializeField] private GameObject target;

    

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
            SpawnMonsters();
        
    }


    private void SpawnMonsters()
    {
        if (_monsterCounter >= maxMonsterAmount) return;

        Instantiate(monsters[monsterIndex], spawnerDots[dotIndex].position, Quaternion.identity);
        monsterIndex = (dotIndex + 1) % monsters.Length;
        dotIndex = (dotIndex + 1) % spawnerDots.Length;
        _monsterCounter += 1;
        _timer = maxTimeToSpawn;
    }

    public void DecreaseMonster()
    {
        _monsterCounter -= 1;
    }
}