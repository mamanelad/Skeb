using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private float health = 0f;
    [SerializeField] private float maxHealth = 100f;

    void Start()
    {
        health = maxHealth;
    }


    public void UpdateHealth(float mod)
    {
        health += mod;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0)
        {
            health = 0;
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Player died");
        
    }
}