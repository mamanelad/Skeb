using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private MenuManager _menuManager;
    [NonSerialized] public bool playerIsDead;
    [SerializeField] private float _health = 0f;
    [SerializeField] private float maxHealth = 100f;
    private float _healthScaer = 1f;
    private PlayerController _playerController;
    private PlayerStats _playerStats;

    [Header("Monster Regeneration")] 
    [SerializeField] private float monsterDeathAddition;
    
    [Header("Static Regeneration")] 
    [SerializeField] private float regenAddition;
    [SerializeField] private float regenPerSecond;
    private float regenTimer;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerStats = GetComponent<PlayerStats>();
        _health = maxHealth;
        regenTimer = regenPerSecond;
    }

    private void Update()
    {
        if (playerIsDead)
            return;
        
        // activate increase max health buff
        if (_healthScaer != _playerStats.maxHealthScaler)
        {
            _healthScaer = _playerStats.maxHealthScaler;
            AddHealthBuff(_healthScaer);
        }
        
        // activate static regeneration 
        if (_playerStats.staticRegeneration)
            StaticRegeneration();

        if (GameManager.Shared.StageDamage)
            UpdateHealth(maxHealth / -500f, Vector3.zero);
        
        if (_health <= 0)
            KillPlayer();
        
    }

    public void UpdateHealth(float mod, Vector3 pos)
    {
        if (_playerController.isStunned)
            return;
        
        _health += mod;
        
        if (_health > maxHealth)
        {
            _health = maxHealth;
        }
        else if (_health <= 0)
        {
            _health = 0;
            KillPlayer();
        }
        
        UpdateHealthBar();
        _playerController.PlayerGotHit(pos);
    }

    private void UpdateHealthBar()
    {
        var lifeBarFillPercentage = _health / maxHealth * 100;
        UIManager.Shared.SetLifeBar(lifeBarFillPercentage);
    }

    private void KillPlayer()
    {
        playerIsDead = true;
    }

    public void AddHealthBuff(float scaler)
    {
        maxHealth = 100 * scaler;
        _health *= scaler;
        UpdateHealthBar();
    }

    private void StaticRegeneration()
    {
        if (regenTimer > 0)
        {
            regenTimer -= Time.deltaTime;
            return;
        }

        regenTimer = regenPerSecond;
        _health = Mathf.Min(maxHealth, _health + regenAddition);
        UpdateHealthBar();
    }

    public void MonsterKillRegeneration()
    {
        if (!_playerStats.monsterKillRegeneration || playerIsDead)
            return;
        
        _health = Mathf.Min(maxHealth, _health + monsterDeathAddition);
        UpdateHealthBar();
    }
    

}