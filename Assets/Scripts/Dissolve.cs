using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private Material material;

    private bool isDissolving = false;
    [Range(0,1)]
    [SerializeField] float fade = 1f;

    void Start()
    {
        // Get a reference to the material
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDissolving = true;
        }

        if (isDissolving)
        {
            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;
            }

            // Set the property
            material.SetFloat("Fade", fade);
        }
        
        material.SetFloat("Fade", fade);


    }
    
}