using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float step = 0.1f;
    [SerializeField] private GameObject target;
    private int _frameCounter = 30;

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
        Destroy(gameObject, 3);
        GameManager.Shared.AudioManagerGeneral.PlaySound(GeneralSound.SoundKindsGeneral.HeartExist);
        target = GameObject.FindGameObjectWithTag("lifeBar");
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void setValue(int newValue)
    {
        coinValue = newValue;
    }

    private void FixedUpdate()
    {
        _frameCounter -= 1;

        if (_frameCounter <= 0)
        {
            _frameCounter = 30;
            var dist = Vector3.Distance(target.transform.position, transform.position);
            if (dist <= minDistance)
                AddCoins();
        }
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Life Bar"))
            AddCoins();
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
