using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxTutorial : MonoBehaviour
{
    [SerializeField] private GameObject skebName;

    [SerializeField] private Animator _animator;

    [SerializeField] private float hitStep = 1f;
    private float hitTimer;
    private bool hit;
    private PlayerController _playerController;

    private bool switchToMainScene;
    [SerializeField] private float switchSceneDelay = 2f;


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
        if (!hit)
        {
            if (other.CompareTag("Player") && _playerController.IsAttacking)
            {
                _animator.SetTrigger("hit");
                hit = true;
            } 
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hit)
        {
            if (other.CompareTag("Player") && _playerController.IsAttacking)
            {
                _animator.SetTrigger("hit");
                hit = true;
            } 
        }
    }

    public void ShowName()
    {
        skebName.SetActive(true);
        switchToMainScene = true;
    }

    private void SwitchToMainScene()
    {
        SceneManager.LoadScene("Main");
    }
    
}