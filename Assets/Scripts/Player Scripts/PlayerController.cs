using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool _canCreateIceDash = true;
    public static PlayerController _PlayerController;
    private GameControls _gameControls;
    private Vector2 move;

    public enum PlayerState
    {
        Idle,
        Move,
        Combat,
        Falling,
        Dead
    }

    private enum AttackStatus
    {
        First,
        Second,
        Special
    }

    #region Inspector Control

    [Header("Movement Control")] [SerializeField]
    private float movementSpeed = 5;

    [SerializeField] private GameObject hitBox;
    [SerializeField] private GameObject reflection;
    [SerializeField] private bool canMoveWhileAttacking;
    [SerializeField] public float stunDuration;
    [SerializeField] private float knockBackDistance;
    [SerializeField] private Animator slashAnimator;
    [SerializeField] private bool playerCantFall;

    [Header("Slippery Floor")] [SerializeField]
    private bool slipperyFloor = true;

    [SerializeField] private float friction = 1.5f;
    [SerializeField] private float speedScalerForSlipperyFloor = 1.5f;


    [Header("Dash Settings")] [SerializeField]
    private LayerMask dashLayerMask;

    [SerializeField] private float dashDistance = 50;
    [SerializeField] private float attackDashDistance = 50;
    [SerializeField] private float dashEffectDurationTime;

    [Header("Ice Dash")] [SerializeField] private GameObject iceSpike;
    [SerializeField] [Range(0, 0.5f)] private float spikeSeparation;
    [SerializeField] [Range(1, 5)] private int spikeScaler;

    #endregion

    #region Private Fields

    private Rigidbody2D _rb;
    private TrailRenderer _dashEffect;
    private PlayerStats _playerStats;
    [NonSerialized] public bool IsAttacking;
    [NonSerialized] public Animator Animator;
    private GameManager.WorldState _currentWorldState;
    private AttackStatus _attackStatus = AttackStatus.First;
    public PlayerState _playerState = PlayerState.Idle;
    private Vector2 _moveDirection = Vector2.zero;
    private Vector2 _idleDirection = Vector2.down;
    private Vector2 _knockBackDirection = Vector2.zero;
    [NonSerialized] public bool _dashStatus;
    private bool _attackDash;
    [NonSerialized] public bool KnockBackStatus;
    private List<GameObject> _monstersInRange;
    private bool _isInTopHalf;
    public bool isStunned;
    [NonSerialized] public bool IsPlayerDead;

    #endregion

    #region Animator Labels

    private static readonly int State = Animator.StringToHash("State");
    private static readonly int WalkHorizontal = Animator.StringToHash("WalkHorizontal");
    private static readonly int WalkVertical = Animator.StringToHash("WalkVertical");
    private static readonly int IdleHorizontal = Animator.StringToHash("IdleHorizontal");
    private static readonly int IdleVertical = Animator.StringToHash("IdleVertical");
    private static readonly int AttackHorizontal = Animator.StringToHash("AttackHorizontal");
    private static readonly int AttackVertical = Animator.StringToHash("AttackVertical");

    public PlayerController(List<GameObject> monstersInRange)
    {
        _monstersInRange = monstersInRange;
    }

    #endregion


    [Header("Screen Shake Settings")] [SerializeField]
    private float screenShakeIntensity = 1f;

    [SerializeField] private float screenShakeTime = .1f;

    private void Awake()
    {
        if (_PlayerController == null)
            _PlayerController = this;
        _gameControls = new GameControls();
        InitializeControls();
    }

    private void InitializeControls()
    {
        _gameControls.GameControl.Attack.performed +=  AttackInput;
    }
    
    
    private void OnEnable()
    {
        _gameControls.GameControl.Enable();
    }

    private void OnDisable()
    {
        _gameControls.GameControl.Disable();
    }

    private void AttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (GameManager.Shared.CurrentState)
            {
                case GameManager.WorldState.Fire:
                    FireStateAttack();
                    break;

                case GameManager.WorldState.Ice:
                    IceStateAttack();
                    break;
            } 
        }
        
    }

    

    private void IceStateAttack()
    {
        if (_playerState != PlayerState.Falling)
        {
            _dashStatus = true;
            var hourGlass = FindObjectOfType<HourGlass>();
            if (hourGlass != null)
            {
                hourGlass.PushHourGlass();
            }

            StartCoroutine(ActivateDashTrail());
        }
    }


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        _dashEffect = GetComponent<TrailRenderer>();
        _playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (transform.position.y < -50)
        {
            _rb.gravityScale = 0f;
            _rb.velocity = Vector2.zero;
        }

        if (IsPlayerDead)
            return;

        if (transform.position.y < -20)
        {
            ResetPlayerFall();
            return;
        }

        SetWorldState();
        SetPlayerState();
        SetMovementAndIdleDirection();
        SetHitBoxRotation();
        // if (_currentWorldState == GameManager.WorldState.Fire)
        //     FireStateAttack();
        PlayAnimation();

        // if (Input.GetButtonDown("Attack"))
        // {
        //     if (_playerState != PlayerState.Falling
        //         && _currentWorldState == GameManager.WorldState.Ice)
        //     {
        //         _dashStatus = true;
        //         var hourGlass = FindObjectOfType<HourGlass>();
        //         if (hourGlass != null)
        //         {
        //             hourGlass.PushHourGlass();
        //         }
        //
        //         StartCoroutine(ActivateDashTrail());
        //     }
        // }
    }

    private void SetWorldState()
    {
        _currentWorldState = GameManager.Shared.CurrentState;

        switch (_currentWorldState)
        {
            case GameManager.WorldState.None:
                break;

            case GameManager.WorldState.Fire:
                slipperyFloor = false;
                canMoveWhileAttacking = false;
                break;

            case GameManager.WorldState.Ice:
                slipperyFloor = true;
                canMoveWhileAttacking = true;
                break;

            default:
                Debug.Log("ArenaScript - Couldn't switch world");
                break;
        }
    }

    private void SetPlayerState()
    {
        if (_playerState == PlayerState.Combat || _playerState == PlayerState.Falling ||
            _playerState == PlayerState.Dead)
            return;
        _attackStatus = AttackStatus.First;
        if (_moveDirection.sqrMagnitude > 0.01f)
            _playerState = PlayerState.Move;
        else
            _playerState = PlayerState.Idle;
    }

    private void FireStateAttack()
    {
        // if (!Input.GetButtonDown("Attack") || IsAttacking) return;
        if (IsAttacking) return;

        
        IsAttacking = true; // affects the animations
        var hourGlass = FindObjectOfType<HourGlass>();
        if (hourGlass != null)
            hourGlass.IsAttacking();

        _playerState = PlayerState.Combat; // changes player state
        _attackDash = true; // boolean used for attack dash
        //TODO: play attack sound here
        if (_monstersInRange != null)
            foreach (var monster in _monstersInRange)
            {
                //TODO: choose player damage
                var monsterController = monster.GetComponent<Enemy>();
                if (monsterController != null)
                    monsterController.DamageEnemy(CalculateDamage());
                if (_playerStats.burnDamage)
                {
                    var fireParticle = monster.GetComponent<FireParticleEffect>();
                    if (fireParticle)
                        fireParticle.CloseAndOpenBurningAffect(true);
                }
            }

        slashAnimator.SetInteger("AttackStage", (int) _attackStatus);
        slashAnimator.SetTrigger("Attack");
        _attackStatus++;
    }

    private int CalculateDamage()
    {
        return _attackStatus switch
        {
            AttackStatus.First => 20,
            AttackStatus.Second => 30,
            AttackStatus.Special => 40,
            _ => 0
        };
    }

    private IEnumerator ActivateDashTrail()
    {
        _dashEffect.emitting = true;
        yield return new WaitForSeconds(dashEffectDurationTime);
        _dashEffect.emitting = false;
    }

    private void SetMovementAndIdleDirection()
    {
        // if (Input.anyKey) // (!Input.GetButton("Attack") && Input.anyKey) - enable if you dont want player attack to stop movement
        if (move != Vector2.zero) // (!Input.GetButton("Attack") && Input.anyKey) - enable if you dont want player attack to stop movement
        {
            // _moveDirection.x = Input.GetAxis("Horizontal");
            // _moveDirection.y = Input.GetAxis("Vertical");
            _moveDirection.x = move.x;
            _moveDirection.y = move.y;

            if (slipperyFloor)
                _moveDirection *= speedScalerForSlipperyFloor;
        }

        else if (slipperyFloor) // slippery floor movement fade additions
        {
            var fade = Time.deltaTime / friction;
            _moveDirection.x = _moveDirection.x == 0 ? 0 :
                _moveDirection.x > 0 ? _moveDirection.x - fade : _moveDirection.x + fade;
            _moveDirection.y = _moveDirection.y == 0 ? 0 :
                _moveDirection.y > 0 ? _moveDirection.y - fade : _moveDirection.y + fade;
            if (_moveDirection.magnitude < 0.1f)
                _moveDirection = Vector2.zero;
        }
        else // non-slippery floor movement fade additions
        {
            // _moveDirection.x = Input.GetAxis("Horizontal");
            // _moveDirection.y = Input.GetAxis("Vertical");
            _moveDirection.x = move.x;
            _moveDirection.y = move.y;
        }


        if (_moveDirection != Vector2.zero)
            _idleDirection = _moveDirection;

        _idleDirection.x = _idleDirection.x == 0 ? 0 : _idleDirection.x > 0 ? 1 : -1;
        _idleDirection.y = _idleDirection.y == 0 ? 0 : _idleDirection.y > 0 ? 1 : -1;
    }

    private void SetHitBoxRotation()
    {
        var hitBoxAngle = Vector2.SignedAngle(Vector2.down, _idleDirection);
        var hitBoxEulerAngles = hitBox.transform.eulerAngles;
        hitBoxEulerAngles.z = hitBoxAngle;
        hitBox.transform.eulerAngles = hitBoxEulerAngles;
    }

    private void PlayAnimation()
    {
        Animator.SetInteger(State, (int) _playerState);
        Animator.SetFloat(WalkHorizontal, _moveDirection.x);
        Animator.SetFloat(WalkVertical, _moveDirection.y);
        Animator.SetFloat(IdleHorizontal, _idleDirection.x);
        Animator.SetFloat(IdleVertical, _idleDirection.y);
        Animator.SetFloat(AttackHorizontal, _idleDirection.x);
        Animator.SetFloat(AttackVertical, _idleDirection.y);
    }

    private void FixedUpdate()
    {

        move = _gameControls.GameControl.Movement.ReadValue<Vector2>();
        
        

        if (IsPlayerDead)
            return;

        ApplyPowerUps();

        _rb.MovePosition(_rb.position + _moveDirection * movementSpeed * Time.fixedDeltaTime);
        
        // if (slipperyFloor || _playerState != PlayerState.Combat)
        //     _rb.MovePosition(_rb.position + _moveDirection * movementSpeed * Time.fixedDeltaTime);
        // else if (canMoveWhileAttacking)
        //     _rb.MovePosition(_rb.position + _moveDirection * movementSpeed * Time.fixedDeltaTime);


        // dash from attack occurs only on non-slippery floors
        if (!slipperyFloor && _attackDash)
        {
            var dashPosition = _rb.position + _idleDirection * attackDashDistance;
            var hit = Physics2D.Raycast(transform.position, _idleDirection,
                attackDashDistance, dashLayerMask);
            if (hit.collider != null)
                dashPosition = hit.point;

            _rb.MovePosition(dashPosition);
            _attackDash = false;
        }

        // knock back from attack
        if (KnockBackStatus)
        {
            var dashPosition = _rb.position + _knockBackDirection * knockBackDistance * -1;
            var hit = Physics2D.Raycast(transform.position, _knockBackDirection,
                attackDashDistance, dashLayerMask);
            if (hit.collider != null)
                dashPosition = hit.point;

            _rb.MovePosition(dashPosition);
            KnockBackStatus = false;
        }

        if (_dashStatus)
        {
            var dashPosition = _rb.position + _idleDirection * dashDistance;
            var hit = Physics2D.Raycast(transform.position, _idleDirection,
                dashDistance, dashLayerMask);
            if (hit.collider != null)
                dashPosition = hit.point;

            _rb.MovePosition(dashPosition);
            _dashStatus = false;
        }
    }

    public void SetPlayerState(PlayerState status)
    {
        _playerState = status;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Arena Collider") && !playerCantFall)
            SetPlayerFall();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Top"))
            _isInTopHalf = true;

        if (other.gameObject.CompareTag("Bottom"))
            _isInTopHalf = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var monList = _monstersInRange ?? new List<GameObject>();
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject;
            if (!monList.Contains(enemy))
                monList.Add(enemy);
        }

        _monstersInRange = monList;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var monList = _monstersInRange ?? new List<GameObject>();
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject;
            if (monList.Contains(enemy))
                monList.Remove(enemy);
        }

        _monstersInRange = monList;
    }

    private void SetPlayerFall()
    {
        _playerState = PlayerState.Falling;
        _rb.gravityScale = 50;
        GetComponent<Collider2D>().enabled = false;
        Animator.SetInteger(State, (int) _playerState);
        if (_isInTopHalf)
            GetComponent<SpriteRenderer>().sortingLayerName = "Default";
    }

    private void ResetPlayerFall()
    {
        GetComponent<PlayerHealth>().ApplyFallDamage();

        if (IsPlayerDead)
            return;

        _playerState = PlayerState.Idle;
        _rb.gravityScale = 0;
        GetComponent<Collider2D>().enabled = true;
        transform.position = Vector3.zero;
        GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        reflection.SetActive(true);
        PlayerGotHit(Vector3.zero);
        if (FindObjectOfType<CinemaMachineShake>())
        {
            CinemaMachineShake.Instance.ShakeCamera(0, 0.5f);
        }
    }

    public void PlayerGotHit(Vector3 pos)
    {
        var direction = (pos - transform.position).normalized;
        _knockBackDirection = new Vector2(direction.x, direction.z);
        GetComponent<HitBreak>().HitBreakAction();
    }

    public PlayerState GetPlayerState()
    {
        return _playerState;
    }

    public Vector2 GetPlayerIdleDirection()
    {
        return _idleDirection;
    }

    private void ApplyPowerUps()
    {
        playerCantFall = _playerStats.cantFall;
        hitBox.transform.localScale = new Vector3(1, -1, 1) * _playerStats.attackRange;
        IceDash();
    }

    private void IceDash()
    {
        if (!_playerStats.iceDash || !_dashEffect.emitting)
            return;
        for (var i = 0; i < spikeScaler; i++)
        {
            var spikePos = transform.position;
            spikePos.x += Random.Range(-spikeSeparation, spikeSeparation);
            spikePos.y += Random.Range(-spikeSeparation, spikeSeparation);
            
            
            
            
            Instantiate(iceSpike, spikePos, Quaternion.identity);
        }
    }

    public void KillPlayer()
    {
        IsPlayerDead = true;
        _moveDirection = Vector2.zero;
        if (_playerState != PlayerState.Falling)
            _playerState = PlayerState.Dead;
        Animator.SetInteger(State, (int) _playerState);
    }
}