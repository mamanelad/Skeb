using UnityEngine;

public class RedSyringeShot : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _direction;
    private Vector3 _axis;
    private Vector3 _pos;
    
    [SerializeField] private float moveSpeed = 3.0f; // movement speed
    [SerializeField] private float frequency = 5.0f;  // speed of sine movement
    [SerializeField] private float magnitude = 0.9f;   // size of sine magnitude

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _pos = transform.position;
        _direction = (_player.transform.position - _pos).normalized;
        _axis = transform.right;
    }

    private void Update()
    {
        // calculating new position
        _pos += _direction * Time.deltaTime * moveSpeed;
        _pos.z = -2;
        var newPos = _pos + _axis * Mathf.Sin (Time.time * frequency) * magnitude;
        // calc. and fixing direction angle
        var syringeAngle = AngleBetweenVector3(transform.position, newPos);
        var syringeRotation = transform.rotation.eulerAngles;
        syringeRotation.z = syringeAngle;
        transform.rotation = Quaternion.Euler(syringeRotation);
        // setting new pos.
        transform.position = newPos;
    }
    
    private static float AngleBetweenVector3(Vector3 vec1, Vector3 vec2)
    {
        var directionVector = vec2 - vec1;
        var sign = (vec2.y < vec1.y)? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, directionVector) * sign;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}


