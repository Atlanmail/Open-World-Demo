using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
public class EntityGroundedState : BaseState
{
    new EntityStateMachine _ctx;
    new EntityStateFactory _factory;
    public EntityGroundedState(BaseStateMachine currentContext, BaseStateFactory factory) : base(currentContext, factory)
    {
        this._ctx = currentContext as EntityStateMachine;
        this._factory = factory as EntityStateFactory;
        _isRootState = true;
    }

    public override void CheckSwitchStates()
    {
        
    }

    public override void Cleanup()
    {
        
    }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void ExitState()
    {
        
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void InitializeSubState()
    {
        this.SetSubState(_factory.Idle());
    }

    public override void LateUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    
}
