using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourGlass : MonoBehaviour
{
    enum GlassState
    {
        IdleOne,
        IdleTwo,
        HourGlassIdle,
    }

    enum ShakeSide
    {
        Right,
        Left
    }

    [SerializeField] private GameObject boxTutorial;
    [SerializeField] private PolygonCollider2D[] iceColliders;
    [SerializeField] private BoxCollider2D hourGlassCollider;
    [SerializeField] private float fallDelay = 2f;
    [SerializeField] private float iceMass = 1000000f;
    [SerializeField] private float hourGlassMass = 10;

    [SerializeField] private float shakeTime = 0.1f;
    [SerializeField] private float pushTime = 0.3f;
    [SerializeField] private float attackingTime = 0.5f;
    private float attackingTimer;
    private FireParticleEffect _fireParticleEffect;
    private PlayerController _playerController;
    private Rigidbody2D _rb;
    private Animator _animator;

    private bool isAttacking;
    private bool _hit;
    private bool _isInTopHalf;
    private bool _fall;
    private bool _shack;
    private bool _canPush;
    private bool inColliderTrigger;
    private ShakeSide _shakeSide = ShakeSide.Left;
    private GlassState _state = GlassState.IdleOne;

    private float _shakeTimer;
    private float _pushTimer;


    private bool boxShow;
    [SerializeField] private float boxTimeShow = 0.5f;
    private float boxTimerShow;

    private void Awake()
    {
        _fireParticleEffect = GetComponentInChildren<FireParticleEffect>();
        _playerController = FindObjectOfType<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _rb.mass = iceMass;
    }


    private void Update()
    {
        if (isAttacking)
        {
            attackingTimer -= Time.deltaTime;
            if (attackingTimer < 0)
            {
                isAttacking = false;
            }
        }

        //Set if we can push the hour glass
        if (_canPush)
        {
            _pushTimer -= Time.deltaTime;
            if (_pushTimer < 0)
            {
                _canPush = false;
                _rb.mass = iceMass;
            }
        }

        if (inColliderTrigger)
        {
            HitHelper();
        }

        //After fall of the hour glass
        if (_fall)
        {
            fallDelay -= Time.deltaTime;
            if (fallDelay < 0)
            {
                SwitchWorld();
                boxShow = true;
                var uiManager = FindObjectOfType<UIManager>();
                if (uiManager != null)
                    uiManager.LaunchWorldStage();
                boxTimerShow = boxTimeShow;
                _fall = false;
            }
        }

        if (boxShow)
        {
            boxTimerShow -= Time.deltaTime;
            if (boxTimerShow < 0)
            {
                boxTutorial.SetActive(true);
                boxTutorial.GetComponent<Dissolve>().StartDissolve();
                
                Destroy(gameObject);
                boxShow = false;
            }
        }

        if (_shack)
        {
            Shack();
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer < 0)
            {
                _shack = false;
            }
        }
    }


    public void PushHourGlass()
    {
        if (_state == GlassState.HourGlassIdle)
        {
            _canPush = true;
            _pushTimer = pushTime;
            _rb.mass = hourGlassMass;
        }
    }

    public void HitHourGlass()
    {
        if (_hit) return;

        switch (_state)
        {
            case GlassState.IdleOne:
                _animator.SetTrigger("bOne");
                _state = GlassState.IdleTwo;
                break;

            case GlassState.IdleTwo:
                _animator.SetTrigger("bTwo");
                _state = GlassState.HourGlassIdle;
                break;

            case GlassState.HourGlassIdle:
                GameManager.Shared.AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.IceBergBreak);
                _fireParticleEffect.isOn = false;
                _animator.SetTrigger("bThree");
                break;
        }

        _hit = true;
        isAttacking = false;
    }

    public void CanHit()
    {
        _hit = false;
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

        _fall = true;
        GameManager.Shared.AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.HourGlassBreak);
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
        {
            HitHelper();
            inColliderTrigger = true;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        inColliderTrigger = false;
    }

    private void HitHelper()
    {
        if (isAttacking)
        {
            GameManager.Shared.AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.IceBergHit);
            _shack = true;
            _shakeTimer = shakeTime;
            HitHourGlass();
        }
    }

    private void Shack()
    {
        switch (_shakeSide)
        {
            case ShakeSide.Left:
                transform.position += new Vector3(0.1f, 0, 0f);
                _shakeSide = ShakeSide.Right;
                break;
            case ShakeSide.Right:
                transform.position -= new Vector3(0.1f, 0, 0f);
                _shakeSide = ShakeSide.Left;
                break;
        }
    }

    public void IsAttacking()
    {
        isAttacking = true;
        attackingTimer = attackingTime;
    }
}