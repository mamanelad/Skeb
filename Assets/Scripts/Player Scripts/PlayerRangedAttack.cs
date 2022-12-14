using System;
using System.Collections;
using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Animator _animator;
    private Rigidbody2D _rb;
    private Vector2 _direction;
    
    private enum Direction
    {
        Back,
        Right,
        Front,
        Left
    }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(FadeOut());
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
        _animator.SetFloat("DirectionHorizonatl", direction.x);
        _animator.SetFloat("DirectionVertical", direction.y);
        _animator.SetTrigger("PlayAnim");
    }
    
    private void Update()
    {
        if (GameManager.Shared.CurrentState != GameManager.WorldState.Fire)
            Destroy(gameObject);
        
        if (GetComponent<SpriteRenderer>().color.a <= 0.1f)
            Destroy(gameObject);
        

    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _direction * movementSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var monsterController = other.gameObject.GetComponent<Enemy>();
            if (monsterController)
                monsterController.DamageEnemy(damage);
        }
    }

    

    private IEnumerator FadeOut()
    {
        var color = GetComponent<SpriteRenderer>().color;
        while (color.a > 0.1f)
        {
            color.a -= 0.015f;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}
