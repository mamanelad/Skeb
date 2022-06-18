using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private FireParticleEffect lifeEffect;
    [SerializeField] private bool inTutorial;
    [SerializeField] public float health = 0f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float fallDamage = 20;
    private float _healthScaler = 1f;
    private PlayerController _playerController;
    private PlayerStats _playerStats;
    private bool _usedSecondWindThisRound;

    [Header("Static Regeneration")] [SerializeField]
    private float regenAddition;

    [SerializeField] private float regenPerSecond;
    private float regenTimer;

    [Header("Second Wind")] [SerializeField]
    private GameObject angle;

    [SerializeField] private float secondWindHealthBuff;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerStats = GetComponent<PlayerStats>();
        health = maxHealth;
        regenTimer = regenPerSecond;
    }

    private void Update()
    {
        if (_playerController.IsPlayerDead)
            return;

        // activate increase max health buff
        if (_healthScaler != _playerStats.maxHealthScaler)
        {
            _healthScaler = _playerStats.maxHealthScaler;
            AddHealthBuff(_healthScaler);
        }

        // activate static regeneration 
        if (_playerStats.staticRegeneration && GameManager.Shared.CurrentGameState == GameManager.GameState.Arena)
            StaticRegeneration();

        if (GameManager.Shared.StageDamage)
            UpdateHealth(maxHealth / -500f, Vector3.zero);

        if (health <= 0)
            _playerController.KillPlayer();
    }

    private static float UpdatePlayerDamageByRound(float mod)
    {
        if (mod >= 0) return mod;
        var damageFactor = (-1) * GameManager.Shared.roundNumber / GameManager.Shared.diffLevel * 2.25f;
        mod += damageFactor;
        return mod;
    }

    public void UpdateHealth(float mod, Vector3 pos)
    {
        if (inTutorial || PlayerController._PlayerController.IsPlayerDead) return;
        
        if (_playerController.isStunned && mod <= 0)
            return;

        mod = UpdatePlayerDamageByRound(mod);

        if (mod > 0 && health < 100)
            lifeEffect.isOn = true;

        if (mod < 0)
            GetComponent<ScreenShakeListener>().Shake();

        mod -= (GameManager.Shared.roundNumber % 2);
        health = Mathf.Min(health + mod, maxHealth);
        health = Mathf.Max(health, 0);
        
        if (health <= 0 && _playerStats.secondWind && !_usedSecondWindThisRound)
        {
            _usedSecondWindThisRound = true;
            health = Mathf.Min(health + secondWindHealthBuff, maxHealth);
            SecondWindEffect();
        }

        UpdateHealthBar();

        if (mod < 0)
            _playerController.PlayerGotHit(pos);
    }

    private void UpdateHealthBar()
    {
        var lifeBarFillPercentage = health / maxHealth * 100;
        UIManager.Shared.SetLifeBar(lifeBarFillPercentage);
    }

    public void AddHealthBuff(float scaler)
    {
        maxHealth = 100 * scaler;
        health *= scaler;
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
        health = Mathf.Min(maxHealth, health + regenAddition);
        UpdateHealthBar();
    }

    public void ApplyFallDamage()
    {
        UpdateHealth(-fallDamage, Vector3.zero);
    }

    private void SecondWindEffect()
    {
        var angelSpawned = Instantiate(angle, transform.position, Quaternion.identity);
        GameManager.Shared.PlayerAudioManager.PlaySound(PlayerSound.SoundKindsPlayer.SecondWind);
        Destroy(angelSpawned, 1f);
    }
}