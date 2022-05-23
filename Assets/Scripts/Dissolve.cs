using UnityEngine;

public class Dissolve : MonoBehaviour
{
    #region Private Fields

    private Enemy _enemyTogetherFather;
    private Material _material;
    private bool _isDissolving;

    #endregion

    #region Fields

    [SerializeField] private Material dissolveMaterial;
    [Range(0, 1)] public float fade = 1f;

    [Header("Coin Settings")] [SerializeField]
    private CoinPickUp coinPick;
    
    #endregion

    #region Animator Labels

    private static readonly int Fade = Shader.PropertyToID("Fade");
    
    #endregion


    private void Start()
    {
        // Get a reference to the material
        _material = GetComponent<SpriteRenderer>().material;
        _enemyTogetherFather = GetComponentInParent<Enemy>();

        if (dissolveMaterial == null)
            dissolveMaterial = _material;
    }

    private void Update()
    {
        if (_isDissolving)
        {
            SetDissolveShader();
            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;
                _isDissolving = false;
                InstantiateCoin();
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

    private void SetDissolveShader()
    {
        if (_material.shader != dissolveMaterial.shader)
            _material.shader = dissolveMaterial.shader;
    }

    private void InstantiateCoin()
    {
        Instantiate(coinPick, transform.position, Quaternion.identity);
    }
}