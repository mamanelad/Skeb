using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private enum CoinKind
    {
        Fire,
        Ice
    }

    [SerializeField] private CoinKind _coinKind;
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite iceSprite;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private GameManager.WorldState _state;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SwichState();
    }

    // Update is called once per frame
    void Update()
    {
        if (_state != GameManager.Shared.CurrentState)
        {
            SwichState();
        }
    }


    private void SwichState()
    {
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
            switch (_coinKind)
            {
                case CoinKind.Fire:
                    GameManager.Shared.fireCoins += coinValue;
                    break;

                case CoinKind.Ice:
                    GameManager.Shared.iceCoins += coinValue;
                    break;
            }

            Destroy(gameObject);
        }
    }
}
