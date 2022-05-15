using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private Enemy enemyFather;

    private Material material;

    private bool isDissolving = false;
    [Range(0,1)]
    [SerializeField] float fade = 1f;

    void Start()
    {
        // Get a reference to the material
        material = GetComponent<SpriteRenderer>().material;
        enemyFather = GetComponentInParent<Enemy>();

    }

    void Update()
    {

        if (isDissolving)
        {
            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;
                Destroy(enemyFather.gameObject);
            }

            // Set the property
            material.SetFloat("Fade", fade);
        }
        
        material.SetFloat("Fade", fade);
        
    }
    
     public void StartDissolve()
    {
        isDissolving = true;
    }
    
}