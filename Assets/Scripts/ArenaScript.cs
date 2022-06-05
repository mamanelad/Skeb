using System.Collections;
using UnityEngine;
using Cinemachine;

public class ArenaScript : MonoBehaviour
{
    #region Private Fields

    private float _flickerUpdateTime = 0.2f;
    private bool _iceArenaFlicker;
    private Animator _animator;
    [TagField][SerializeField] private string reflectionTag;
    
    #endregion

    #region Inspector Control

    [SerializeField] [Range(0,100)] private int iceArenaFlickerPercentage;
    
    #endregion

    #region Animator Labels

    private static readonly int WorldState = Animator.StringToHash("World State");
    private static readonly int Property = Animator.StringToHash("Special Effect");

    #endregion

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetInteger(WorldState, (int) GameManager.Shared.CurrentState);

        _flickerUpdateTime -= Time.deltaTime;

        if (_flickerUpdateTime < 0)
        {
            CheckForFlicker();
            _flickerUpdateTime = 0.2f;
        }

        if (GameManager.Shared.CurrentState == GameManager.WorldState.Fire)
        {
            _animator.SetBool(Property, false);
            _iceArenaFlicker = false;
        }
    }

    private void CheckForFlicker()
    {
        if (!_iceArenaFlicker && Random.Range(0, 100) <= iceArenaFlickerPercentage
                             && GameManager.Shared.CurrentState == GameManager.WorldState.Ice)
        {
            _iceArenaFlicker = true;
            StartCoroutine(IceArenaFlicker());
        }
    }

    private IEnumerator IceArenaFlicker()
    {
        _animator.SetBool(Property, true);
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool(Property, false);
        _iceArenaFlicker = false;

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
