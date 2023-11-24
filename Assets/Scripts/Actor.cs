using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public float moveSpeed;
    public int hp = 10;
    public int attack = 1;

    protected bool isMyTurn = false;
    protected bool isMoving = false;
    protected bool isAttacking = false;
    protected Vector3 lastPosition;
    protected Vector3 nextPosition;
    protected float t; // movement offset between last and next

    protected GameController gameController;

    abstract protected void Move();
    abstract protected void Attack();

    virtual protected void MoveToTarget()
    {
        if (t != 1)
        {
            // change movement between last and next
            t += Time.deltaTime * moveSpeed;
            t = Mathf.Clamp01(t);

            transform.position = Vector3.Lerp(lastPosition, nextPosition, t);
        }
        else
        {
            t = 0;
            lastPosition = transform.position;
            nextPosition = lastPosition;
            isMoving = false;
            gameController.ChangeTurn();
        }
    }

    virtual protected void DealDamage(int damage, int squareOffset = 1)
    {
        Collider2D left = Physics2D.OverlapCircle(transform.position + Vector3.left * squareOffset, .1f);
        Collider2D right = Physics2D.OverlapCircle(transform.position + Vector3.right * squareOffset, .1f);

        if (left != null)
        {
            Actor actor;
            if (left.gameObject.TryGetComponent<Actor>(out actor))
            {
                actor.TakeDamage(damage);
            }
        }
        if (right != null)
        {
            Actor actor;
            if (right.gameObject.TryGetComponent<Actor>(out actor))
            {
                actor.TakeDamage(damage);
            }
        }
    }

    virtual protected void TakeDamage(int damage)
    {
        hp -= damage;
    }

    virtual protected bool IsPositionOccupied(Vector3 pos)
    {
        Collider2D c = Physics2D.OverlapCircle(pos, .1f);
        if (c)
            return true;
        return false;
    }

    public void SetTurn(bool takeTurn)
    {
        isMyTurn = takeTurn;
    }
}
