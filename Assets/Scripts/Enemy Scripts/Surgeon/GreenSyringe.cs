using UnityEngine;

public class GreenSyringe : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;
    private Vector3 _direction;
    private Vector3 _pos;
    
    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        _pos = transform.position;
        var syringeAngle = AngleBetweenVector3(transform.position, player.transform.position);
        var syringeRotation = transform.rotation.eulerAngles;
        syringeRotation.z = syringeAngle;
        transform.rotation = Quaternion.Euler(syringeRotation);
    }

    private void Update()
    {
        _pos += _direction * Time.deltaTime * moveSpeed;
        _pos.z = -2;
        transform.position = _pos;
        
    }
    
    private static float AngleBetweenVector3(Vector3 vec1, Vector3 vec2)
    {
        var directionVector = vec2 - vec1;
        var sign = (vec2.y < vec1.y)? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, directionVector) * sign;
    }
    
    public void SetDirection(Vector3 newDirection)
    {
        _direction = newDirection;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
