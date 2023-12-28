using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
public class EntityIdleState : BaseState
{
    new EntityStateMachine _ctx;
    new EntityStateFactory _factory;

    Animator _animator;
    EntityCharacterController _characterController;

    public EntityIdleState(BaseStateMachine currentContext, BaseStateFactory factory) : base(currentContext, factory)
    {
        this._ctx = currentContext as EntityStateMachine;
        this._factory = factory as EntityStateFactory;
        _animator = _ctx.Animator;
        _characterController = _ctx.EntityController;
    }


    
    public override void CheckSwitchStates()
    {
        if (_ctx.rollButtonPressed)
        {
            SwitchState(_factory.Roll());
        }
        else if (_ctx.movementInput != Vector2.zero || _characterController.shouldTurn())
        {
            SwitchState(_factory.Walk());
        }
    }

    public override void Cleanup()
    {
       
    }

    public override void EnterState()
    {
        _animator.Play("Idle_Normal_SwordAndShield");
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isRolling", false);
    }

    public override void ExitState()
    {
        
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void InitializeSubState()
    {
        ///throw new System.NotImplementedException();
    }

    public override void LateUpdateState()
    {
        ///throw new System.NotImplementedException();
    }
    public override void UpdateState()
    {
        base.UpdateState();
        
    }

    private void handleAnimation()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
