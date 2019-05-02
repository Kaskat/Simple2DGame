using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OldSript
{
    public class PlayerController : MonoBehaviour
    {

        public float jumpVelocity;
        public float bounceVelocity;
        public Vector2 velocity;
        public float gravity = 10;
        public bool isDebug;

        public float offsetY = 0.5f;

        public LayerMask wallMask;
        public LayerMask floorMask;

        bool facingRight;
        bool grounded;
        bool bounce;

        Vector3 localScale;
        Vector3 posPlayer;

        float horizontal;
        bool jump;

        public VariableJoystick joystick;
        public JoyButton joybutton;

        public float countCoin = 0;
        public Text textcoin;

        public enum PlayerState
        {
            jumping,
            idle,
            walking,
            bouncing
        }

        private PlayerState playerState = PlayerState.idle;

        void Start()
        {
            localScale = transform.localScale;
            posPlayer = transform.localPosition;


            //Fall();
        }

        public void CoinUp()
        {
            countCoin++;
            textcoin.text = "X0" + countCoin.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            
            /*
            if (joystick != null && joybutton != null) 
            {
                horizontal = joystick.Horizontal;
                Debug.Log(horizontal);
                jump = joybutton.pressed;
            }
            */
            horizontal = Input.GetAxis("Horizontal");
            jump = Input.GetKeyDown(KeyCode.Space);

            //Debug.Log(horizontal);
            //if (Input.GetKeyDown(KeyCode.Alpha0))
            //{
            //    isDebug = !isDebug;
            //}

            if (horizontal != 0)
            {
                //Debug.Log("YEAH");
                posPlayer = Move(horizontal);
            }

            if (jump && playerState != PlayerState.jumping)
            {
                Debug.Log(playerState);
                playerState = PlayerState.jumping;
                velocity = new Vector2(velocity.x, jumpVelocity);
                
            }

            if (playerState == PlayerState.jumping)
            {
                posPlayer.y += velocity.y * Time.deltaTime;
                velocity.y -= gravity * Time.deltaTime;
            }

            if (bounce && playerState != PlayerState.bouncing)
            {
                playerState = PlayerState.bouncing;

                velocity = new Vector2(velocity.x, bounceVelocity);
            }

            if (playerState == PlayerState.bouncing)
            {
                posPlayer.y += velocity.y * Time.deltaTime;

                velocity.y -= gravity * Time.deltaTime;
            }

            if (velocity.y <= 0)
                posPlayer = CheckFloorRays(posPlayer);

            if (velocity.y > 0)
                posPlayer = CheckCeilingRays(posPlayer);

            transform.localPosition = posPlayer;

            UpdateAnimationState();
        }

        void UpdateAnimationState()
        {
            if (grounded && horizontal == 0 && !bounce)
            {
                GetComponent<Animator>().SetBool("IsJumping", false);
                GetComponent<Animator>().SetBool("IsRunning", false);
                GetComponent<Animator>().SetBool("Ground", true);
            }

            if (grounded && (horizontal > 0 || horizontal < 0))
            {
                GetComponent<Animator>().SetBool("IsJumping", false);
                GetComponent<Animator>().SetBool("IsRunning", true);
                GetComponent<Animator>().SetBool("Ground", true);
            }

            if (playerState == PlayerState.jumping)
            {
                GetComponent<Animator>().SetBool("IsJumping", true);
                GetComponent<Animator>().SetBool("IsRunning", false);
                GetComponent<Animator>().SetBool("Ground", false);
            }

        }

        public Vector3 Move(float hor)
        {
            if (hor < 0 && !facingRight)
            {
                FlipPlayer();
            }
            else if (hor > 0 && facingRight)
            {
                FlipPlayer();
            }

            if (CheckWallRays(posPlayer, localScale.x)) posPlayer.x += velocity.x * hor * Time.deltaTime;
            //posPlayer = CheckWallRays(posPlayer, localScale.x);
            return new Vector3(posPlayer.x, transform.localPosition.y, transform.localPosition.z);
            // Debug.Log(posPlayer.x);
        }

        private void FlipPlayer()
        {
            facingRight = !facingRight;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        bool CheckWallRays(Vector3 pos, float direction)
        {
            Vector2 originTop = new Vector2(pos.x + direction * .4f, pos.y + 0.8f - .2f);
            Vector2 originMiddle = new Vector2(pos.x + direction * .4f, pos.y);
            Vector2 originBottom = new Vector2(pos.x + direction * .4f, pos.y - 0.6f + .2f);

            RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
            RaycastHit2D wallMiddle = Physics2D.Raycast(originMiddle, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
            RaycastHit2D wallBottom = Physics2D.Raycast(originBottom, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);

            if (isDebug)
            {
                Debug.DrawRay(originTop, new Vector2(direction, 0) * velocity.x * Time.deltaTime, Color.red);
                Debug.DrawRay(originMiddle, new Vector2(direction, 0) * velocity.x * Time.deltaTime, Color.red);
                Debug.DrawRay(originBottom, new Vector2(direction, 0) * velocity.x * Time.deltaTime, Color.red);
            }

            /* if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null) {
              return false;
             }  else  return true;*/

            return (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null) ? false : true;
        }


        Vector3 CheckFloorRays(Vector3 pos)
        {
            Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y - 0.6f);
            Vector2 originMiddle = new Vector2(pos.x, pos.y - 0.6f);
            Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y - 0.6f);

            RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
            RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
            RaycastHit2D floorRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);


            if (floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null)
            {
                //Debug.Log("Hit");
                RaycastHit2D hitRay = floorRight;

                if (floorLeft)
                {
                    hitRay = floorLeft;
                }
                else if (floorMiddle)
                {
                    hitRay = floorMiddle;
                }
                else if (floorRight)
                {
                    hitRay = floorRight;
                }

                if (hitRay.collider.tag == "Enemy")
                {
                    bounce = true;

                    if (hitRay.collider.tag != "Sundew") hitRay.collider.GetComponent<EnemyMushroomAI>().Crush();
                }

                playerState = PlayerState.idle;
                grounded = true;
                velocity.y = 0;

                pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + 0.5f;
                //Debug.Log(hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + 1);
            }
            else
            {
                if (playerState != PlayerState.jumping)
                {
                    Fall();
                }
            }
            return pos;
        }


        Vector3 CheckCeilingRays(Vector3 pos)
        {
            Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y + 0.6f);
            Vector2 originMiddle = new Vector2(pos.x, pos.y + 0.6f);
            Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y + 0.6f);

            RaycastHit2D ceilLeft = Physics2D.Raycast(originLeft, Vector2.up, velocity.y * Time.deltaTime, floorMask);
            RaycastHit2D ceilMiddle = Physics2D.Raycast(originMiddle, Vector2.up, velocity.y * Time.deltaTime, floorMask);
            RaycastHit2D ceilRight = Physics2D.Raycast(originRight, Vector2.up, velocity.y * Time.deltaTime, floorMask);

            if (ceilLeft.collider != null || ceilMiddle.collider != null || ceilRight.collider != null)
            {
                RaycastHit2D hitRay = ceilRight;

                if (ceilLeft)
                {
                    hitRay = ceilLeft;
                }
                else if (ceilMiddle)
                {
                    hitRay = ceilMiddle;
                }
                else if (ceilRight)
                {
                    hitRay = ceilRight;
                }

                if (hitRay.collider.tag == "BounceBlock")
                {
                    hitRay.collider.GetComponent<TouchBlock>().BlockBounce();
                }

                pos.y  = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 - 1.5f;

                Fall();
            }
            return pos;
        }

        void Fall()
        {
            velocity.y = 0;
            playerState = PlayerState.jumping;
            grounded = false;
            bounce = false;
        }

    }
}