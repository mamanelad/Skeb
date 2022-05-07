using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyPatrolHorizontal : MonoBehaviour
{
    enum Direction
    {
        LEFT,
        RIGHT
    }

    private Direction facingDirection = Direction.RIGHT;
    private Vector3 baseScale;
    
    [SerializeField] private Transform castPos;
    [SerializeField] private float baseCastDict;
    
    private Rigidbody2D _rb;

    [SerializeField] private float moveSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float vX = moveSpeed;
        if (facingDirection == Direction.LEFT)
        {
            vX = -moveSpeed;
        }
        
        // print(facingDirection);
        // //Move the game object
        _rb.velocity = new Vector2(vX, _rb.velocity.y);

        if (IsHittingWall())
        {
            if (facingDirection == Direction.RIGHT)
            {
                ChangeFacingDirection(Direction.LEFT);
            }
            
            else if (facingDirection == Direction.LEFT)
            {
                ChangeFacingDirection(Direction.RIGHT);
            }
            
        }

        
    }

    bool IsHittingWall()
    {
        bool retVal = false;

        //define the cast distance for left and right
        //right
        float castDist = baseCastDict;
        //left
        if (facingDirection == Direction.LEFT)
            castDist = -baseCastDict;

        //determine the target destination based on the cast distance.
        Vector3 targetPosVertical = castPos.position;
        targetPosVertical.x += castDist;

        Debug.DrawLine(castPos.position, targetPosVertical, Color.blue);
        
        if (Physics2D.Linecast(castPos.position, targetPosVertical, 1 << LayerMask.NameToLayer("Terrain")))
        {
            retVal = true;
        }
        else
        {
            retVal = false;
        }
        
        return retVal;
    }

    void ChangeFacingDirection(Direction newDirect)
    {
        Vector3 newScale = baseScale;

        if (newDirect == Direction.LEFT)
        {
            newScale.x = -baseScale.x;
        }
        
        else if (newDirect == Direction.RIGHT)
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;
        facingDirection = newDirect ;

    }
}
