using System.Collections;
using UnityEngine;

public class RangedAttackEchoSpawn : MonoBehaviour
{
    [SerializeField] private GameObject echoObject;
    private SpriteRenderer _spriteRenderer;
    private float _timeBeforeEcho = 0.01f;
    private bool _echoStarted;
    private bool flipEchoX;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }
    
    private void Update()
    {
        _timeBeforeEcho -= Time.deltaTime;

        if (_timeBeforeEcho < 0 && !_echoStarted)
        {
            _echoStarted = true;
            StartCoroutine(EchoEffect());
        }
    }
    

    private IEnumerator EchoEffect()
    {
        while (true)
        {
            var newEcho = Instantiate(echoObject, transform.position, Quaternion.identity);
            var echoEffectManager = newEcho.GetComponent<EchoObjectScript>();
            echoEffectManager.SetSprite(_spriteRenderer.sprite);
            StartCoroutine(echoEffectManager.DecreaseOpacityRoutine());
            if (flipEchoX)
                echoEffectManager.FlipVerticalSpriteDirection();
            yield return new WaitForSeconds(echoEffectManager.GetSpawnTimeSteps());
        }
    }

    public void FlipEchoVertical()
    {
        flipEchoX = true;
    }
}
