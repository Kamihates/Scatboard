using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
    private CharacterEntity2D characterEntity;

    private void Awake()
    {
        characterEntity = GetComponentInChildren<CharacterEntity2D>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        // Lecture de l'input horizontal et vertical
        Vector2 input = context.ReadValue<Vector2>();
        characterEntity.SetMovement(input.x, input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
            characterEntity.Jump();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
            characterEntity.Dash();
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        characterEntity.Crouch(context.performed);
    }

    public void Levitate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Emp�cher d'activer la l�vitation si le cooldown n'est pas termin�
            if (!characterEntity.manager.isLevitationActive && characterEntity.manager.canLevitate)
            {
                characterEntity.manager.isLevitationActive = true;
                characterEntity.manager.levitationStartY = characterEntity.transform.position.y;
                characterEntity.manager.levitationTimer = characterEntity.manager.levitationDuration;
                characterEntity.manager.hasReachedLevitationStartY = false;
            }
            else if (characterEntity.manager.isLevitationActive)
            {
                characterEntity.manager.isLevitationActive = false;
                characterEntity.rb.gravityScale = 2f;
                characterEntity.rb.linearVelocity = Vector2.zero;

                // Lancer le cooldown une fois la l�vitation d�sactiv�e
                characterEntity.manager.levitationCooldownTimer = characterEntity.manager.levitationCooldown;
                characterEntity.manager.canLevitate = false;
            }
        }
    }
}