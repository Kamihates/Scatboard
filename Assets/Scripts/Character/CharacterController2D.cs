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
        float inputX = context.ReadValue<Vector2>().x;
        characterEntity.SetMovement(inputX);
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
}
