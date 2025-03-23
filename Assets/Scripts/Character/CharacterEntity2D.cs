using UnityEngine;
using System.Collections;

public class CharacterEntity2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private TrailRenderer tr;
    private CapsuleCollider2D capsuleCollider2D;

    [HideInInspector] public CharacterManager2D manager;

    private bool isDashing;
    private bool isFacingRight = true;

    private int jumpsLeft;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        tr = GetComponent<TrailRenderer>();
        manager = FindFirstObjectByType<CharacterManager2D>();
        capsuleCollider2D = GetComponentInParent<CapsuleCollider2D>();

        jumpsLeft = manager.maxJump;
    }

    private void FixedUpdate()
    {
        if (manager.isDead || isDashing || manager.isCrouching) return;

        rb.linearVelocity = new Vector2(manager.horizontal * manager.speed, rb.linearVelocity.y);
        animator.SetFloat("Speed", Mathf.Abs(manager.horizontal));

        Flip();

        if (IsGrounded())
        {
            jumpsLeft = manager.maxJump;
            animator.ResetTrigger("Jump");
        }
    }

    public void SetMovement(float inputX)
    {
        manager.horizontal = inputX;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(manager.groundCheck.position, new Vector2(0.28f, 0.23f), CapsuleDirection2D.Horizontal, 0, manager.groundLayer);
    }

    private void Flip()
    {
        if ((isFacingRight && manager.horizontal < 0f) || (!isFacingRight && manager.horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void Jump()
    {
        if (manager.isDead || jumpsLeft <= 0) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, manager.jumpPower);
        animator.SetTrigger("Jump");
        jumpsLeft--;
    }

    public void Dash()
    {
        if (manager.isDead || isDashing) return;
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        if (!manager.canDash) yield break;

        isDashing = true;
        manager.canDash = false;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float dashDirection = manager.horizontal != 0 ? Mathf.Sign(manager.horizontal) : (isFacingRight ? 1 : -1);
        rb.linearVelocity = new Vector2(dashDirection * manager.dashingPower, rb.linearVelocity.y);
        tr.emitting = true;

        yield return new WaitForSeconds(manager.dashingTime);

        rb.gravityScale = originalGravity;
        tr.emitting = false;
        isDashing = false;

        yield return new WaitForSeconds(manager.dashingCooldown);
        manager.canDash = true;
    }

    public void Crouch(bool isCrouching)
    {
        if (!manager.canCrouch) return;

        manager.isCrouching = isCrouching;

        if (isCrouching)
        {
            rb.linearVelocity = Vector2.zero; // Stop le mouvement
            // Réduit la taille du collider
            capsuleCollider2D.transform.localScale = new Vector3(capsuleCollider2D.transform.localScale.x, manager.crouchPower, capsuleCollider2D.transform.localScale.z);
        }
        else
        {
            capsuleCollider2D.transform.localScale = new Vector3(capsuleCollider2D.transform.localScale.x, manager.normalHeight, capsuleCollider2D.transform.localScale.z); // Remet la taille normale
        }
    }

}
