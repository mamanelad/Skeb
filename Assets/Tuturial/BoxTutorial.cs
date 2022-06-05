using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxTutorial : MonoBehaviour
{
    enum shakeSide
    {
        Right,
        Left
    }

    private shakeSide _shakeSide = shakeSide.Left;
    [SerializeField] private float shakeTime = 0.2f;
    private float shakeTimer;
    private bool shack;

    [SerializeField] private GameObject skebName;

    private Animator _animator;

    [SerializeField] private float hitStep = 1f;
    private float hitTimer;
    private bool hit;
    private PlayerController _playerController;

    private bool switchToMainScene;
    [SerializeField] private float switchSceneDelay = 2f;
    private SpriteRenderer _spriteRenderer;


    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (hit)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer < 0)
            {
                hit = false;
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

        if (switchToMainScene)
        {
            switchSceneDelay -= Time.deltaTime;
            if (switchSceneDelay < 0)
            {
                SwitchToMainScene();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
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
        if (hit) return;
        if (_playerController.IsAttacking)
        {
            shack = true;
            shakeTimer = shakeTime;
            _animator.SetTrigger("hit");
            hit = true;
        }
    }

    public void ShowName()
    {
        skebName.SetActive(true);
        switchToMainScene = true;
    }

    private void SwitchToMainScene()
    {
        SceneManager.LoadScene("Tamir new Arena");
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
                _shakeSide = shakeSide.Left;
                break;
        }
    }

    public void ChangeLayerToGui()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "GUI";
    }
}