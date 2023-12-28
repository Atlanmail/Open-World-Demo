using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


/**
 * The disable MovementState has a value of 0 for all.
 * 
 * Movement states control the speed of horizontal movement and the gravity on that object.
 */
[System.Serializable]



public enum MovementStates
{
    Ground,
    GroundSprint,
    Falling,
    Glide,
    Jumping,
    Disable

} 


[System.Serializable]
public struct MovementStats
{
    [SerializeField] public MovementStates Type;
    [SerializeField] public float speed;
    [SerializeField] public float turnSpeed;
    

    public MovementStats(MovementStates type, float _speed, float _turnSpeed)
    {   
        this.Type = type;
        this.speed = _speed;
        this.turnSpeed = _turnSpeed;
        
    }
}

[System.Serializable]
public struct RollStats
{
    [SerializeField] private float _distance;
    [SerializeField] private int _frames;

    public float distance { get { return _distance; } }
    public int frames { get { return _frames; } }
}

[System.Serializable]

public struct JumpStats
{
    [SerializeField] private float _distance;
    [SerializeField] private float _frames;
    [SerializeField] private MovementStates _state;
    public float distance { get { return _distance; } }
    public float frames { get { return _frames; } }
    public MovementStates state { get { return _state; } }

}

public class EntityCharacterController : MonoBehaviour
{
    private enum ControllerState
    {
        Movement,
        Roll,
        Jump,
        Disable
    }

    CharacterController _controller;

    EntityData _data;
    EntityControllerData _controllerData;
    EntityStateMachine _entityStateMachine;

    MovementStats _currentMovementStat;
    RollStats _curRollStats;
    JumpStats _curJumpStats;
    ControllerState _currentControllerState;

    float speed;
    float turnSpeed;
    float gravityValue = -9.8f;

    bool lockMovementForwards = true;
    bool gravityEnabled = true;

    Vector3 targetPosition;
    Vector3 targetFacePosition;

    int curFrameCount;

    public event Action<string> OnControlStateSwitch;

    public Vector3 Position { get { return _controller.transform.position; } }
    private void Awake()
    {
        _entityStateMachine = GetComponent<EntityStateMachine>();
        _controller = GetComponent<CharacterController>();

        _data = _entityStateMachine.EntityData;
        _controllerData = _data.ControllerData;

        _currentControllerState = ControllerState.Movement;
        _curRollStats = _controllerData.rollStats;
        _curJumpStats = _controllerData.jumpStats;
        


        
    }
    void Start()
    {
        targetPosition = Position;
        targetFacePosition = Position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_currentControllerState == ControllerState.Disable)
        {
            return;
        }   
        else if (_currentControllerState == ControllerState.Roll)
        {
            handleRoll();
        }
        else if (_currentControllerState == ControllerState.Movement)
        {
            handleMovement();
            handleGravity();
            handleRotation();
        }
        else if (_currentControllerState == ControllerState.Jump)
        {
            handleJump();
        }


    }

    public Vector3 Forward()
    {
        return _controller.transform.forward;
    }

    public void moveTo(float x, float y, float z)
    {
        this.moveTo(new Vector3(x, y, z));
    }

    /// <summary>
    /// sets a destination
    /// </summary>
    /// <param name="position"></param>
    public void moveTo(Vector3 position)
    {
        if (_currentControllerState == ControllerState.Disable)
        {
            return;
        }
        if (_currentControllerState == ControllerState.Roll)
        {
            return;
        }

        targetPosition = position;

        if (lockMovementForwards)
        {
            targetFacePosition = position;
        }
    }

    public void lookAt(Vector3 position)
    {
        if (_currentControllerState != ControllerState.Movement)
        {
            return;
        }
        targetFacePosition = position;
    }

    private void handleMovement()
    {
        Vector2 movementInput = new Vector2(targetPosition.x -this.Position.x, targetPosition.z - this.Position.z);

        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        movement = movement.normalized * speed * Time.fixedDeltaTime;

        

        ///Debug.Log(movement);
        ///Debug.Log("HandleMovement movement " + movement);
        _controller.Move(movement);
        
    }

    private void handleGravity()
    {
        
        if (gravityEnabled && _controller.isGrounded == false)
        {
            Vector3 gravityVector = new Vector3(0, gravityValue * Time.fixedDeltaTime, 0);
            _controller.Move(gravityVector);
        }
        
    }

    private void handleRotation()
    {

        if (!shouldTurn())
        {
            return;
        }

        Vector3 positionToLookAt = targetFacePosition - this.Position;

       

        ///positionToLookAt.y = this.Position.y;

        Debug.DrawRay(this.Position, positionToLookAt);

        Quaternion currentRotation = _controller.transform.rotation;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

        _controller.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    /// <summary>
    /// rolls in a direction relative to worldspace
    /// </summary>
    /// <param name="rollDirection"></param>
    public void roll(Vector2 rollDirection)
    {
        targetPosition = Position + new Vector3(rollDirection.x, 0, rollDirection.y) * _curRollStats.distance;

        curFrameCount = 0;
        SwitchState(ControllerState.Roll);
        
    }

    private void handleRoll()
    {
        float rollSpeed = _curRollStats.distance / _curRollStats.frames;
        ////Debug.Log("Rollspeed " + rollSpeed);
        Vector2 movementInput = new Vector2(targetPosition.x - this.Position.x, targetPosition.z - this.Position.z);

        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        movement = movement.normalized * rollSpeed;



        ///Debug.Log("Movement " + movement);
        ///Debug.Log("HandleMovement movement " + movement);
        _controller.Move(movement);

        curFrameCount = curFrameCount + 1;
        if (curFrameCount >= _curRollStats.frames )
        {
            ///Debug.Log("Switched to roll");
            SwitchState(ControllerState.Movement);
        }
    }

    public void jump(Vector2 movementVector)
    {
        targetPosition = new Vector3() + this.Position;
        curFrameCount = 0;

        setMovementState(MovementStates.Jumping);
    }
    private void handleJump()
    {
        handleMovement();

        applyJumpForce();
        
        handleRotation();
        curFrameCount++;

        if (curFrameCount >= _curRollStats.frames)
        {
            ///Debug.Log("Switched to roll");
            SwitchState(ControllerState.Movement);
            setMovementState(MovementStates.Falling);
        }
    }

    private void applyJumpForce()
    {
        float velocity = _curJumpStats.distance / _curJumpStats.frames;
        Vector3 gravityVector = new Vector3(0, velocity, 0);
        _controller.Move(gravityVector);
    }

    public void setMovementState(MovementStates state)
    {
        MovementStats[] stats = _controllerData.movementStats;

        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i].Type == state)
            {
                _currentMovementStat = stats[i];
            }
        }

        this.speed = _currentMovementStat.speed;
        this.turnSpeed = _currentMovementStat.turnSpeed;
    }

    private void SwitchState(ControllerState state)
    {
        _currentControllerState = state;

        OnControlStateSwitch?.Invoke(state.ToString());
    }

    public void Disable()
    {
        SwitchState(ControllerState.Disable);
    }

    public void Enable()
    {
        SwitchState(ControllerState.Movement);
    }

    /*
     * trigger to see if the thing should turn during movement;
     */
    public bool shouldTurn()
    {
        if (_currentControllerState != ControllerState.Movement)
        {
            return false;
        }
        Vector3 positionToLookAt = targetFacePosition - this.Position;

        if (positionToLookAt.magnitude < 0.5f)
        {
            return false;
        }
        return true;
    }
}
