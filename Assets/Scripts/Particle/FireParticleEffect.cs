using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class FireParticleEffect : MonoBehaviour
{

    [SerializeField] private float timeToBurn = 1.5f;
    private float timerBurning;
    [SerializeField] GameObject ParticlePrefab;
    [SerializeField] float Rate = 500; // per second
    [SerializeField] public bool isOn;

    [SerializeField] private int fireDamage = 5;
    [SerializeField] private float damageEnemyStep = 0.2f;
    private float damageEnemyTimer;
    public bool damageEnemy;
    
    
    float timeSinceLastSpawn = 0;

    private void Start()
    {
        timerBurning = timeToBurn;
    }

    // Update is called once per frame
    void Update () {

        if (!isOn) return;

        timerBurning -= Time.deltaTime;
        if (timerBurning <= 0)
            CloseAndOpenBurningAffect(false);

        damageEnemyTimer -= Time.deltaTime;
        if (damageEnemyTimer <= 0)
            FireDamage();
        
        timeSinceLastSpawn += Time.deltaTime;
        
        float correctTimeBetweenSpawns = 1f/Rate;

        while( timeSinceLastSpawn > correctTimeBetweenSpawns )
        {
            // Time to spawn a particle
            SpawnFireAlongOutline();
            timeSinceLastSpawn -= correctTimeBetweenSpawns;
        }

    }

    void SpawnFireAlongOutline()
    {

        PolygonCollider2D col = GetComponent<PolygonCollider2D>();

        int pathIndex = Random.Range(0, col.pathCount);

        Vector2[] points = col.GetPath(pathIndex);

        int pointIndex = Random.Range(0, points.Length);

        Vector2 pointA = points[ pointIndex ];
        Vector2 pointB = points[ (pointIndex+1) % points.Length ];

        Vector2 spawnPoint = Vector2.Lerp(pointA, pointB, Random.Range(0f, 1f) );

        SpawnFireAtPosition(spawnPoint + (Vector2)transform.position);
    }

    private void SpawnFireAtPosition(Vector2 position)
    {
        SimplePool.Spawn(ParticlePrefab, position, Quaternion.identity);

    }

    
    public void CloseAndOpenBurningAffect(bool mode)
    {
        isOn = mode;
        timerBurning = timeToBurn;
        damageEnemyTimer = damageEnemyStep;
    }

    private void FireDamage()
    {
        damageEnemyTimer = damageEnemyStep;
        damageEnemy = true;
        var enemy = FindObjectOfType<Enemy>();
        enemy.DamageEnemy(fireDamage);
        damageEnemy = false;
    }
}