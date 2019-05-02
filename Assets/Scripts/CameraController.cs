using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Transform playerPos;
    Vector3 cameraPos;
    Vector3 oldPos;
    public float speedPlayer = 8f; //speed mario
    bool check;

    public float distanceToCenter;

    void Awake()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        //cameraPos = transform.localPosition;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        cameraPos = transform.position;
        distanceToCenter = cameraPos.x - playerPos.position.x;

        //Debug.Log("Player pos.x: " + playerPos.position.x + "Camera pos.x: " + cameraPos.x + "Distant: " + distanceToCenter);
    }

    private void LateUpdate()
    {
        if(Input.GetAxis("Horizontal") > 0.5f) { // Узнаём в какую сторону идёт игрок,и не стоит ли он на месте
                                                 
        if (distanceToCenter > 0 && distanceToCenter < 0.2f) 
            {
                transform.position += Vector3.right * speedPlayer * Time.deltaTime;
            }
            else if (distanceToCenter <= 2.5)
            {
                transform.position += Vector3.right * speedPlayer * 0.8f * Time.deltaTime;
            }
        }
    }


}
