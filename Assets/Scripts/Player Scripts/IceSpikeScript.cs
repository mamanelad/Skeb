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
    private SpriteRenderer _sp;

    void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _sp.sprite = _images[Random.Range(0, _images.Count)];
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            var monster = other.gameObject;
            var monsterController = monster.GetComponent<Enemy>();
            if (monsterController != null)
                monsterController.DamageEnemy(spikeDamage);
        }

    }
}
