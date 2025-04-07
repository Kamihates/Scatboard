using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rgbd2d;
    private SpriteRenderer _spriteRend;
    private BoxCollider2D _boxColl;
    private Animator _animator;

    [Header("Speed")]
    public float maxSpeed = 5f;
    public float acceleration = 2f;
    private float _speed = 0f;
    private Vector2 _previousPosition;

    [Header("Jump")]
    public float jumpingPower;
    [HideInInspector]public LayerMask groundLayer;
    [HideInInspector]public Transform groundCheck;
    private bool _doubleJump = true;
    
    [Header("Levitation")]
    public float flyTime = 3f;

    [Header("RampSlide")]
    public float rampDeath = 1f;
    private bool _buttonPress = false;
    [HideInInspector]public LayerMask rampLayer;
    [HideInInspector]public Transform rampCheck;

    void Start()
    {
        _rgbd2d = GetComponent<Rigidbody2D>();
        _spriteRend = GetComponent<SpriteRenderer>();
        _boxColl = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();

        _previousPosition.x = transform.position.x;
    }

    private void Update()
    {
        if (IsSliding() && !_buttonPress)
            {
                rampDeath -= Time.deltaTime;
                if (rampDeath <= 0)
                {
                    _spriteRend.enabled = false;
                }
            }
    }

    private void FixedUpdate()
    {
        if (IsGrounded())
            _rgbd2d.gravityScale = 1f;

        _speed = Mathf.MoveTowards(_speed, maxSpeed, acceleration * Time.deltaTime);
        float _currentSpeed = Mathf.Abs(transform.position.x - _previousPosition.x) / Time.deltaTime;
        if (Mathf.Abs(_speed - _currentSpeed) > 0.10f)
            _speed = _currentSpeed;
        _rgbd2d.linearVelocity = new Vector2(_speed, _rgbd2d.linearVelocity.y);
        _previousPosition.x = transform.position.x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            _doubleJump = true;
            if(context.performed)
            {
                _rgbd2d.linearVelocity = new Vector2(_rgbd2d.linearVelocity.x, jumpingPower);
            }
        }
        else if(_doubleJump && context.performed)
        {
            _rgbd2d.linearVelocity = new Vector2(_rgbd2d.linearVelocity.x, jumpingPower);
            _doubleJump = false;
            _rgbd2d.gravityScale = 1f;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.55f, .1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            _boxColl.size = new Vector2(6.8f, 3.6f);
            _boxColl.offset = new Vector2(0f, -1.8f);
            _animator.SetBool("isCrouch", true);
        }
        else
        {
            _boxColl.size = new Vector2(6.8f, 7.2f);
            _boxColl.offset = new Vector2(0f, 0f);
            _animator.SetBool("isCrouch", false);
        }
    }
    public void Fly(InputAction.CallbackContext context)
    {
        if (context.performed && !IsGrounded())
        {
            _rgbd2d.gravityScale = 0f;
            _rgbd2d.linearVelocity = new Vector2(_rgbd2d.linearVelocity.x, 0);
            StartCoroutine(waitFly());
        }
    }

    IEnumerator waitFly()
    {
        yield return new WaitForSeconds(flyTime);
        _rgbd2d.gravityScale = 1f;
    }

    public void Torpille(InputAction.CallbackContext context)
    {
        if (context.performed && !IsGrounded())
        {
            _rgbd2d.gravityScale = 10f;
        }
    }

    bool IsSliding()
    {
        return Physics2D.OverlapCapsule(rampCheck.position, new Vector2(1f, .3f), CapsuleDirection2D.Horizontal, 0, rampLayer);
    }

    public void RampSlide(InputAction.CallbackContext context){
        if (context.performed && IsSliding())
        {
            _buttonPress = true;
            rampDeath = 1f;
            Debug.Log("OUUIIIIIIIIII");
        }
        else
        {
            _buttonPress = false;
        }
    }
}
