using UnityEngine;
using UnityEngine.Serialization;

namespace FireParticle
{
    public enum AlphaFalloff
    {
        None,
        Linear,
        Sqrt
    }; 

    public class FireParticle : MonoBehaviour
    {
        #region private Fields

        private float _actualLifeSpan;
        private float _timeAlive;
        private Vector2 _velocity;
        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        
        #endregion
        
        #region Inspector Control

        [FormerlySerializedAs("MinVelocity")] [SerializeField]
        private Vector2 minVelocity = new Vector2(-0.05f, 0.1f);

        [FormerlySerializedAs("MaxVelocity")] [SerializeField]
        private Vector2 maxVelocity = new Vector2(0.05f, 0.2f);

        [FormerlySerializedAs("LifeSpan")] [SerializeField] private float lifeSpan = 2f;
        
        [FormerlySerializedAs("DestroysSelf")] public bool destroysSelf = true;

        [FormerlySerializedAs("AlphaFalloff")] public AlphaFalloff alphaFalloff;

        #endregion
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
        }

        private void OnEnable()
        {
            _velocity = new Vector2(Random.Range(minVelocity.x, maxVelocity.x),
                Random.Range(minVelocity.y, maxVelocity.y));

            _actualLifeSpan = lifeSpan * Random.Range(0.9f, 1.1f);

            _timeAlive = 0;

            _spriteRenderer.color = _originalColor;
        }
        
        private void Update()
        {
            _timeAlive += Time.deltaTime;

            if (destroysSelf && _timeAlive >= _actualLifeSpan)
            {
                SimplePool.Despawn(gameObject);
                return;
            }

            if (alphaFalloff == AlphaFalloff.Linear)
            {
                // As the particle gets older, it fades out
                var alpha = Mathf.Clamp01(1.0f - (_timeAlive / _actualLifeSpan));
                var newColor = _originalColor;
                newColor.a *= alpha;
                _spriteRenderer.color = newColor;
            }
            else if (alphaFalloff == AlphaFalloff.Sqrt)
            {
                // As the particle gets older, it fades out
                var alpha = Mathf.Clamp01(1.0f - (_timeAlive / _actualLifeSpan));
                alpha = Mathf.Sqrt(alpha);
                var newColor = _originalColor;
                newColor.a *= alpha;
                _spriteRenderer.color = newColor;
            }

            this.transform.Translate(_velocity * Time.deltaTime);
        }
    }
}