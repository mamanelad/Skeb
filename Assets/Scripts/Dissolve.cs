using UnityEngine;

public class Dissolve : MonoBehaviour
{
    #region Private Fields

    private Enemy _enemyTogetherFather;
    private Material _material;
    private bool _isDissolving;

    #endregion

    #region Public Fields

    [Range(0, 1)] public float fade = 1f;

    #endregion

    #region Animator Labels

    private static readonly int Fade = Shader.PropertyToID("Fade");
    
    #endregion


    private void Start()
    {
        // Get a reference to the material
        _material = GetComponent<SpriteRenderer>().material;
        _enemyTogetherFather = GetComponentInParent<Enemy>();
    }

    private void Update()
    {
        if (_isDissolving)
        {
            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;
                _isDissolving = false;
                Destroy(_enemyTogetherFather.gameObject);
            }

            // Set the property
            _material.SetFloat(Fade, fade);
        }

        _material.SetFloat(Fade, fade);
    }

    public void StartDissolve()
    {
        _isDissolving = true;
    }
}