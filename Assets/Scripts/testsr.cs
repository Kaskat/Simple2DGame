using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testsr : MonoBehaviour {

    public float minimum = 10.0F;
    public float maximum = 20.0F;
    public float duration = 5.0F;
    private float startTime;
    bool pause;
    void Start()
    {
        startTime = Time.time;
    }
    void Update()
    {
            //float t = (Time.time - startTime) / duration;
        //Debug.Log("St "+ startTime + "Time " + Time.time + "T " + t);
        if (Input.anyKey)
        {
            pause = !pause;
        }

        if (pause) {
            float t = Time.time / duration;
            transform.position = new Vector3(Mathf.SmoothStep(minimum, maximum, t), 0, 0);
        }
    }
}
