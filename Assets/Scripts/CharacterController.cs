using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float dashDistance = 50;
    [SerializeField] private LayerMask dashLayerMask;
    
    private Rigidbody2D _rb;
    private Vector3 _moveDirection = Vector3.zero;
    private bool _dashStatus;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
            _dashStatus = true;
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Camera.main == null)
                return;
            
            var mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            var attackDirection = (mousePosition - transform.position).normalized;
            // play attack animation 
            // set move direction to zero

        }
    }
    
    

    private void FixedUpdate()
    {
        _rb.velocity = _moveDirection * movementSpeed;

        if (_dashStatus)
        {
            var dashPosition = transform.position + _moveDirection * dashDistance;
            var hit = Physics2D.Raycast(transform.position, _moveDirection,
                dashDistance, dashLayerMask);
            if (hit.collider != null)
                dashPosition = hit.point;

            _rb.MovePosition(dashPosition);
            _dashStatus = false;
        }
    }
}
