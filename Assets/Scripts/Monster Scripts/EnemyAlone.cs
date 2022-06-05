using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAlone : MonoBehaviour
{
    #region Private Fields

    private PlayerController _playerController;
    private FireParticleEffect _iceParticleEffect;
    private EnemyAI _enemyAI;
    private GameObject _energyBallFather;
    private Enemy _enemyTogetherFather;
    private GameObject _player;
    private Animator _animator;
    private float _timerBetweenAttacks;
    private float _afterDashWaitTimer;
    private bool _canAttack = true;
    private bool _canAttackAfterCollisionIce;
    private bool _isAttacking;
    private bool _isDead;
    
    #endregion

    #region Animator Labels

    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");

    #endregion

    #region Inspector Control

    [Header("Attack Settings")] [SerializeField]
    private GameObject energyBall;

    [FormerlySerializedAs("AfterDashWaitTime")] [SerializeField] private float afterDashWaitTime = .3f;
    [SerializeField] private float attackRangeCheck = 0.5f;
    [SerializeField] private float attackRangeHit = 1f;
    [SerializeField] private float attackDamage = 50f;
    [SerializeField] private float timeBetweenAttacks = 0.5f;

    #endregion

    private void Start()
    {
        _canAttackAfterCollisionIce = true;
        _playerController = FindObjectOfType<PlayerController>();

        if (FindObjectOfType<EnemySpawnerDots>())
            _energyBallFather = FindObjectOfType<EnemySpawnerDots>().gameObject;

        _iceParticleEffect = GetComponentInChildren<FireParticleEffect>();
        _enemyAI = GetComponentInParent<EnemyAI>();
        _enemyTogetherFather = GetComponentInParent<Enemy>();
        _player = FindObjectOfType<PlayerController>().gameObject;
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_playerController.IsPlayerDead) return;
        //See if the player close enough for attack.
        if (!_canAttackAfterCollisionIce)
        {
            _afterDashWaitTimer -= Time.deltaTime;
            if (_afterDashWaitTimer < 0)
                _canAttackAfterCollisionIce = true;
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
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.Shared.CurrentState == GameManager.WorldState.Ice)
        {
            _iceParticleEffect.isOn = true;
            _canAttackAfterCollisionIce = false;
            _afterDashWaitTimer = afterDashWaitTime;
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
        if (!_canAttackAfterCollisionIce) return;
        SoundEnemy("Attack");
        _animator.SetTrigger(Attack);
    }

    private void SoundEnemy(string mode)
    {
        switch (_enemyTogetherFather.enemyKind)
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
        ball.GetComponent<EnergyBall>().attackDamage = attackDamage;
        if (_energyBallFather)
            ball.transform.SetParent(_energyBallFather.transform);
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