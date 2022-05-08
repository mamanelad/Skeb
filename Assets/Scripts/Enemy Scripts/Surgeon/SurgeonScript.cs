using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SurgeonScript : MonoBehaviour
{
    [SerializeField] private GameObject syringe;
    [SerializeField] private List<GameObject> attacks;
    [SerializeField] private float attackDelay;
    private float _attackTimer;
    private GameObject _player;
    private bool _playerDetected;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _attackTimer = attackDelay;
    }

    private void Update()
    {
        if (!_playerDetected)
            return;
        FixSyringePosition();
        DoAttack();
    }

    private void FixSyringePosition()
    {
        var playerPos = _player.transform.position;
        var doctorPos = transform.position;
        // fix syringe rotation 
        var syringeAngle = AngleBetweenVector3(doctorPos, playerPos);
        var syringeRotation = syringe.transform.rotation.eulerAngles;
        syringeRotation.z = syringeAngle;
        syringe.transform.rotation = Quaternion.Euler(syringeRotation);
        // fix syringe position
        var syringeDirection = (playerPos - doctorPos).normalized;
        var syringeNewPos = doctorPos + syringeDirection * 1.5f;
        syringe.transform.position = syringeNewPos;
    }

    private void DoAttack()
    {
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
            return;
        }

        _attackTimer = attackDelay;
        var attack = Instantiate(attacks[Random.Range(0, attacks.Count)], syringe.transform.position, 
            quaternion.identity);
        var script = attack.GetComponent<GreenSyringeShot>();
        if (script != null)
            script.SetBasePos(syringe);
    }

    private float AngleBetweenVector3(Vector3 vec1, Vector3 vec2)
    {
        var directionVector = vec2 - vec1;
        var sign = vec2.y < vec1.y ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, directionVector) * sign;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            _playerDetected = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            _playerDetected = false;
    }
}