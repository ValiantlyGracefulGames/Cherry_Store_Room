using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveDir;
    private Vector2 lastMoveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveDir = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) moveDir = Vector2.up;
        else if (Input.GetKey(KeyCode.S)) moveDir = Vector2.down;
        else if (Input.GetKey(KeyCode.A)) moveDir = Vector2.left;
        else if (Input.GetKey(KeyCode.D)) moveDir = Vector2.right;

        // Save last direction ONLY when moving
        if (moveDir != Vector2.zero)
        {
            lastMoveDir = moveDir;
        }

        // Animator parameters
        animator.SetBool("IsMoving", moveDir != Vector2.zero);
        animator.SetFloat("MoveX", lastMoveDir.x);
        animator.SetFloat("MoveY", lastMoveDir.y);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }
}