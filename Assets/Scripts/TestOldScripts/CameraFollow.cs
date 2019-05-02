using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace OldSript
{
    public class CameraFollow : MonoBehaviour
    {

        Transform player;
        [SerializeField] Transform leftBounds;
        [SerializeField] Transform rightBounds;

        public float right, left;

        public float smoothDampTime = 0.15f;

        Vector3 smoothDampVelocity = Vector3.zero;

        float camWidth, camHeight, levelMinX, levelMaxX;

        void Start()
        {

            player = GameObject.FindGameObjectWithTag("Player").transform;

            camHeight = Camera.main.orthographicSize * 2;
            camWidth = camHeight * Camera.main.aspect;

            //  Debug.Log(camWidth);

            float leftBoundsWidth = leftBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
            float rightBoundsWidth = rightBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
            //Debug.Log(rightBoundsWidth);

            levelMinX = leftBounds.position.x + leftBoundsWidth + (camWidth / 3) + right;
            levelMaxX = rightBounds.position.x - rightBoundsWidth - (camWidth / 2) - left;
            //Debug.Log("Max: " + levelMaxX + "Min: " + levelMinX);
        }

        // Update is called once per frame
        void Update()
        {

            if (player)
            {

                float targetX = Mathf.Max(levelMinX, Mathf.Min(levelMaxX, player.position.x));

                float x = Mathf.SmoothDamp(transform.position.x, targetX, ref smoothDampVelocity.x, smoothDampTime);

                transform.position = new Vector3(x, transform.position.y, transform.position.z);
            }
        }
    }
}