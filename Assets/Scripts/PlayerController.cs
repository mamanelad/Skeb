using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController _PlayerController;
    public enum PlayerState { Idle, Move, Combat }
    
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private bool canDash;
    [SerializeField] private float dashDistance = 50;
    [SerializeField] private LayerMask dashLayerMask;
    
    private Rigidbody2D _rb;
    [NonSerialized] public Animator Animator;
    private PlayerState _playerState = PlayerState.Idle;
    private Vector2 _moveDirection = Vector2.zero;
    private Vector2 _idleDirection = Vector2.down;
    private bool _dashStatus;
    [NonSerialized] public bool IsAttacking;
    private static readonly int State = Animator.StringToHash("State");
    private static readonly int WalkHorizontal = Animator.StringToHash("WalkHorizontal");
    private static readonly int WalkVertical = Animator.StringToHash("WalkVertical");
    private static readonly int IdleHorizontal = Animator.StringToHash("IdleHorizontal");
    private static readonly int IdleVertical = Animator.StringToHash("IdleVertical");
    private static readonly int AttackHorizontal = Animator.StringToHash("AttackHorizontal");
    private static readonly int AttackVertical = Animator.StringToHash("AttackVertical");

    private void Awake()
    {
        if (_PlayerController == null)
            _PlayerController = this;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        SetPlayerState();
        SetMovementAndIdleDirection();
        Attack();
        PlayAnimation();
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            _dashStatus = true;
        
    }

    private void SetPlayerState()
    {
        if (_playerState == PlayerState.Combat)
            return;
        if (_moveDirection.sqrMagnitude > 0.01f)
            _playerState = PlayerState.Move;
        else
            _playerState = PlayerState.Idle;
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !IsAttacking)
        {
            IsAttacking = true;
            _playerState = PlayerState.Combat;
        }
        
    }

    private void SetMovementAndIdleDirection()
    {
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.y = Input.GetAxis("Vertical");

        if (_moveDirection != Vector2.zero)
            _idleDirection = _moveDirection;
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
        //_rb.velocity = _moveDirection * movementSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + _moveDirection * movementSpeed * Time.fixedDeltaTime);

        if (_dashStatus)
        {
            var dashPosition = _rb.position + _moveDirection * dashDistance;
            var hit = Physics2D.Raycast(transform.position, _moveDirection,
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
}
