using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionScript : MonoBehaviour
{
    [SerializeField] private GameObject reflection;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _objectSpriteRenderer;
    private bool _isPlayer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _objectSpriteRenderer = reflection.GetComponent<SpriteRenderer>();

        if (reflection.GetComponent<PlayerController>() != null)
            _isPlayer = true;
    }
    
    void Update()
    {
        if (_objectSpriteRenderer == null)
            return;

        _spriteRenderer.sprite = _objectSpriteRenderer.sprite;

        if (_isPlayer) // this code is going to affect player only
        {
            if (PlayerController._PlayerController.GetPlayerState() == PlayerController.PlayerState.Falling)
                gameObject.SetActive(false);
        }
        else // this code will affect anything but the player
        {
            
        }

    }
}
