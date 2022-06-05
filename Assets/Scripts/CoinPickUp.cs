using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    private enum CoinKind
    {
        Fire,
        Ice,
        Heart
    }

    #region Private Fields

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private GameManager.WorldState _state;

    #endregion

    #region Inspector Control

    [SerializeField] private CoinKind coinKind;

    [Header("End Settings")] [SerializeField]
    private int coinValue = 1;

    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float step = 0.1f;
    [SerializeField] private GameObject target;

    [Header("Sprites Settings")] [SerializeField]
    private Sprite fireSprite;

    [SerializeField] private Sprite iceSprite;
    private static readonly int Fire = Animator.StringToHash("Fire");
    private static readonly int Ice = Animator.StringToHash("Ice");

    #endregion

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("lifeBar");
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SwitchState();
    }
    
    private void Update()
    {
        if (_state != GameManager.Shared.CurrentState)
            SwitchState();
    }
    
    private void FixedUpdate()
    {
        var dist = Vector3.Distance(target.transform.position, transform.position);
        if (dist <= minDistance)
            AddCoins();

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    private void SwitchState()
    {
        if (coinKind == CoinKind.Heart) return;
        _state = GameManager.Shared.CurrentState;

        switch (_state)
        {
            case GameManager.WorldState.Fire:
                _spriteRenderer.sprite = fireSprite;
                _animator.SetBool(Fire, true);
                _animator.SetBool(Ice, false);
                break;

            case GameManager.WorldState.Ice:
                _spriteRenderer.sprite = iceSprite;
                _animator.SetBool(Fire, false);
                _animator.SetBool(Ice, true);
                break;
        }
    }

    private void AddCoins()
    {
        if (coinKind == CoinKind.Heart)
            FindObjectOfType<PlayerHealth>().UpdateHealth(coinValue, Vector3.zero);

        Destroy(gameObject);
    }
}