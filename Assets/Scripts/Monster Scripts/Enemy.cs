using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Private Fields

    private GameObject _player;
    private Rigidbody2D _rb;
    private EnemyAI _enemyAI;
    private EnemySpawnerDots _enemySpawnerDots;
    private Animator _fireMonsterAnimator;
    private Animator _iceMonsterAnimator;
    private GameManager.WorldState _state;
    private bool _isInTopHalf;

    #endregion

    #region Inspector Control

    [Header("Push back Settings")] [SerializeField]
    private float pushBackStrengthFire = 5f;

    [SerializeField] private float pushBackStrengthIce = 7.5f;

    [Header("Monsters alone Settings")] [SerializeField]
    private GameObject fireMonster;

    [SerializeField] private GameObject iceMonster;

    [Header("Health Settings")] [SerializeField]
    private float currHealth;

    [SerializeField] private float startHealth = 100;

    #endregion

    #region Animator Labels

    private static readonly int Damage = Animator.StringToHash("Demage");

    #endregion

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<EnemyAI>();
        _fireMonsterAnimator = fireMonster.GetComponent<Animator>();
        _iceMonsterAnimator = iceMonster.GetComponent<Animator>();
        currHealth = startHealth;
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemySpawnerDots = FindObjectOfType<EnemySpawnerDots>();
        ChangeState();
    }


    private void Update()
    {
        if (_state != GameManager.Shared.CurrentState)
            ChangeState();

        if (transform.position.y < -50) // enemy fell off arena
            KillEnemy();
        
        SideHandler();
    }


    /**
     * Make the enemy direction towards the player.
     */
    private void SideHandler()
    {
        if (_player.transform.position.x < transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);

        else if (_player.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
    }


    /**
     * A getter function for the health amount.
     */
    public float GetHealth()
    {
        return currHealth;
    }

    /**
     * Decreasing the enemy health and trigger the right damage animation.
     */
    public void DamageEnemy(int damage)
    {
        currHealth -= damage;
        GoBack();
        switch (_state)
        {
            case GameManager.WorldState.Fire:
                _fireMonsterAnimator.SetTrigger(Damage);
                break;

            case GameManager.WorldState.Ice:
                _iceMonsterAnimator.SetTrigger(Damage);
                break;
        }

        if (currHealth <= 0)
            KillEnemy();
    }

    private void GoBack()
    {
        var forceAmount = pushBackStrengthFire;
        if (_state == GameManager.WorldState.Ice)
            forceAmount = pushBackStrengthIce;
        
        
        float forceX = 0;
        float forceY = 0;

        var dir = transform.position - _player.transform.position; 
        
        // if (_player.transform.position.x < transform.position.x)
        //     forceX = forceAmount;
        // if (_player.transform.position.x > transform.position.x)
        //     forceX = -forceAmount;
        //
        // if (_player.transform.position.y < transform.position.y)
        //     forceY = forceAmount;
        // if (_player.transform.position.y > transform.position.y)
        //     forceY = -forceAmount;


        // _rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);

        _rb.AddForce(new Vector2(dir.x * forceAmount, dir.y * forceAmount), ForceMode2D.Impulse);


        // print("kaka");
        // var curVelocity = _rb.velocity;
        // _rb.AddForce(-200f * curVelocity);
    }


    /**
     * Decreasing the amount of enemies in the enemies counter spawner and lock the enemy movement.
     */
    public void KillEnemy()
    {
        GetComponent<EnemyAI>().enabled = false;
        _enemySpawnerDots.DecreaseMonster();
        
        // increase player health if MonsterRegenerationBuff is on
        var playerHealth = _player.GetComponent<PlayerHealth>();
        if (playerHealth)
            playerHealth.MonsterKillRegeneration();
    }

    /**
     * This function on charge of changing the enemy state,
     * that means switch between which monster is shown.
     */
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Top"))
            _isInTopHalf = true;
        
        if (other.gameObject.CompareTag("Bottom"))
            _isInTopHalf = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Arena Collider"))
            SetMonsterFall();
    }
    
    private void SetMonsterFall()
    {
        _rb.velocity = Vector2.zero;
        foreach (var collider in GetComponentsInChildren<Collider2D>())
            collider.enabled = false;
        GetComponentInChildren<Animator>().SetTrigger("Fall");
        _enemyAI.enabled = false;
        StartCoroutine(FallDelay());
    }

    private IEnumerator FallDelay()
    {
        _rb.gravityScale = _isInTopHalf ? -5f : 0;
        yield return new WaitForSeconds(_isInTopHalf ? 0.3f : 0.15f);
        if (_isInTopHalf)
            foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
                sr.sortingLayerName = "Default";
        _rb.gravityScale = 10;

    }
    
}