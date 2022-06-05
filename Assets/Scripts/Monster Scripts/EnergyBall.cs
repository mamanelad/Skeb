using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnergyBall : MonoBehaviour
{
    #region Private Fields

    private Animator _animator;
    private GameObject _player;
    private bool _startLife;
    private bool _hit;
    private bool goAfter;
    
    #endregion

    #region Inspector Control

    [SerializeField] private float lifeBallTimer = 3f;
    [SerializeField] private float timeToDieAfterHit = 0.05f;
    [SerializeField] private float step = 1f;
    [HideInInspector] public float _attackDamage = 20f;
    
    #endregion

    #region Animator Labels

    private static readonly int Die = Animator.StringToHash("Die");

    #endregion

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
        
    }

    private void FixedUpdate()
    {
        if (!_startLife) return;
        
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);
            lifeBallTimer -= Time.deltaTime;
            if (lifeBallTimer <= 0)
            {
                _animator.SetTrigger(Die);
                _startLife = false;
            } 
        
        
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _hit = true;
            _player.GetComponent<PlayerHealth>().UpdateHealth(-_attackDamage, transform.position);
            _animator.SetTrigger(Die);
        }
       
    }

    /**
     * This function declare that the ball can start his movement.
     * Called from the end of the ball creat animation.
     */
    public void StartLife()
    {
        _startLife = true;
    }

    /**
     * Destroy the game object.
     * Called if the amount of time for the ball has ended or the ball hit the player.
     */
    public void DestroyBall()
    {
        GameManager.Shared.monsterAudioManager.
            PlaySound(MonsterSounds.SoundKindsMonster.EnergyBallExplosion, transform.position);
        Destroy(gameObject);
    }
}
