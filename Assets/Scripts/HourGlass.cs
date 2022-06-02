using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourGlass : MonoBehaviour
{
    private GameObject player;
    private bool _isInTopHalf;
    private bool _isDead;
    private Rigidbody2D _rb;
    private PlayerController playerMovement;
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

    [SerializeField] private float shackDistance = 3;
    
    enum GlassState
    {
        IdleOne,
        IdleTwo,
        HourGlassIdle,
    }

    private GlassState state = GlassState.IdleOne;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _playerController = FindObjectOfType<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerController>();
        _animator = GetComponent<Animator>();
    }
    

    private void FixedUpdate()
    {
        var dist = Vector3.Distance(transform.position, player.transform.position);
        print(dist);
        if ( dist < shackDistance)
        {
            
            _animator.SetTrigger("Shake");
        }

        else
        {
            _animator.SetTrigger("StopShake");
        }

        if (_playerController._dashStatus && state == GlassState.HourGlassIdle)
        {
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
        GameManager.Shared.CurrentState = GameManager.WorldState.Ice;
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
            SetHourGlassFall();
    }

  
    
}