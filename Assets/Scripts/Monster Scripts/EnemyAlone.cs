using System;
using UnityEngine;

public class EnemyAlone : MonoBehaviour
{
    #region Private Fields

    private FireParticleEffect _fireParticleEffect;
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
    private static readonly int Damage = Animator.StringToHash("Demage");

    #endregion

    #region Inspector Control

    [Header("Attack Settings")] [SerializeField]
    private GameObject energyBall;

    [SerializeField] private float attackRangeCheck = 0.5f;
    [SerializeField] private float attackRangeHit = 1f;
    [SerializeField] private float attackDamage = 50f;
    [SerializeField] private float timeBetweenAttacks = 0.5f;

    [Header("Screen Shake Settings")] [SerializeField]
    private float screenShakeIntensity = 5f;

    [NonSerialized] public float speed;
    private Vector3 oldPosition;
    
    [SerializeField] private float screenShakeTime = .1f;

    #endregion

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();

        if (FindObjectOfType<EnemySpawnerDots>())
        {
            _energyBallFather = FindObjectOfType<EnemySpawnerDots>().gameObject;
        }

        _fireParticleEffect = GetComponentInChildren<FireParticleEffect>();
        _enemyAI = GetComponentInParent<EnemyAI>();
        _enemyTogetherFather = GetComponentInParent<Enemy>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        speed = (transform.position - oldPosition).magnitude / Time.deltaTime;
        oldPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (_playerController.IsPlayerDead) return;
        //See if the player close enough for attack.
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
        if (GameManager.Shared.CurrentState == GameManager.WorldState.Ice)
        {
            if (other.gameObject.CompareTag("Player") && _playerController.GetPlayerSpeed() >= 0.5f &&
                _playerController._dashStatus)
            {
                var dashColEffect = other.gameObject.GetComponentInChildren<FireParticleEffect>();
                if (dashColEffect != null)
                {
                    print("kaka");
                    dashColEffect.isOn = true;    
                }
                _enemyTogetherFather.GoBack(Enemy.pushKind.Player, _player.transform.position);
                _animator.SetTrigger(Damage);
                _fireParticleEffect.isOn = true;
                GameManager.Shared.PlayerAudioManager.PlaySound(PlayerSound.SoundKindsPlayer.DashHitMonster);
            }

            if (other.gameObject.CompareTag("Enemy"))
            {
                var otherEnemyAlone = GetComponent<EnemyAlone>();
                if (otherEnemyAlone != null)
                {
                    var speedEnemy = otherEnemyAlone.speed;
                    if (speedEnemy >= 1.5f)
                    {
                        _enemyTogetherFather.GoBack(Enemy.pushKind.Enemy, other.transform.position);
                        _animator.SetTrigger(Damage);
                        _fireParticleEffect.isOn = true;
                        GameManager.Shared.PlayerAudioManager.PlaySound(PlayerSound.SoundKindsPlayer.DashHitMonster);
                    }
                }
            }
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
        _animator.SetTrigger(Attack);
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

        _enemyAI.MonsterAttackSound();
        _isAttacking = false;
    }

    /**
     * For the mage.
     * Creating the energy ball.
     */
    public void DamagePlayerEnergyBall()
    {
        //TODO :: ENERGYBALL MUSIC
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