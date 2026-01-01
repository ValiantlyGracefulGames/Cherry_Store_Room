using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDir = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) moveDir = Vector2.up;
        else if (Input.GetKey(KeyCode.S)) moveDir = Vector2.down;
        else if (Input.GetKey(KeyCode.A)) moveDir = Vector2.left;
        else if (Input.GetKey(KeyCode.D)) moveDir = Vector2.right;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }
}