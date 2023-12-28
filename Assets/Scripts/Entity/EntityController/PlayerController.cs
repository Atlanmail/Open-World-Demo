using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private EntityStateMachine _entityStateMachine; /// variable that allows to set entityStateMachine
    /// </summary>
    [SerializeField] private CameraController _cameraController;


    /// <summary>
    private PlayerActions _playerActions;
    /// </summary>

    private bool sprintButtonPressed = false;
    Vector2 movementInput;

    /// <summary>
    /// PlayerTracker _playerTracker;
    /// </summary>

    private void Awake()
    {
    }
    void Start()
    {
        
    }
    void OnEnable()
    {
        _playerActions = new PlayerActions();
        _playerActions.Enable();


        movementInput = Vector2.zero;
        // Subscribe to actions
       
        _playerActions.Grounded.Movement.performed += MovementDirection_performed;
        _playerActions.Grounded.Roll.performed += Roll_Performed;
        
    }

   

    void OnDisable()
    {
        _playerActions.Grounded.Movement.performed -= MovementDirection_performed;
        _playerActions.Grounded.Roll.performed -= Roll_Performed;
    }
    /*private void Jump_performed(InputAction.CallbackContext context)
    {
        Debug.Log("Jump pressed");
        _entityStateMachine.OnJump();
    }
    

    
    */
    private void MovementDirection_performed(InputAction.CallbackContext context)
    {
        
        movementInput = context.ReadValue<Vector2>();
        ///Debug.Log(movementInput);
        _entityStateMachine.move(movementInput);
    }

    private void Roll_Performed(InputAction.CallbackContext context)
    {
        float rollInput = context.ReadValue<float>();

        if (rollInput == 1)
        {
            if (movementInput != Vector2.zero)
            {
                _entityStateMachine.roll(movementInput);
            }
            
        }
        else
        {
            ///Debug.Log("Sprint ended");
        }
    }

    /*private void OnAttack(InputAction.CallbackContext context)
    {
        ///Debug.Log(context.performed);
        _entityStateMachine.Attack();
    }

    private void OnCast1(InputAction.CallbackContext context)
    {
        _entityStateMachine.revive();
    }

    private void OnShield(InputAction.CallbackContext context)
    {
        float shieldInput = context.ReadValue<float>();

        if (shieldInput == 1)
        {
            _entityStateMachine.startBlock();
        }
        else
        {
            _entityStateMachine.endBlock();
        }
    }
   */




}
