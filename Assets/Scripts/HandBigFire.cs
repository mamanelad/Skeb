using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBigFire : MonoBehaviour
{
    private bool canSeeHand;
    private float canSeeHandTimer;

    private bool inside;
    private SpriteRenderer _sp;
    private PlayerController _player;
    [SerializeField] private float canSeeHandTime = .1f;

    private void Awake()
    {
        _sp = GetComponent<SpriteRenderer>();
        HideHand();
        _player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (canSeeHand)
        {
            AimHand();
            canSeeHandTimer -= Time.deltaTime;
            if (canSeeHandTimer <= 0)
                HideHand();
        }
    }

    private void AimHand()
    {
        var direction = _player.transform.position - transform.position;
        var handRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (!(handRotation <= 90 && handRotation >= -90))
            handRotation += 180;
        transform.rotation = Quaternion.Euler(0f, 0f, handRotation + 0);
    }
    
    public void ShowAndAttack(float attackRange, float attackDamage, float screenShakeIntensity, float screenShakeTime)
    {
        var newCol = _sp.color;
        newCol.a = 1;
        _sp.color = newCol;
        canSeeHand = true;
        canSeeHandTimer = canSeeHandTime;

        if (inside)
        {
            if (FindObjectOfType<CinemaMachineShake>())
                CinemaMachineShake.Instance.ShakeCamera(screenShakeIntensity, screenShakeTime);
            _player.GetComponent<PlayerHealth>().UpdateHealth(-attackDamage, transform.position);
        }
        
        gameObject.SetActive(false);
    }

    private void HideHand()
    {
        canSeeHand = false;
        var newCol = _sp.color;
        newCol.a = 0;
        _sp.color = newCol;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            inside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            inside = false;    }
}