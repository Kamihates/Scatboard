using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rgbd2d;
    private SpriteRenderer _spriteRend;
    private BoxCollider2D _boxColl;
    private Animator _animator;

    [Header("Controller Settings")]
    [SerializeField] private Movement movement;
    [SerializeField] private Jump jump;
    [SerializeField] private Fly fly;
    [SerializeField] private RampSlide rampSlide;

    private float _speed = 0f;
    private Vector2 _previousPosition;
    private bool _canDoubleJump = true;

    void Start()
    {
        _rgbd2d = GetComponent<Rigidbody2D>();
        _spriteRend = GetComponent<SpriteRenderer>();
        _boxColl = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();

        _previousPosition.x = transform.position.x;
    }

    private void FixedUpdate()
    {
        Movement();
        
        if (IsGrounded())
            _rgbd2d.gravityScale = 1f;
    }

    public void Movement()
    {
        _speed = Mathf.MoveTowards(_speed, movement.maxSpeed, movement.maxSpeed * Time.deltaTime);
        float _currentSpeed = Mathf.Abs(transform.position.x - _previousPosition.x) / Time.deltaTime;
        if (Mathf.Abs(_speed - _currentSpeed) > 0.10f && _speed > 2)
            _speed = _currentSpeed;
        _rgbd2d.linearVelocity = new Vector2(_speed, _rgbd2d.linearVelocity.y);
        _previousPosition.x = transform.position.x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded() || IsSliding())
        {
            _canDoubleJump = true;
            if(context.performed)
            {
                _rgbd2d.linearVelocity = new Vector2(_rgbd2d.linearVelocity.x, jump.jumpPower);
            }
        }
        else if(_canDoubleJump && context.performed)
        {
            _rgbd2d.linearVelocity = new Vector2(_rgbd2d.linearVelocity.x, jump.jumpPower);
            _canDoubleJump = false;
            _rgbd2d.gravityScale = 1f;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(jump.groundCheck.position, new Vector2(0.70f, .1f), CapsuleDirection2D.Horizontal, 0, jump.groundLayer);
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
        yield return new WaitForSeconds(fly.duration);
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
        return Physics2D.OverlapCapsule(rampSlide.rampCheck.position, new Vector2(1f, .3f), CapsuleDirection2D.Horizontal, 0, rampSlide.rampLayer);
    }

    public void RampSlide(InputAction.CallbackContext context){
        if (context.performed && IsSliding())
        {
            Debug.Log("OUUIIIIIIIIII");
        }
        else
        {
        }
    }
}
