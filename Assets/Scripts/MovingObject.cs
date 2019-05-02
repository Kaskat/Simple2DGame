using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {

    BoxCollider2D collider;

    LayerMask mask;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        
    }

    private void Start()
    {
        Debug.Log("Collider Center : " + collider.bounds.center);
        Debug.Log("Collider Size : " + collider.bounds.size);
        Debug.Log("Collider bound Minimum : " + collider.bounds.min);
        Debug.Log("Collider bound Maximum : " + collider.bounds.max);
    }

    //public bool CheckSide(Vector3 dir)
    //{
    //    Vector2 originLeft = new Vector2(transform.position.x - 0.5f + 0.2f, transform.position.y - 0.6f);
    //    Vector2 originMiddle = new Vector2(transform.position.x, transform.position.y - 0.6f);
    //    Vector2 originRight = new Vector2(transform.position.x + 0.5f - 0.2f, transform.position.y - 0.6f);

    //    RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, dir, distance, mask);
    //    RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, dir, distance, mask);
    //    RaycastHit2D floorRight = Physics2D.Raycast(originRight, dir, distance, mask);


    //    if (floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null)
    //    {

    //        RaycastHit2D hitRay = floorRight;

    //        if (floorLeft)
    //        {
    //            hitRay = floorLeft;
    //        }
    //        else if (floorMiddle)
    //        {
    //            hitRay = floorMiddle;
    //        }
    //        else if (floorRight)
    //        {
    //            hitRay = floorRight;
    //        }
    //    }

    //    return true;
    //}

}
