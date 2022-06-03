using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Cinemachine;

public class ArenaScript : MonoBehaviour
{
    [TagField][SerializeField] private string reflectionTag;
    private Animator _animator;
    
    
    private static readonly int WorldState = Animator.StringToHash("World State");


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetInteger(WorldState, (int) GameManager.Shared.CurrentState);
    }
    
    
    private void ChangeReflectionStatus(bool activeState)
    {
        foreach (var reflection in GameObject.FindGameObjectsWithTag(reflectionTag))
        {
            var color = reflection.GetComponent<SpriteRenderer>().color;
            color.a = activeState ? 0.5f : 0f;
            reflection.GetComponent<SpriteRenderer>().color = color;
            
        }
        
    }
}
