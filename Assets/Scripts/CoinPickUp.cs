using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float step = 0.1f;
     [SerializeField] private GameObject target;

    private enum CoinKind
    {
        Fire,
        Ice,
        Heart
    }

    [SerializeField] private CoinKind coinKind;
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite iceSprite;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private GameManager.WorldState _state;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Shared.AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.HeartExist);
        target = GameObject.FindGameObjectWithTag("lifeBar");
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SwitchState();
    }

    // Update is called once per frame
    void Update()
    {
        if (_state != GameManager.Shared.CurrentState)
        {
            SwitchState();
            
        }

    }


    private void FixedUpdate()
    {
        var dist = Vector3.Distance(target.transform.position, transform.position);
        if (dist <= minDistance)
        {
            AddCoins();
            
        }
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
                _animator.SetBool("Fire", true);
                _animator.SetBool("Ice", false);
                break;
            
            case GameManager.WorldState.Ice:
                _spriteRenderer.sprite = iceSprite;
                _animator.SetBool("Fire", false);
                _animator.SetBool("Ice", true);
                break;
        }
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
           AddCoins();
        }
    }

    private void AddCoins()
    {
        if (coinKind == CoinKind.Heart)
        {
            GameManager.Shared.AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.HeartAdd);
            FindObjectOfType<PlayerHealth>().UpdateHealth(coinValue, Vector3.zero);
        }

        Destroy(gameObject);
    }
}
