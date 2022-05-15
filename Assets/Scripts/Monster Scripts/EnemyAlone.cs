using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlone : MonoBehaviour
{
    // Start is called before the first frame update

    private Enemy enemyFather;
    private Animator _animator;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float attackDamage = 50f;

    private GameObject _player;
    private bool isAttacking = false;
    private bool isDead = false;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        enemyFather = GetComponentInParent<Enemy>();

    }
    

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        if (enemyFather.GetHealth() <= 0 && !isDead)
        {
            isDead = true;
            _animator.SetTrigger("Dead");
        }
    }
    
    
    private void DetectPlayer()
    {
        var dist = Vector3.Distance(_player.transform.position, transform.position);
        if (dist <= attackRange && !isAttacking)
        {
            AttackPlayer();
            isAttacking = true;
        }
        
    }
    
    private void AttackPlayer()
    {
        _animator.SetTrigger("Attack");
        
    }
    
    public void DamagePlayer()
    {
        _player.GetComponent<PlayerHealth>().UpdateHealth(-attackDamage);
        isAttacking = false;
    }
}
