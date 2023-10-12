using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace meow.GamePlay2D
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCharacter2D : CharacterController2D
    {
        public float speed = 10.0f;
        public float collisionTestOffset = 0.5f;
        public int damageAmount = 0;
        public float jumpAmount = 30f;

        public bool isCrouching = false;
        
        private bool canDoubleJump = true;

        public SpriteRenderer spriteRenderer;
        public Animator animator;

        public UnityEvent<int> damagePlayer = new UnityEvent<int>();


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

            if (isTouchingGround)
            {
                canDoubleJump = true;
            }

            if (Input.GetKeyDown("space") == true && (isTouchingGround || canDoubleJump))
            {
                motion.y = jumpAmount;
                if (!isTouchingGround)
                {
                    canDoubleJump = false;
                }
            }

            if ((Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) && isTouchingGround)
            {
                isCrouching = true;
                motion.x *= 0.1f;
            }
            else
            {
                isCrouching = false;
            }

            if (animator != null)
            {
                animator.SetFloat("SpeedX", Mathf.Abs(motion.x));
                animator.SetFloat("SpeedY", motion.y);
                animator.SetBool("Grounded", isTouchingGround);
                animator.SetBool("Crouching", isCrouching);
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "spike")
            {
                damagePlayer.Invoke(damageAmount);
            }
        }

        public void reloadScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }

}