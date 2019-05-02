using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldSript
{
    public class SimpleCamera : MonoBehaviour
    {

        public float speedFollowCamera = 5f;

        Transform playerTransform;
        Transform camTransform;
        Vector3 offset;

        // Use this for initialization
        void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            camTransform = Camera.main.transform;
            offset = camTransform.position - playerTransform.position;
        }

        void LateUpdate()
        {
            camTransform.position = playerTransform.position + offset;
        }
    }
}
