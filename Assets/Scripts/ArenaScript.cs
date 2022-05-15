using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Cinemachine;

public class ArenaScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> fireWorldObjects;
    [SerializeField] private List<GameObject> iceWorldObjects;
    [TagField][SerializeField] private string reflectionTag;
    
    private GameManager.WorldState _currentWorldState;
    
    private void Update()
    {
        _currentWorldState = GameManager.Shared.CurrentState;
        
        switch (_currentWorldState)
        {
            case GameManager.WorldState.None:
                break;
            
            case GameManager.WorldState.Fire:
                ActivateWorld(fireWorldObjects);
                DeactivateWorld(iceWorldObjects);
                ChangeReflectionStatus(false);
                break;
            
            case GameManager.WorldState.Ice:
                ActivateWorld(iceWorldObjects);
                DeactivateWorld(fireWorldObjects);
                ChangeReflectionStatus(true);
                break;
            
            default:
                Debug.Log("ArenaScript - Couldn't switch world");
                break;
        }

    }

    private static void ActivateWorld(List<GameObject> worldItems)
    {
        foreach (var item in worldItems)
        {
            item.SetActive(true);
        }
    }
    
    private static void DeactivateWorld(List<GameObject> worldItems)
    {
        foreach (var item in worldItems)
        {
            item.SetActive(false);
        }
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
