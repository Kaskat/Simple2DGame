using UnityEngine;
using System.Collections;

public class OverlordPlayer : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    [Range(0f, 1f)]
    [SerializeField]
    private float _fadeSpeed;
    [SerializeField]
    private float _gravityForce;
    [SerializeField]
    private SpriteRenderer _chatacterSprite;

    private BoxCollider2D _collider;


    private bool _lookingRight;

    private float _targetSpeed;
    private float _currentSpeed;
    
    private void Start()
    {
        _lookingRight = true;
    }

    private void Update()
    {
        PlayerInput();
        Move();
        AddGravtiy();
    }

    private void AddGravtiy()
    {
        var playerPos = transform.position;
        
        playerPos.y -= _gravityForce * Time.deltaTime;

        transform.position = playerPos;
    }

    private void Move()
    {
        var playerPos = transform.position;

        _currentSpeed = Mathf.SmoothStep(_currentSpeed, _targetSpeed, _fadeSpeed);
        playerPos.x += _currentSpeed * _moveSpeed * Time.deltaTime;

        transform.position = playerPos;

        if(_currentSpeed > 0 && !_lookingRight)
        {
            FlipPlayer();
        }
        else if(_currentSpeed < 0 && _lookingRight)
        {
            FlipPlayer();
        }
    }
    
    private void PlayerInput()
    {
        _targetSpeed = 0;

        if (Input.GetKey(KeyCode.A))
        {
            _targetSpeed = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _targetSpeed = 1f;
        }
        if (Input.GetKey(KeyCode.Space))
        {

        }
    }

    private void FlipPlayer()
    {
        _lookingRight = !_lookingRight;

        _chatacterSprite.flipX = !_lookingRight;
    }
}
