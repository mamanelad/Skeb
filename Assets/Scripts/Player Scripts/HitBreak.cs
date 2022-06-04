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
        if (_hitBreakOn || _playerController.IsPlayerDead) return;
        
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
        yield return new WaitUntil(() => Time.timeScale != 0);
        Time.timeScale = 1.0f;
        _hitBreakOn = false;
    }

    private IEnumerator SetOldShader()
    {
        var material = _renderer.material;
        while (Time.timeScale != 1.0f)
            yield return null;
        var switchTurns = (int) _playerController.stunDuration / 0.2f;
        for (var i = 0; i < switchTurns; i++)
        {
            yield return new WaitUntil(() => Time.timeScale != 0);
            yield return new WaitForSecondsRealtime(0.1f);
            material.shader = _curShader;
            yield return new WaitForSecondsRealtime(0.1f);
            material.shader = hitShader;
        }
        material.shader = _curShader;
        _playerController.isStunned = false;
    }
    
    
}
