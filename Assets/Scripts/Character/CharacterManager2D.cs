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

    [Header("Crouch")]
    public bool canCrouch = true;
    [Range(0,1)] public float crouchPower;
    [Range(0, 1)] public float normalHeight;
    [HideInInspector] public bool isCrouching = false;

}
