using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourGlass : MonoBehaviour
{
    [SerializeField] private GameObject boxTutorial;
    private GameObject player;
    private bool _isInTopHalf;
    private bool _isDead;
    private Rigidbody2D _rb;
    private Animator _animator;
    [SerializeField] private PolygonCollider2D[] iceColliders;
    [SerializeField] private BoxCollider2D hourGlassCollider;

    [SerializeField] private float gapTime = 0.5f;
    private float gapTimer;
    private bool hit;

    [SerializeField] private float switchWorldTime = 2f;
    private float switchWorldTimer;
    private bool switchWorld;

    private PlayerController _playerController;
    private bool fall;
    [SerializeField] private float fallDelay = 2f;
    [SerializeField] private float shackDistance = 3;

    [SerializeField] private float iceMass = 1000000f;
    [SerializeField] private float hourGlassMass = 10;
    
    enum GlassState
    {
        IdleOne,
        IdleTwo,
        HourGlassIdle,
    }

    enum shakeSide
    {
        Right,
        Left
    }

    private shakeSide _shakeSide = shakeSide.Left;
    private GlassState state = GlassState.IdleOne;
    [SerializeField] private float shakeTime = 0.2f;
    private float shakeTimer;
    private bool shack;

    private bool canPush;
    [SerializeField] private float pushTime = 0.3f;
    private float pushTimer;
    
    private void Awake()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        _playerController = FindObjectOfType<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _rb.mass = iceMass;
    }


    private void Update()
    {
        if (canPush)
        {
            pushTimer -= Time.deltaTime;
            if (pushTimer < 0)
            {
                canPush = false;
                _rb.mass = iceMass;
            }
                
        }
        
        if (_playerController._dashStatus && state == GlassState.HourGlassIdle)
        {
            canPush = true;
            pushTimer = pushTime;
            _rb.mass = hourGlassMass;
        }

        
        

        if (fall)
        {
            fallDelay -= Time.deltaTime;
            if (fallDelay < 0)
            {
                SwitchWorld();
                boxTutorial.SetActive(true);
                Destroy(gameObject);
            }
        }

        if (shack)
        {
            Shack();
            shakeTimer -= Time.deltaTime;
            if (shakeTimer < 0)
            {
                shack = false;
            }
        }
    }

    

    public void HitHourGlass()
    {
        if (hit) return;

        switch (state)
        {
            case GlassState.IdleOne:
                _animator.SetTrigger("bOne");
                state = GlassState.IdleTwo;
                break;

            case GlassState.IdleTwo:
                _animator.SetTrigger("bTwo");
                state = GlassState.HourGlassIdle;
                break;

            case GlassState.HourGlassIdle:
                _animator.SetTrigger("bThree");
                // _rb.mass = hourGlassMass;

                // switchWorld = true;
                // switchWorldTimer = switchWorldTime;
                break;
        }

        gapTimer = gapTime;
        hit = true;
    }

    public void CanHit()
    {
        hit = false;
    }

    public void SwitchWorld()
    {
        switch (GameManager.Shared.CurrentState)
        {
            case GameManager.WorldState.Fire:
                GameManager.Shared.CurrentState = GameManager.WorldState.Ice;
                break;

            case GameManager.WorldState.Ice:
                GameManager.Shared.CurrentState = GameManager.WorldState.Fire;
                break;
        }

        SwitchToHourGlass();
    }

    private void SwitchToHourGlass()
    {
        foreach (var collider in iceColliders)
            collider.enabled = false;


        hourGlassCollider.enabled = true;
    }

    private void SetHourGlassFall()
    {
        _rb.velocity = Vector2.zero;
        foreach (var collider in GetComponentsInChildren<Collider2D>())
            collider.enabled = false;
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

        fall = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Top"))
            _isInTopHalf = true;

        if (other.gameObject.CompareTag("Bottom"))
            _isInTopHalf = false;

        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Arena Collider"))
            SetHourGlassFall();

        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }


    private void HitHelper()
    {
        if (_playerController.IsAttacking)
        {
            shack = true;
            shakeTimer = shakeTime;
            HitHourGlass();
        }
    }

    private void Shack()
    {
        switch (_shakeSide)
        {
            case shakeSide.Left:
                transform.position += new Vector3(0.1f, 0, 0f);
                _shakeSide = shakeSide.Right;
                break;
            case shakeSide.Right:
                transform.position -= new Vector3(0.1f, 0, 0f);
                ;
                _shakeSide = shakeSide.Left;
                break;
        }
    }
}