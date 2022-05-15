using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Animator _animator;
    private GameObject _player;
    private bool startLife = false;
    [SerializeField] private float lifeBallTimer = 2f;
    [SerializeField] private float step = 1f;

    private bool hit = false;
    [SerializeField] private float timeToDieAfterHit = 0.2f; 

    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            timeToDieAfterHit -= Time.deltaTime;
            if (timeToDieAfterHit <= 0 )
            {
                DestroyBall();
            }
        }
        if (startLife)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);
            lifeBallTimer -= Time.deltaTime;
            if (lifeBallTimer <= 0)
            {
                _animator.SetTrigger("Die");
                startLife = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hit = true;
        }
       
    }

    public void StartLife()
    {
        startLife = true;
    }

    public void DestroyBall()
    {
        Destroy(gameObject);
    }
}
