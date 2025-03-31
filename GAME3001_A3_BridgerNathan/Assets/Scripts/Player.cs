using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 4.0f;
    bool isFacingLeft = true;
    float jumpPower = 15.0f;
    bool isJumping = false;
    
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();

        if(Input.GetKeyDown(KeyCode.W) && !isJumping) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = true;
        }

        Vector2 rayDirection = new Vector2(horizontalInput, 0);
        Debug.DrawRay(transform.position, rayDirection * 2, Color.green);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y); 
    }

    void FlipSprite()
    {
        if (isFacingLeft && horizontalInput < 0f || !isFacingLeft && horizontalInput > 0f)
        {
            isFacingLeft = !isFacingLeft;
            Vector3 direction = transform.localScale;
            direction.x *= -1f;
            transform.localScale = direction;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false; 
    }
}
