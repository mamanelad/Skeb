using System.Collections;
using UnityEngine;

public class HourGlass : MonoBehaviour
{
    private enum GlassState
    {
        IdleOne,
        IdleTwo,
        HourGlassIdle,
    }

    private enum ShakeSide
    {
        Right,
        Left
    }

    #region Inspector Control

    [SerializeField] private GameObject boxTutorial;
    [SerializeField] private PolygonCollider2D[] iceColliders;
    [SerializeField] private BoxCollider2D hourGlassCollider;
    [SerializeField] private float fallDelay = 2f;
    [SerializeField] private float iceMass = 1000000f;
    [SerializeField] private float hourGlassMass = 10;
    [SerializeField] private float shakeTime = 0.1f;
    [SerializeField] private float pushTime = 0.3f;
    [SerializeField] private float attackingTime = 0.5f;
    [SerializeField] private float boxTimeShow = 0.5f;

    #endregion

    #region Private Fields

    private float _attackingTimer;

    private Rigidbody2D _rb;
    private Animator _animator;

    private bool _isAttacking;
    private bool _hit;
    private bool _isInTopHalf;
    private bool _fall;
    private bool _shack;
    private bool _canPush;
    private bool _inColliderTrigger;
    private bool _boxShow;
    
    private ShakeSide _shakeSide = ShakeSide.Left;
    private GlassState _state = GlassState.IdleOne;

    private float _shakeTimer;
    private float _pushTimer;
    private float _boxTimerShow;
    private static readonly int BOne = Animator.StringToHash("bOne");
    private static readonly int BTwo = Animator.StringToHash("bTwo");
    private static readonly int BThree = Animator.StringToHash("bThree");

    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _rb.mass = iceMass;
    }


    private void Update()
    {
        if (_isAttacking)
        {
            _attackingTimer -= Time.deltaTime;
            if (_attackingTimer < 0)
                _isAttacking = false;
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

        if (_inColliderTrigger)
            HitHelper();

        //After fall of the hour glass
        if (_fall)
        {
            fallDelay -= Time.deltaTime;
            if (fallDelay < 0)
            {
                SwitchWorld();
                _boxShow = true;
                var uiManager = FindObjectOfType<UIManager>();
                if (uiManager != null)
                    uiManager.LaunchWorldStage();
                _boxTimerShow = boxTimeShow;
                _fall = false;
            }
        }

        if (_boxShow)
        {
            _boxTimerShow -= Time.deltaTime;
            if (_boxTimerShow < 0)
            {
                boxTutorial.SetActive(true);
                boxTutorial.GetComponent<Dissolve>().StartDissolve();
                Destroy(gameObject);
                _boxShow = false;
            }
        }

        if (_shack)
        {
            Shack();
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer < 0)
                _shack = false;
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
                _animator.SetTrigger(BOne);
                _state = GlassState.IdleTwo;
                break;

            case GlassState.IdleTwo:
                _animator.SetTrigger(BTwo);
                _state = GlassState.HourGlassIdle;
                break;

            case GlassState.HourGlassIdle:
                _animator.SetTrigger(BThree);
                break;
        }

        _hit = true;
        _isAttacking = false;
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
        foreach (var coll in iceColliders)
            coll.enabled = false;
        
        hourGlassCollider.enabled = true;
    }

    private void SetHourGlassFall()
    {
        _rb.velocity = Vector2.zero;
        foreach (var coll in GetComponentsInChildren<Collider2D>())
            coll.enabled = false;
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
            _inColliderTrigger = true;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        _inColliderTrigger = false;
    }

    private void HitHelper()
    {
        if (_isAttacking)
        {
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
        _isAttacking = true;
        _attackingTimer = attackingTime;
    }
}