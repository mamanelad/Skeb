using System;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] private float _currHealth;
    [SerializeField] private float startHealth = 100;

    private EnemySpawnerDots _enemySpawnerDots;

    [SerializeField] private GameObject fireMonster;
    [SerializeField] private GameObject iceMonster;
    private Animator fireMonsterAnimator;
    private Animator IceMonsterAnimator;

    private GameObject _player;
    private GameManager.WorldState _state;


    
    

    

    private void Start()
    {
        fireMonsterAnimator = fireMonster.GetComponent<Animator>();
        IceMonsterAnimator = iceMonster.GetComponent<Animator>();
        _currHealth = startHealth;
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemySpawnerDots = FindObjectOfType<EnemySpawnerDots>();
        ChangeState();
    }


    private void Update()
    {
        if (_state != GameManager.Shared.CurrentState)
        {
            ChangeState();
        }

        if (_player.transform.position.x < transform.position.x )
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        else if (_player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        
    }




    public float GetHealth()
    {
        return _currHealth;
    }
    public void DamageEnemy(int damage)
    {
        
        _currHealth -= damage;
        switch (_state)
        {
            case GameManager.WorldState.Fire:
                fireMonsterAnimator.SetTrigger("Demage");
                break;

            case GameManager.WorldState.Ice:
                IceMonsterAnimator.SetTrigger("Demage");
                break;
        }
        
        if (_currHealth <= 0)
        {
            KillEnemy();
        }
        
        
        
        
    }


    private void KillEnemy()
    {
        GetComponent<EnemyAI>().enabled = false;
        _enemySpawnerDots.DecreaseMonster();
    }

    

    private void AnotherEnemyInteraction(Collider2D other)
    {
    }

    private void ChangeState()
    {
        _state = GameManager.Shared.CurrentState;
        switch (_state)
        {
            case GameManager.WorldState.Fire:
                iceMonster.SetActive(false);
                fireMonster.SetActive(true);
                break;

            case GameManager.WorldState.Ice:
                iceMonster.SetActive(true);
                fireMonster.SetActive(false);
                break;
        }
    }

    
}