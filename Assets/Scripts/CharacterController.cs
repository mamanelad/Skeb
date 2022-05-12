using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public enum PlayerState { Idle, Move, Combat }
    
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private bool canDash;
    [SerializeField] private float dashDistance = 50;
    [SerializeField] private LayerMask dashLayerMask;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerState _playerState = PlayerState.Idle;
    private Vector2 _moveDirection = Vector2.zero;
    private Vector2 _idleDirection = Vector2.down;
    private bool _dashStatus;
    private static readonly int State = Animator.StringToHash("State");
    private static readonly int WalkHorizontal = Animator.StringToHash("WalkHorizontal");

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        SetPlayerState();
        SetMovementAndIdleDirection();
        PlayAnimation();
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            _dashStatus = true;
        
    }

    private void SetPlayerState()
    {
        if (_moveDirection.sqrMagnitude > 0.01f)
            _playerState = PlayerState.Move;
        else
            _playerState = PlayerState.Idle;
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
        _animator.SetInteger(State, (int) _playerState);
        _animator.SetFloat("WalkHorizontal", _moveDirection.x);
        _animator.SetFloat("WalkVertical", _moveDirection.y);
        _animator.SetFloat("IdleHorizontal", _idleDirection.x);
        _animator.SetFloat("IdleVertical", _idleDirection.y);
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
}
