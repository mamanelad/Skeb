using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator playerAnimator;
    [SerializeField] private float movementSpeed;
    private Vector2 _movement;
    private Rigidbody2D _rb;

    //Dash
    private float doubleTapTime;
    private KeyCode _lastKeyCode;
    
    [SerializeField] private float dashSpeed = 5f;
    private float _dashCount;
    [SerializeField] private float statDashCount;
    private Sides _side;

    [SerializeField] private float dashCollDown = .5f;
    private float dashCollDownTimer;

    private enum Sides
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _dashCount = statDashCount;
    }

    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        
        playerAnimator.SetFloat("Horizontal", _movement.x);
        playerAnimator.SetFloat("Vertical", _movement.y);
        playerAnimator.SetFloat("speed", _movement.sqrMagnitude);
        


        dashCollDownTimer -= Time.deltaTime;
        //Dash
        if (_side == Sides.NONE && dashCollDownTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (doubleTapTime > Time.time && _lastKeyCode == KeyCode.A)
                    _side = Sides.LEFT;

                else
                    doubleTapTime = Time.time + 0.5f;

                _lastKeyCode = KeyCode.A;
            }
            
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (doubleTapTime > Time.time && _lastKeyCode == KeyCode.D)
                    _side = Sides.RIGHT;

                else
                    doubleTapTime = Time.time + 0.5f;

                _lastKeyCode = KeyCode.D;
            }
            
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (doubleTapTime > Time.time && _lastKeyCode == KeyCode.W)
                    _side = Sides.UP;

                else
                    doubleTapTime = Time.time + 0.5f;

                _lastKeyCode = KeyCode.W;
            }
            
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (doubleTapTime > Time.time && _lastKeyCode == KeyCode.D)
                    _side = Sides.DOWN;

                else
                    doubleTapTime = Time.time + 0.5f;

                _lastKeyCode = KeyCode.D;
            }
        }

        else
        {
            if (_dashCount <= 0)
            {
                _side = Sides.NONE;
                _dashCount = statDashCount;
                _rb.velocity = Vector2.zero;
            }
            else
            {
                _dashCount -= Time.deltaTime;
                if (_side == Sides.LEFT)
                {
                    _rb.velocity = Vector2.left * dashSpeed;
                    dashCollDownTimer = dashCollDown;
                }

                if (_side == Sides.RIGHT)
                {
                    _rb.velocity = Vector2.right * dashSpeed;
                    dashCollDownTimer = dashCollDown;
                } 
                
                if (_side == Sides.UP)
                {
                    _rb.velocity = Vector2.up * dashSpeed;
                    dashCollDownTimer = dashCollDown;
                } 
                
                if (_side == Sides.DOWN)
                {
                    _rb.velocity = Vector2.down * dashSpeed;
                    dashCollDownTimer = dashCollDown;
                } 
            }
            
        }
    }

    private void FixedUpdate()
    {
        //No dash
        if (_side == 0)
            _rb.MovePosition(_rb.position + _movement * movementSpeed * Time.fixedDeltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            other.GetComponent<DoorScript>().OpenDoor();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            print("Can see");
        }
    }

    
}