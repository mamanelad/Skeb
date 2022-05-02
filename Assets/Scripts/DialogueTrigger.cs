using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue _dialogueScript;
    private bool _playerDetected;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerDetected = true;
            _dialogueScript.ToggleIndicator(_playerDetected);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerDetected = false;
            _dialogueScript.ToggleIndicator(_playerDetected);
            _dialogueScript.EndDialogue();
        }
    }

    private void Update()
    {
        if (_playerDetected && Input.GetKeyDown(KeyCode.Space))
        {
            _dialogueScript.StartDialogue();
        }
    }
}
