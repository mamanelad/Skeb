using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    private Animator _animator;
    private GameObject _player;
    private bool _startLife;
    private bool _hit;
    
    [SerializeField] private float lifeBallTimer = 2f;
    [SerializeField] private float timeToDieAfterHit = 0.2f;
    [SerializeField] private float step = 1f;
    private static readonly int Die = Animator.StringToHash("Die");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_hit)
        {
            timeToDieAfterHit -= Time.deltaTime;
            if (timeToDieAfterHit <= 0 )
            {
                DestroyBall();
            }
        }
        if (_startLife)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);
            lifeBallTimer -= Time.deltaTime;
            if (lifeBallTimer <= 0)
            {
                _animator.SetTrigger(Die);
                _startLife = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _hit = true;
        }
       
    }

    public void StartLife()
    {
        _startLife = true;
    }

    public void DestroyBall()
    {
        Destroy(gameObject);
    }
}
