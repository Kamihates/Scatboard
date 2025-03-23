using UnityEngine;

[System.Serializable]
public class CharacterManager2D : MonoBehaviour
{
    [Header("Movements")]
    public float speed;
    public float jumpPower;
    public LayerMask groundLayer;
    public Transform groundCheck;

    [Header("Jump")]
    public int maxJump;
    public bool canDoubleJump;

    [Header("Dash")]
    public bool canDash;
    public float dashingPower;
    public float dashingCooldown;
    public float dashingTime;

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public float horizontal;
    [HideInInspector] public float verticalInput; // Input vertical pour la l�vitation

    [Header("Crouch")]
    public bool canCrouch = true;
    [Range(0, 1)] public float crouchPower;
    [Range(0, 1)] public float normalHeight;
    [HideInInspector] public bool isCrouching = false;

    [Header("Levitation")]
    public bool canLevitate = true;
    [Range(1, 5)] public float levitationHeight;  // La hauteur � laquelle le joueur l�vite
    [Range(1, 10)] public float levitationDuration;  // Dur�e de la l�vitation
    [Range(1, 10)] public float levitationCooldown;  // Cooldown du mode l�vitation
    public float levitationTimer;  // Timer de la l�vitation
    public float levitationCooldownTimer;  // Timer du cooldown de la l�vitation
    [HideInInspector] public float levitationStartY;
    [HideInInspector] public bool isLevitationActive = false;  // Le mode l�vitation est-il actif ?
    [HideInInspector] public bool hasReachedLevitationStartY;

}
