using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class IceSpikeScript : MonoBehaviour
{
    [SerializeField] private List<Sprite> _images;
    [SerializeField] private int spikeDamage;
    [SerializeField] private bool destroyOnCollision;
    [SerializeField] private float timeToDestroy;
    private SpriteRenderer _sp;

    void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _sp.sprite = _images[Random.Range(0, _images.Count)];
        Destroy(gameObject, timeToDestroy);
        // play ice spawn sound 
    }

    private void Update()
    {
        if (GameManager.Shared.CurrentState == GameManager.WorldState.Fire)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Arena Collider"))
            Destroy(gameObject);
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            var monster = other.gameObject;
            var monsterController = monster.GetComponent<Enemy>();
            if (monsterController != null)
            {
                // play spike damage sound
                monsterController.DamageEnemy(spikeDamage);
            }
            if (destroyOnCollision)
            {
                // play distruction sound
                Destroy(gameObject);
            }
        }
    }
}
