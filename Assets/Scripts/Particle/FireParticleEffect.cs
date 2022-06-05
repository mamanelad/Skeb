using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FireParticleEffect : MonoBehaviour
{
    #region Private Fields
    
    private float _timerBurning;
    private float _damageEnemyTimer;
    private float _timeSinceLastSpawn;
    #endregion
    
    #region Public Fields

    public bool damageEnemy;

    #endregion

    #region Inspector Control
    
    [FormerlySerializedAs("ParticlePrefab")] [SerializeField] public GameObject particlePrefab;
    
    [SerializeField] private float timeToBurn = 1.5f;
    [FormerlySerializedAs("Rate")] [SerializeField] private float rate = 500; // per second
    [SerializeField] public bool isOn;
    [SerializeField] private float damageEnemyStep = 0.2f;
    [SerializeField] private int fireDamage = 5;
    
    #endregion


    private void Start()
    {
        _timerBurning = timeToBurn;
    }

    private void Update()
    {
        if (!isOn) return;

        _timerBurning -= Time.deltaTime;
        if (_timerBurning <= 0)
            CloseAndOpenBurningAffect(false);

        _damageEnemyTimer -= Time.deltaTime;
        if (_damageEnemyTimer <= 0)
            FireDamage();

        _timeSinceLastSpawn += Time.deltaTime;

        var correctTimeBetweenSpawns = 1f / rate;

        while (_timeSinceLastSpawn > correctTimeBetweenSpawns)
        {
            // Time to spawn a particle
            SpawnFireAlongOutline();
            _timeSinceLastSpawn -= correctTimeBetweenSpawns;
        }
    }

    private void SpawnFireAlongOutline()
    {
        var col = GetComponent<PolygonCollider2D>();

        var pathIndex = Random.Range(0, col.pathCount);

        var points = col.GetPath(pathIndex);

        var pointIndex = Random.Range(0, points.Length);

        var pointA = points[pointIndex];
        var pointB = points[(pointIndex + 1) % points.Length];

        var spawnPoint = Vector2.Lerp(pointA, pointB, Random.Range(0f, 1f));

        SpawnFireAtPosition(spawnPoint + (Vector2) transform.position);
    }

    private void SpawnFireAtPosition(Vector2 position)
    {
        SimplePool.Spawn(particlePrefab, position, Quaternion.identity);
    }


    public void CloseAndOpenBurningAffect(bool mode)
    {
        isOn = mode;
        _timerBurning = timeToBurn;
        _damageEnemyTimer = damageEnemyStep;
    }

    private void FireDamage()
    {
        if (GameManager.Shared.CurrentState == GameManager.WorldState.Ice) return;
        _damageEnemyTimer = damageEnemyStep;
        damageEnemy = true;
        var enemy = GetComponentInParent<Enemy>();
        if (enemy != null)
            enemy.DamageEnemy(fireDamage);
            
        damageEnemy = false;
    }
}