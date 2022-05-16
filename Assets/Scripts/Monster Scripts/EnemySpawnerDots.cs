using UnityEngine;

public class EnemySpawnerDots : MonoBehaviour
{
    
    [SerializeField] private GameObject lightningStrike;
    
    [SerializeField] private GameObject[] monsters;
    private int _dotIndexBolt;
    private int _dotIndexMonster;
    
    [SerializeField] private Transform[] spawnerDots; //Positions for the monster initialization.
    private int _monsterIndex;
    
    private float _timer;
    [SerializeField] private float maxTimeToSpawn = 4; //Amount of time between enemies initialization.
    [SerializeField] private int maxMonsterAmount = 8; //Max amount of monster that can be alive.

    private int _monsterCounter; //Count how many monsters are alive.

    void Start()
    {
        _timer = maxTimeToSpawn;
    }

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
        var monster = Instantiate(monsters[_monsterIndex], spawnerDots[_dotIndexMonster].position, Quaternion.identity);
        monster.transform.SetParent(gameObject.transform);

        _monsterIndex = (_monsterIndex + 1) % monsters.Length;
        _dotIndexMonster = (_dotIndexMonster + 1) % spawnerDots.Length;
    }
}