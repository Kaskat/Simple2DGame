using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldSript
{
    public class Trash : MonoBehaviour
    {

        public float jumpVelocity;
        public float gravity;
        public Vector2 velocity;

        public LayerMask floorMask;

        private bool jump, grounded;

        public enum PlayerState { jumping, idle, walking };

        private PlayerState playerState = PlayerState.idle;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            Chk();
            Up();

        }

        private void Chk()
        {
            jump = Input.GetKeyDown(KeyCode.Space);
        }

        private void Up()
        {
            Vector3 pos = transform.localPosition;
            Vector3 scale = transform.localScale;

            if (jump && playerState != PlayerState.jumping)
            {
                playerState = PlayerState.jumping;

                velocity = new Vector2(velocity.x, jumpVelocity);
            }

            if (playerState == PlayerState.jumping)
            {
                pos.y += velocity.y * Time.deltaTime;

                velocity.y -= gravity * Time.deltaTime;
            }

            if (velocity.y <= 0)
                pos = CheckFloorRays(pos);


            transform.localPosition = pos;
            transform.localScale = scale;
        }


        void Fall()
        {
            velocity.y = 0;
            playerState = PlayerState.jumping;
            grounded = false;

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
                Debug.Log("Hit");
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
    }
}
