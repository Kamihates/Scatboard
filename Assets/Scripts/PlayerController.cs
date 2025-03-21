using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Transform playerTransform;
    private float horizontal;
    [HideInInspector] public bool isFacingRight = true;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    private int jumpCount;
    [SerializeField] private int jumpCountLimit;
    [SerializeField] private LayerMask jumpMaskDetection;

    [Header("Wall")]
    [SerializeField] private LayerMask detectWall;
    [SerializeField] private int distanceWall;

    [Header("Dash")]
    [SerializeField] private bool canDash; // Le joueur peut-il dash?
    [SerializeField] private float dashingPower; // Puissance de dash
    [SerializeField] private float dashingCooldown; // Le temps avant que le joueur puisse dash a nouveau
    private bool isDashing;
    private float dashingTime = 0.2f;
    [SerializeField] private TrailRenderer tr;

    void Start()
    {
        TryGetComponent(out rb);
        TryGetComponent(out playerTransform);
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        // Detection du mur
        var hit = Physics2D.BoxCast(transform.position,Vector2.one,0, transform.right, distanceWall, detectWall);

        if (hit.collider != null)
            return;

        Debug.DrawRay(transform.position, transform.TransformPoint(horizontal, distanceWall, detectWall));

        horizontal = context.ReadValue<Vector2>().x;
        Flip();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && jumpCount < jumpCountLimit)
        {
            rb.linearVelocity = Vector2.up * jumpForce;
            jumpCount++;
        }
    }

    public void HandleDash(InputAction.CallbackContext context)
    {

        if (context.performed)  
        {
            StartCoroutine(Dash());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            jumpCount = 0;

    }

    private IEnumerator Dash()
    {
        if (!canDash) yield break; // Si le dash n'est pas disponible, on sort immdiatement.

        isDashing = true;
        canDash = false; // Desactive immediatement la possibilit  de dash

        // Sauvegarde la gravit  actuelle pour pouvoir la restaurer plus tard
        float originalGravity = rb.gravityScale;

        // R duit ou annule la gravit  pendant le dash pour  viter que le personnage tombe trop vite
        rb.gravityScale = 0f;

        // Utilise le mouvement horizontal ou la direction actuelle (flipX) pour d terminer le sens du dash
        float dashDirection;

        if (horizontal != 0)
            dashDirection = horizontal; // Si l'utilisateur bouge, on utilise la direction horizontale
        else
            dashDirection = !isFacingRight ? -1 : 1; // Sinon on v rifie la direction du flipX

        // Dash dans la direction choisie
        rb.linearVelocity = new Vector2(dashDirection * dashingPower, rb.linearVelocity.y); // Garde la vitesse verticale inchang e
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime); // Attendre le temps du dash

        // Restaure la gravit  apr s le dash
        rb.gravityScale = originalGravity;

        tr.emitting = false;
        isDashing = false;

        // Attendre le cooldown avant de permettre un nouveau dash
        yield return new WaitForSeconds(dashingCooldown); // Le cooldown avant le prochain dash

        canDash = true; // Le dash est de nouveau disponible apr s le cooldown
    }

    public void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

    }
}
