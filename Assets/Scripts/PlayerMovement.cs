using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction moveAction;

    private Vector2 deplacementInput;
    public float vitesse = 5f;
    public float vitesseRotation = 12f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        if (playerInput != null && playerInput.actions != null)
        {
            moveAction = playerInput.actions.FindAction("Player/Deplacement");
        }

        if (moveAction != null)
        {
            moveAction.performed += OnDeplacementPerformed;
            moveAction.canceled += OnDeplacementCanceled;
        }
        else
        {
            Debug.LogWarning("Action 'Player/Deplacement' introuvable. Vérifie les noms dans PlayerInputActions.");
        }
    }

    void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.performed -= OnDeplacementPerformed;
            moveAction.canceled -= OnDeplacementCanceled;
        }
    }

    void Update()
    {
        Vector3 direction = new Vector3(deplacementInput.x, 0f, deplacementInput.y);

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion rotationCible = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationCible, vitesseRotation * Time.deltaTime);
        }

        controller.Move(direction * vitesse * Time.deltaTime);
    }

    private void OnDeplacementPerformed(InputAction.CallbackContext contexte)
    {
        deplacementInput = contexte.ReadValue<Vector2>();
    }

    private void OnDeplacementCanceled(InputAction.CallbackContext contexte)
    {
        deplacementInput = Vector2.zero;
    }
}