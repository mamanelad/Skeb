using System;
using UnityEngine;

public class EnemyAlone : MonoBehaviour
{
    #region Private Fields

    private FireParticleEffect _IceParticleEffect;
    private bool canAttackAfterCollisionIce;
    private float AfterDashWaitTimer;
    private EnemyAI _enemyAI;
    private GameObject _energyBallFather;
    private Enemy _enemyTogetherFather;
    private GameObject _player;
    private Animator _animator;
    private bool _isAttacking;
    private bool _isDead;
    private float _timerBetweenAttacks;
    private bool _canAttack = true;
    private PlayerController _playerController;

    #endregion

    #region Animator Labels

    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");

    #endregion

    #region Inspector Control

    [Header("Attack Settings")] [SerializeField]
    private GameObject energyBall;

    [SerializeField] private float AfterDashWaitTime = .3f;
    [SerializeField] private float attackRangeCheck = 0.5f;
    [SerializeField] private float attackRangeHit = 1f;
    [SerializeField] private float attackDamage = 50f;
    [SerializeField] private float timeBetweenAttacks = 0.5f;

    [Header("Screen Shake Settings")] [SerializeField]
    private float screenShakeIntensity = 5f;

    [SerializeField] private float screenShakeTime = .1f;

    #endregion

    private void Start()
    {
        canAttackAfterCollisionIce = true;
        _playerController = FindObjectOfType<PlayerController>();

        if (FindObjectOfType<EnemySpawnerDots>())
        {
            _energyBallFather = FindObjectOfType<EnemySpawnerDots>().gameObject;
        }

        _IceParticleEffect = GetComponentInChildren<FireParticleEffect>();
        _enemyAI = GetComponentInParent<EnemyAI>();
        _enemyTogetherFather = GetComponentInParent<Enemy>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_playerController.IsPlayerDead) return;
        //See if the player close enough for attack.
        if (!canAttackAfterCollisionIce)
        {
            AfterDashWaitTimer -= Time.deltaTime;
            if (AfterDashWaitTimer < 0)
            {
                canAttackAfterCollisionIce = true;
            }
        }

        if (_canAttack)
            DetectPlayer();

        else
        {
            _timerBetweenAttacks -= Time.deltaTime;
            if (_timerBetweenAttacks <= 0)
                _canAttack = true;
        }

        //Checks the enemy health from the enemy script.

        if (_enemyTogetherFather.GetHealth() <= 0 && !_isDead)
        {
            _isDead = true;
            _animator.SetTrigger(Dead);
            _animator.SetBool(Die, true);

            // MovementLock();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.Shared.CurrentState == GameManager.WorldState.Ice)
        {
            _IceParticleEffect.isOn = true;
            canAttackAfterCollisionIce = false;
            AfterDashWaitTimer = AfterDashWaitTime;
        }
    }

    /**
     * See if the player is near enough for attack.
     */
    private void DetectPlayer()
    {
        if (_isAttacking || !_player) return;

        var dist = Vector3.Distance(_player.transform.position, transform.position);
        if (dist <= attackRangeCheck)
        {
            AttackPlayer();
            _isAttacking = true;
            _canAttack = false;
            _timerBetweenAttacks = timeBetweenAttacks;
        }
    }

    private void AttackPlayer()
    {
        if (!canAttackAfterCollisionIce) return;
        SoundEnemy("Attack");
        _animator.SetTrigger(Attack);
    }

    private void SoundEnemy(string mode)
    {
        switch (_enemyTogetherFather._enemyKind)
        {
            case Enemy.EnemyKind.Big:
                switch (GameManager.Shared.CurrentState)
                {
                    case GameManager.WorldState.Fire:
                        switch (mode)
                        {
                            case "Attack":
                                GameManager.Shared._audioManager.PlaySound("BigMonsterAttackFire");
                                break;
                        }
                        break;
                    case GameManager.WorldState.Ice:
                        switch (mode)
                        {
                            case "Attack":
                                GameManager.Shared._audioManager.PlaySound("BigMonsterAttackIce");
                                break;
                        }
                    break;
                        
                }
                break;
        }
    }

    /**
     * Onchange for damaging the player.
     * Called from the end of the enemies attack animations.
     * If the player distance from the monster is equal or less
     * than attackRangeHit the player will get hit. 
     */
    public void DamagePlayer()
    {
        var dist = Vector3.Distance(_player.transform.position, transform.position);
        if (dist <= attackRangeHit)
        {
            if (FindObjectOfType<CinemaMachineShake>())
            {
                CinemaMachineShake.Instance.ShakeCamera(screenShakeIntensity, screenShakeTime);
            }

            _player.GetComponent<PlayerHealth>().UpdateHealth(-attackDamage, transform.position);
        }

        _isAttacking = false;
    }

    /**
     * For the mage.
     * Creating the energy ball.
     */
    public void DamagePlayerEnergyBall()
    {
        var ball = Instantiate(energyBall, transform.position, Quaternion.identity);
        ball.GetComponent<EnergyBall>()._attackDamage = attackDamage;
        if (_energyBallFather)
        {
            ball.transform.SetParent(_energyBallFather.transform);
        }

        _isAttacking = false;
    }

    /**
     * Call this function to lock and unlock the enemy movement.
     * The function is called from the start of the enemy damage animation.
     * In contact with the enemyAI script.
     */
    public void MovementUnlock()
    {
        _enemyAI.lockMovement = false;
    }

    /**
     * Call this function to lock and lock the enemy movement.
     * The function is called from the end of the enemy damage animation.
     * In contact with the enemyAI script.
     */
    public void MovementLock()
    {
        _enemyAI.lockMovement = true;
    }
}