using UnityEngine;

public class EnemyAlone : MonoBehaviour
{
    #region Private Fields

    private EnemyAI _enemyAI;
    private Enemy _enemyTogetherFather;
    private GameObject _player;
    private Animator _animator;
    private bool _isAttacking;
    private bool _isDead;
    private float _timerBetweenAttacks;
    private bool _canAttack = true;

    #endregion

    #region Animator Labels

    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");

    #endregion

    #region Inspector Control

    [Header("Attack Settings")] [SerializeField]
    private GameObject energyBall;

    [SerializeField] private float attackRangeCheck = 0.5f;
    [SerializeField] private float attackRangeHit = 1f;
    [SerializeField] private float attackDamage = 50f;
    [SerializeField] private float timeBetweenAttacks = 0.5f;

    #endregion

    private void Start()
    {
        _enemyAI = GetComponentInParent<EnemyAI>();
        _enemyTogetherFather = GetComponentInParent<Enemy>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
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

            MovementLock();
        }
    }

    /**
     * See if the player is near enough for attack.
     */
    private void DetectPlayer()
    {
        if (_isAttacking) return;
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
            _player.GetComponent<PlayerHealth>().UpdateHealth(-attackDamage);
        }

        _isAttacking = false;
    }

    /**
     * For the mage.
     * Creating the energy ball.
     */
    public void DamagePlayerEnergyBall()
    {
        Instantiate(energyBall, transform.position, Quaternion.identity);
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