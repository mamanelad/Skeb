using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RegularEnemies : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float switchDirectionTime;
    [SerializeField] private float movementSpeed;
    [SerializeField] private LayerMask wallLayer; 
    private Rigidbody2D _rb;
    private float directionTimer;
    private Vector3 direction;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        directionTimer = switchDirectionTime;
    }

    private void Update()
    {
        var pos = transform.position;
        var hit = Physics2D.Raycast(pos, pos + direction.normalized * 1.5f, 1.5f ,wallLayer);
        Debug.DrawRay(pos, direction.normalized, Color.blue);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                ChooseDirection();
                return;
            }
        }

        if (directionTimer > 0)
            directionTimer -= Time.deltaTime;
        else
        {
            ChooseDirection();
            directionTimer = switchDirectionTime;
        }

    }

    private void FixedUpdate()
    {
        Vector2 tempDirection = direction;
        _rb.MovePosition(_rb.position + tempDirection * movementSpeed * Time.fixedDeltaTime);
    }

    private void ChooseDirection()
    {
        var x = Random.Range(-1f, 1f);
        var y = Random.Range(-1f, 1f);
        direction = new Vector3(x, y, 0f);
    }
}
