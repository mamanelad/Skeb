using System.Collections;
using UnityEngine;

public class HitBreak : MonoBehaviour
{
    #region Inspector Fields
    
    [SerializeField] private Shader hitShader;
    [SerializeField] private float duration = 1f;
    
    #endregion

    #region Private Fields
    
    private PlayerController _playerController;
    private SpriteRenderer _renderer;
    private Shader _curShader;
    private bool _hitBreakOn;

    #endregion

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _renderer = GetComponent<SpriteRenderer>();
        _curShader = _renderer.sharedMaterial.shader;

        if (hitShader == null)
            hitShader = _curShader;
    }

    public void HitBreakAction()
    {
        if (_hitBreakOn) return;
        
        var material = _renderer.material;
        material.shader = hitShader;
        _playerController.isStunned = true;
        Time.timeScale = 0.1f;
        _playerController.KnockBackStatus = true;

        StartCoroutine(Break());
        StartCoroutine(SetOldShader());

    }

    private IEnumerator Break()
    {
        _hitBreakOn = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        _hitBreakOn = false;
    }

    private IEnumerator SetOldShader()
    {
        var material = _renderer.material;
        while (Time.timeScale != 1.0f)
            yield return null;
        yield return new WaitForSecondsRealtime(_playerController.stunDuration);
        material.shader = _curShader;
        _playerController.isStunned = false;
    }
    
    
}
