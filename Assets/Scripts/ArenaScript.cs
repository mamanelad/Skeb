using System.Collections;
using UnityEngine;
using Cinemachine;


public class ArenaScript : MonoBehaviour
{
    [TagField][SerializeField] private string reflectionTag;
    private Animator _animator;
    [SerializeField] [Range(0,100)] private int iceArenaFlickerPercentage;
    private float flickerUpdateTime = 0.2f;
    private bool iceArenaFlicker;
    
    
    private static readonly int WorldState = Animator.StringToHash("World State");


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetInteger(WorldState, (int) GameManager.Shared.CurrentState);

        flickerUpdateTime -= Time.deltaTime;

        if (flickerUpdateTime < 0)
        {
            CheckForFlicker();
            flickerUpdateTime = 0.2f;
        }

        if (GameManager.Shared.CurrentState == GameManager.WorldState.Fire)
        {
            _animator.SetBool("Special Effect", false);
            iceArenaFlicker = false;
        }
    }

    private void CheckForFlicker()
    {
        if (!iceArenaFlicker && Random.Range(0, 100) <= iceArenaFlickerPercentage
                             && GameManager.Shared.CurrentState == GameManager.WorldState.Ice)
        {
            iceArenaFlicker = true;
            StartCoroutine(IceArenaFlicker());
        }
    }

    private IEnumerator IceArenaFlicker()
    {
        _animator.SetBool("Special Effect", true);
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool("Special Effect", false);
        iceArenaFlicker = false;

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
