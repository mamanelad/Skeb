using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionScript : MonoBehaviour
{
    [SerializeField] private GameObject reflection;
    [SerializeField] private Sprite playerShadow;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _objectSpriteRenderer;
    private bool _isPlayer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _objectSpriteRenderer = reflection.GetComponent<SpriteRenderer>();

        if (reflection.GetComponent<PlayerController>() != null)
            _isPlayer = true;
    }
    
    private void Update()
    {
        if (_objectSpriteRenderer == null)
            return;

        if (GameManager.Shared.CurrentState == GameManager.WorldState.Ice)
        {
            _spriteRenderer.sprite = _objectSpriteRenderer.sprite;
            SetOpacity(0.4f);
        }
            
        else
            _spriteRenderer.sprite = null;

        if (_isPlayer) // this code is going to affect player only
        {
            if (GameManager.Shared.CurrentState == GameManager.WorldState.Fire)
            {
                _spriteRenderer.sprite = playerShadow;
                SetOpacity(0.5f);
            }

            if (PlayerController._PlayerController.GetPlayerState() == PlayerController.PlayerState.Falling)
                _spriteRenderer.sprite = null;
        }
        else 
        {
            if (GetComponentInParent<Rigidbody2D>().gravityScale != 0)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);

        }
    }

    private void SetOpacity(float opacity)
    {
        var color = _spriteRenderer.color;
        color.a = opacity;
        _spriteRenderer.color = color;
    }
}
