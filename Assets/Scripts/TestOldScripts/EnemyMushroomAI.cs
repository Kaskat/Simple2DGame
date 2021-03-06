﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OldSript
{
    public class EnemyMushroomAI : MonoBehaviour
    {


        public float gravity;
        public Vector2 velocity;
        public bool isWalkLeft = true;

        public LayerMask groundMask;
        public LayerMask wallMask;

        bool grounded;
        bool shouldDie;

        float deathTimer = 0;
        float timeBeforeDestroy = 2.0f;

        private enum EnemyState
        {
            walking,
            falling,
            dead
        }

        EnemyState state = EnemyState.falling;
        // Use this for initialization
        void Start()
        {

            Fall();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateEnemyPosition();

            CheckCrushed();
        }

        public void Crush()
        {
            state = EnemyState.dead;

            GetComponent<Animator>().SetBool("isCrushed", true);

            GetComponent<Collider2D>().enabled = false;

            shouldDie = true;
        }

        void CheckCrushed()
        {
            if (shouldDie)
            {
                if (deathTimer <= timeBeforeDestroy)
                {
                    deathTimer += Time.deltaTime;
                }
                else
                {
                    shouldDie = false;

                    Destroy(this.gameObject);
                }
            }
        }

        private void UpdateEnemyPosition()
        {
            if (state != EnemyState.dead)
            {
                Vector3 pos = transform.localPosition;
                Vector3 scale = transform.localScale;

                if (state == EnemyState.falling)
                {
                    pos.y += velocity.y * Time.deltaTime;
                    velocity.y -= gravity * Time.deltaTime;
                }

                if (state == EnemyState.walking)
                {
                    // Debug.Log("Walk left");
                    if (isWalkLeft)
                    {
                        pos.x -= velocity.x * Time.deltaTime;
                        scale.x = -1;
                    }
                    else
                    {
                        // Debug.Log("Walk right");
                        pos.x += velocity.x * Time.deltaTime;
                        scale.x = 1;
                    }
                }

                if (velocity.y <= 0)
                    pos = CheckGround(pos);

                CheckWalls(pos, scale.x);

                transform.localPosition = pos;
                transform.localScale = scale;
            }
        }

        Vector3 CheckGround(Vector3 pos)
        {
            Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y - 0.6f);
            Vector2 originMiddle = new Vector2(pos.x, pos.y - 0.6f);
            Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y - 0.6f);

            RaycastHit2D groundLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, groundMask);
            RaycastHit2D groundMiddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, groundMask);
            RaycastHit2D groundRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, groundMask);

            Debug.DrawRay(originLeft, Vector2.down, Color.red);
            Debug.DrawRay(originMiddle, Vector2.down, Color.red);
            Debug.DrawRay(originRight, Vector2.down, Color.red);

            if (groundLeft.collider != null || groundMiddle.collider != null || groundRight.collider != null)
            {
                RaycastHit2D hitRay = groundLeft;

                if (groundLeft)
                {
                    hitRay = groundLeft;
                }
                else if (groundMiddle)
                {
                    hitRay = groundMiddle;
                }
                else if (groundRight)
                {
                    hitRay = groundRight;
                }

                pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + 0.5f;

                // Debug.Log(hitRay.collider.name);

                grounded = true;

                velocity.y = 0;

                state = EnemyState.walking;
            }
            else
            {
                if (state != EnemyState.falling)
                {
                    Fall();
                }
            }
            return pos;
        }

        void CheckWalls(Vector3 pos, float direction)
        {
            Vector2 originTop = new Vector2(pos.x + direction + 0.4f, pos.y + 0.5f - 0.2f);
            Vector2 originMiddle = new Vector2(pos.x + direction + 0.4f, pos.y);
            Vector2 originBottom = new Vector2(pos.x + direction + 0.4f, pos.y - 0.5f + 0.2f);

            RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
            RaycastHit2D wallMiddle = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
            RaycastHit2D wallBottom = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);


            if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null)
            {
                RaycastHit2D hitRay = wallTop;

                if (wallTop)
                {
                    hitRay = wallTop;
                }
                else if (wallMiddle)
                {
                    hitRay = wallMiddle;
                }
                else if (wallBottom)
                {
                    hitRay = wallBottom;
                }

                if (hitRay.collider.tag == "Player")
                {
                    Debug.Log("Dead");
                    Application.LoadLevel("MarioTestSprite");
                }

                isWalkLeft = !isWalkLeft;

            }
        }

        private void OnBecameVisible()
        {
            enabled = true;
        }

        void Fall()
        {
            velocity.y = 0;

            state = EnemyState.falling;

            grounded = false;
        }
    }
}