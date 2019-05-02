using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 7;
    public float speedJump = 1;
    public float jumpForce = 2;
    public float gravity = 7;
    public float speedModifyUp = 1;
    public float speedModifyDown = 1;
    public float jumpModifyUp = 1;
    public float jumpModifyDown = 1; // gravity

    public float rayLength = 0.2f;

    float horizontal = 0, vertical = 0;

    public Vector2 velocity = Vector2.zero;

    Vector3 playerPos;
    Vector3 playerScale;

 

    BoxCollider2D collider;
    RaycastHit2D hitRay;

    bool facingRight;
    bool isGround;
    bool isRun;
    bool isJump;
    bool isJumpToo;

    public enum PlayerState
    {
        jumping,
        idle,
        walking,
        bouncing
    }

    private PlayerState playerState = PlayerState.idle;

    private void Start()
    {
        playerPos = transform.localPosition;
        playerScale = transform.localScale;
        collider = GetComponent<BoxCollider2D>();

        //Fall();
        //Debug.Log("Size: " + collider.bounds.size);
    }

    private void Update()
    {
        PlayerInput();

        CalckVelocityX();
        if (isRun || (velocity.x >= -1 || velocity.x <= 1) && velocity.x != 0) // 
        {
            playerState = PlayerState.walking;
            playerPos.x += speed * velocity.x * Time.deltaTime;

            if (CheckSide(new Vector2(playerScale.x, 0), out hitRay))
            {
                playerPos.x = hitRay.collider.bounds.center.x - (hitRay.collider.bounds.size.x / 2 + collider.bounds.size.x / 2) * playerScale.x;
                velocity.x = 0;
            }
        }

        if (velocity.x != 0 && CheckSide(-playerScale.x, 0)) //(velocity.x >= -1 || velocity.x <= 1)
        {
            velocity.x = 0;
        }


        Debug.Log(isJump +"  " +playerState);
        if (isJump && playerState != PlayerState.jumping && isGround)
        {
            playerState = PlayerState.jumping;
            velocity.y = jumpForce;
            isGround = false;
        }


        if (!isGround || playerState == PlayerState.jumping || playerState == PlayerState.walking)
        {
            if (isJumpToo && velocity.y > 0)
            {
                velocity.y += jumpModifyUp * Time.deltaTime;
                isJump = false;
            }
            playerPos.y += velocity.y* speedJump * Time.deltaTime;
            velocity.y -= gravity * Time.deltaTime;
        }

        if(velocity.y < 0 && CheckSide(Vector2.down))
        {
                velocity.y = 0;
                isGround = true;
                playerPos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + collider.bounds.size.y / 2;
                if (velocity.x == 0) playerState = PlayerState.idle;
        }

        if(velocity.y > 0)
        {
            if (CheckSide(Vector2.up))
            {
                velocity.y = 0;
                playerPos.y = hitRay.collider.bounds.center.y - hitRay.collider.bounds.size.y / 2 - collider.bounds.size.y / 2;
            }
        }

        //playerPos.x = PlayerInput().x;
        transform.localPosition = playerPos;
    }
    
    void PlayerInput() {
        horizontal = Input.GetAxisRaw("Horizontal"); // Input method

        if (horizontal < 0 && !facingRight)
        {
            FlipPlayer();
        }
        else if (horizontal > 0 && facingRight)
        {
            FlipPlayer();
        }

        //Run
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            isRun = true;
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            isRun = false;
        }

        /*if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            isRun = true;
        }*/

        //Jump
        if (Input.GetKeyDown(KeyCode.K))
        {
            isJump = true;
        }

        if (Input.GetKey(KeyCode.K))
        {
            isJumpToo = true;
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            isJump = false;
            isJumpToo = false;
        }
    }

    void CalckVelocityY()
    {
        

    }

    void CalckVelocityX()
    {
        if (isRun && playerScale.x == 1)
        {
            if (velocity.x < 1) velocity.x += speedModifyUp * Time.deltaTime;
        }

        if (isRun && playerScale.x == -1)
        {
            if (velocity.x > -1) velocity.x -= speedModifyUp * Time.deltaTime;
        }

        if (!isRun && velocity.x > 0 && velocity.x <= 1)
        {
            velocity.x -= speedModifyDown * Time.deltaTime;
            if (velocity.x < 0.001) velocity.x = 0;
        }

        if (!isRun && velocity.x < 0 && velocity.x >= -1)
        {
            velocity.x += speedModifyDown * Time.deltaTime;
            //if (velocity.x < -0.0001) velocity.x = 0;
        }

        velocity.x = Mathf.Clamp(velocity.x, -1, 1);
    }

    private void FlipPlayer()
    {
        facingRight = !facingRight;
        playerScale.x *= -1;
        transform.localScale = playerScale;
        //Debug.Log("Turn");
    }
    
    //public void Fall()
    //{
    //    isGround = false;
    //}

    //public Vector3 test(float vert)
    //{
    //    if (vert > 0)
    //    {
    //        if (CheckSide(Vector2.up, out hitRay))
    //        {
    //            playerPos.y += speed * vert * Time.deltaTime;
    //        }
    //        else
    //        {
    //            playerPos.y = hitRay.collider.bounds.center.y - hitRay.collider.bounds.size.y / 2 - collider.bounds.size.y / 2;
    //        }
    //    }
    //    else if (vert < 0)
    //    {
    //        if (CheckSide(Vector2.down, out hitRay))
    //        {
    //            playerPos.y += speed * vert * Time.deltaTime;
    //        }
    //        else
    //        {
    //            playerPos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + collider.bounds.size.y / 2;
    //        }
    //    }

    //    return new Vector3(transform.localPosition.x, playerPos.y, transform.localPosition.z);
    //}
    
    public bool CheckSide(Vector2 dir, out RaycastHit2D ray)
    {
        Vector2 originMiddle = new Vector2(transform.position.x + collider.size.x / 1.9f * dir.x,
                                           transform.position.y + collider.size.y / 1.9f * dir.y);// 1.9f -_-

        Vector2 originLeft = originMiddle + collider.size / 2.1f * new Vector2(dir.y, -dir.x); //перпендикуляр
        Vector2 originRight = originMiddle + collider.size / 2.1f * new Vector2(-dir.y, dir.x); 

        RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, dir, rayLength); // velocity * Time.deltaTime
        RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, dir, rayLength);
        RaycastHit2D floorRight = Physics2D.Raycast(originRight, dir, rayLength);
        
        Debug.DrawRay(originMiddle, dir * rayLength, Color.red);
        Debug.DrawRay(originLeft, dir * rayLength, Color.red);
        Debug.DrawRay(originRight, dir * rayLength, Color.red);

        //Debug.Log(originLeft + "|" + originMiddle + "|" + originRight);
        

        ray = floorLeft;

        if (floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null)
        {

            //ray = floorLeft;

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

            //Debug.Log(hitRay.collider.name);
            return true;
        }

        return false;
    }

    public bool CheckSide(Vector2 dir)
    {
        Vector2 originMiddle = new Vector2(transform.position.x + collider.size.x / 1.9f * dir.x, // 1.9f -_-
                                           transform.position.y + collider.size.y / 1.9f * dir.y);
        Vector2 originLeft = originMiddle + collider.size / 2.1f * new Vector2(dir.y, -dir.x); //перпендикуляр
        Vector2 originRight = originMiddle + collider.size / 2.1f * new Vector2(-dir.y, dir.x);

        RaycastHit2D rayLeft = Physics2D.Raycast(originLeft, dir, rayLength);
        RaycastHit2D rayMiddle = Physics2D.Raycast(originMiddle, dir, rayLength);
        RaycastHit2D rayRight = Physics2D.Raycast(originRight, dir, rayLength);

        Debug.DrawRay(originMiddle, dir * rayLength, Color.red);
        Debug.DrawRay(originLeft, dir * rayLength, Color.red);
        Debug.DrawRay(originRight, dir * rayLength, Color.red);

        //Debug.Log(originLeft + "|" + originMiddle + "|" + originRight);


        if (rayLeft.collider != null || rayMiddle.collider != null || rayRight.collider != null)
        {

            RaycastHit2D ray = rayLeft;

            if (rayLeft)
            {
                hitRay = rayLeft;
            }
            else if (rayMiddle)
            {
                hitRay = rayMiddle;
            }
            else if (rayRight)
            {
                hitRay = rayRight;
            }

            //Debug.Log(hitRay.collider.name);
            return true;
        }

        return false;
    }

    public bool CheckSide(float x, float y)
    {
        Vector2 originMiddle = new Vector2(transform.position.x + collider.size.x / 1.9f * x, transform.position.y + collider.size.y / 1.9f * y);

        RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, new Vector2(x, y), rayLength / 2f); //07 -_-

        Debug.DrawRay(originMiddle, new Vector2(x, y) * rayLength/2, Color.red);
   
        if (floorMiddle.collider != null)
        {
            Debug.Log(floorMiddle.collider.name);
            return true;
        }

        return false;
    }
}
