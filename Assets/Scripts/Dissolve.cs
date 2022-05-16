using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private Enemy _enemyFather;
    private Material _material;
    [Range(0, 1)] public float fade = 1f;
    private bool _isDissolving;
    private static readonly int Fade = Shader.PropertyToID("Fade");
    
    private void Start()
    {
        // Get a reference to the material
        _material = GetComponent<SpriteRenderer>().material;
        _enemyFather = GetComponentInParent<Enemy>();
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
                Destroy(_enemyFather.gameObject);
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