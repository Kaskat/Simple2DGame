using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OldSript
{
    public class PlayerControllerRB : MonoBehaviour
    {

        Rigidbody2D rb2;

        public int playerSpeed = 10;
        public int playerJumPower = 1250;

        bool facingRight;

        float horizontal = 0;
        //float vertical = 0;


        void Start()
        {
            rb2 = GetComponent<Rigidbody2D>();
        }
        // Update is called once per frame
        void Update()
        {
            PlayerMove();

        }

        void PlayerMove()
        {
            horizontal = Input.GetAxis("Horizontal");

            if (Input.GetButton("Jump"))
            {
                Jump();
            }

            if (horizontal < 0 && !facingRight)
            {
                FlipPlayer();
            }
            else if (horizontal > 0 && facingRight)
            {
                FlipPlayer();
            }

            rb2.velocity = new Vector2(horizontal * playerSpeed, rb2.velocity.y);
        }

        void FlipPlayer()
        {
            facingRight = !facingRight;
            Vector2 localScale = gameObject.transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        void Jump()
        {
            rb2.AddForce(Vector2.up * playerJumPower);
        }
    }
}
