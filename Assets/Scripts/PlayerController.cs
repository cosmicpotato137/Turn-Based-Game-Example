using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Actor
{
    // Start is called before the first frame update
    void Awake()
    {
        lastPosition = transform.position;
        nextPosition = lastPosition;
        t = 0;
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMyTurn)
        {
            Move();
            if (!isMoving)
                Attack();
        }
    }

    override protected void Move()
    {
        if (!isMoving)
        {
            t = 0; // reset movement
            if (Input.GetKeyDown(KeyCode.A))
            {
                // move left
                nextPosition = transform.position + Vector3.left;
                if (!IsPositionOccupied(nextPosition)) 
                { 
                    isMoving = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // move right
                nextPosition = transform.position + Vector3.right;
                if (!IsPositionOccupied(nextPosition))
                {
                    isMoving = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                // skip turn
                gameController.ChangeTurn();
            }
        }
        else
        {
            MoveToTarget();
        }
    }

    protected override void Attack()
    {
        if (!isAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isAttacking = true;
            }
        }
        else
        {
            isAttacking = false;
            DealDamage(attack);
            gameController.ChangeTurn();
        }
    }
}
