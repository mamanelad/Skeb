using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Rigidbody2D _rB;

    private Vector2 _movement;

    private void Awake()
    {
        _rB = GetComponent<Rigidbody2D>();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        _rB.MovePosition(_rB.position + _movement*_moveSpeed*Time.fixedDeltaTime);
    }
    
    
}
