using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxTutorial : MonoBehaviour
{
    private enum ShakeSide
    {
        Right,
        Left
    }

    #region Private Fields

    private ShakeSide _shakeSide = ShakeSide.Left;
    private float _hitTimer;
    private float _shakeTimer;
    private bool _shack;

    private bool _hit;
    private bool _switchToMainScene;

    private Animator _animator;
    private PlayerController _playerController;

    #endregion

    #region Inspector Control

    [SerializeField] private float switchSceneDelay = 2f;
    [SerializeField] private float shakeTime = 0.2f;
    [SerializeField] private GameObject skebName;
    private static readonly int Hit = Animator.StringToHash("hit");

    #endregion

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (_hit)
        {
            _hitTimer -= Time.deltaTime;
            if (_hitTimer < 0)
                _hit = false;
        }

        if (_shack)
        {
            Shack();
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer < 0)
                _shack = false;
        }

        if (_switchToMainScene)
        {
            switchSceneDelay -= Time.deltaTime;
            if (switchSceneDelay < 0)
                SwitchToMainScene();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            HitHelper();
    }

    private void HitHelper()
    {
        if (_hit) return;
        if (_playerController.IsAttacking)
        {
            _shack = true;
            _shakeTimer = shakeTime;
            _animator.SetTrigger(Hit);
            _hit = true;
        }
    }

    public void ShowName()
    {
        skebName.SetActive(true);
        _switchToMainScene = true;
    }

    private void SwitchToMainScene()
    {
        SceneManager.LoadScene("Tamir new Arena");
    }


    private void Shack()
    {
        switch (_shakeSide)
        {
            case ShakeSide.Left:
                transform.position += new Vector3(0.1f, 0, 0f);
                _shakeSide = ShakeSide.Right;
                break;
            case ShakeSide.Right:
                transform.position -= new Vector3(0.1f, 0, 0f);
                _shakeSide = ShakeSide.Left;
                break;
        }
    }

    public void ChangeLayerToGui()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "GUI";
    }
}