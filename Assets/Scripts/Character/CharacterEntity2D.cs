using UnityEngine;
using System.Collections;

public class CharacterEntity2D : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
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

        if (manager.isLevitationActive)
        {
            HandleLevitationMovement();
            manager.levitationTimer -= Time.deltaTime;

            if (manager.levitationTimer <= 0)
            {
                manager.isLevitationActive = false;
                rb.gravityScale = 2;
                manager.levitationCooldownTimer = manager.levitationCooldown;
                manager.canLevitate = false;  // Désactive temporairement la lévitation
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(manager.horizontal * manager.speed, rb.linearVelocity.y);
        }

        // Gestion du cooldown de la lévitation
        if (!manager.canLevitate)
        {
            manager.levitationCooldownTimer -= Time.deltaTime;
            if (manager.levitationCooldownTimer <= 0)
            {
                manager.canLevitate = true; // Réactive la possibilité de léviter
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(manager.horizontal));
        Flip();

        if (IsGrounded())
        {
            jumpsLeft = manager.maxJump;
            animator.ResetTrigger("Jump");
        }
    }


    private void HandleLevitationMovement()
    {
        // Désactive la gravité
        rb.gravityScale = 0f;

        // Calcul de la hauteur cible
        float targetY = manager.levitationStartY + manager.levitationHeight;

        if (!manager.hasReachedLevitationStartY)
        {
            // Tant que la hauteur cible n'est pas atteinte, on monte
            if (transform.position.y < targetY - 0.1f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 5f); // Vitesse de montée
            }
            else
            {
                // Une fois à la hauteur, on passe en mode libre
                manager.hasReachedLevitationStartY = true;
            }
        }

        if (manager.hasReachedLevitationStartY)
        {
            // Permet le déplacement libre
            rb.linearVelocity = new Vector2(manager.horizontal * manager.speed, manager.verticalInput * manager.speed);
        }
    }


    public void SetMovement(float inputX, float inputY)
    {
        manager.horizontal = inputX;
        manager.verticalInput = inputY;  // Ajout du mouvement vertical
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
        if (manager.isDead || jumpsLeft <= 0 || manager.isLevitationActive) return; // Bloque le saut pendant la lévitation

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
            capsuleCollider2D.transform.localScale = new Vector3(capsuleCollider2D.transform.localScale.x, manager.crouchPower, capsuleCollider2D.transform.localScale.z);
        }
        else
        {
            capsuleCollider2D.transform.localScale = new Vector3(capsuleCollider2D.transform.localScale.x, manager.normalHeight, capsuleCollider2D.transform.localScale.z); // Remet la taille normale
        }
    }
}