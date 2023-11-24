using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyController : Actor
{
    public int minDistance;
    public int maxDistance;
    public bool shouldAttackNextTurn;
    PlayerController player;

    // Start is called before the first frame update
    void Awake()
    {
        lastPosition = transform.position;
        nextPosition = lastPosition;
        t = 0;
        gameController = FindObjectOfType<GameController>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMyTurn && !shouldAttackNextTurn)
            Move();
        if (isMyTurn)
            Attack();
    }

    void SetNextPosition(Vector3 dir)
    {
        nextPosition = transform.position + dir;
        if (!IsPositionOccupied(nextPosition))
            isMoving = true;
        else
            gameController.ChangeTurn();
    }

    void SetShouldAttack()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) >= minDistance &&
            Mathf.Abs(player.transform.position.x - transform.position.x) <= maxDistance)
        {
            shouldAttackNextTurn = true;
        }
        else
            shouldAttackNextTurn = false;
    }


    protected override void Move()
    {
        shouldAttackNextTurn = false;

        if (!isMoving)
        {
            if (player.transform.position.x < transform.position.x)
            {
                if (player.transform.position.x < transform.position.x - maxDistance)
                {
                    SetNextPosition(Vector3.left);
                }
                else if (player.transform.position.x > transform.position.x - minDistance)
                {
                    SetNextPosition(Vector3.right);
                }
                else
                {
                    shouldAttackNextTurn = true;
                    gameController.ChangeTurn();
                }
            }
            else
            {
                if (player.transform.position.x > transform.position.x + maxDistance)
                {
                    SetNextPosition(Vector3.right);
                }
                else if (player.transform.position.x < transform.position.x + minDistance)
                {
                    SetNextPosition(Vector3.left);
                }
                else
                {
                    shouldAttackNextTurn = true;
                    gameController.ChangeTurn();
                }
            }
        }
        else
        {
            // only check after we're done moving
            if (t == 1)
            {
                SetShouldAttack();
            }
            MoveToTarget();
        }

    }

    protected override void Attack()
    {
        if (shouldAttackNextTurn)
        {
            for (int i = minDistance; i <= maxDistance; i++)
            {
                DealDamage(attack, i);
            }
            SetShouldAttack();
            gameController.ChangeTurn();
            isAttacking = false;
        }
    }
}
