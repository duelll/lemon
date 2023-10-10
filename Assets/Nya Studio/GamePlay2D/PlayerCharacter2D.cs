using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace meow.GamePlay2D
{

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacter2D : CharacterController2D
{
    public float speed = 10.0f;
    public float collisionTestOffset = 0.5f;

    public SpriteRenderer spriteRenderer;
    public Animator animator;


    private Rigidbody2D m_rigidbody;

    void Start()
    {
        m_rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        isTouchingGround = IsTouchingGround();
        Vector2 motion = m_rigidbody.velocity;

        if (xInput != 0.0f)
        {
            if (!TestMove(Vector2.right, collisionTestOffset) && xInput > 0.0f)
            {
                motion.x = -xInput * (speed * 0.01f);
            }
            else if (!TestMove(Vector2.left, collisionTestOffset) && xInput < 0.0f)
            {
                motion.x = -xInput * (speed * 0.01f);
            }
            else 
            {
            motion.x = xInput * speed;
            }
        }    

        if (Input.GetAxis("Jump") > 0.0f && isTouchingGround)
        {
            motion.y = speed + 2.5f;
        }

        if (animator != null)
        {
            animator.SetFloat("SpeedX", Mathf.Abs(motion.x));
            animator.SetFloat("SpeedY", motion.y);
            animator.SetBool("Grounded", isTouchingGround);
        } 

        if (spriteRenderer != null)
        {
            if (xInput > -0.01f)
            {
                spriteRenderer.flipX = false;
            }
            if (xInput < -0.01f)
            {
                spriteRenderer.flipX = true;
            }

        }

        m_rigidbody.velocity = motion;
    }
}

}