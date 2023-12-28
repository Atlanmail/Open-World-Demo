using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;

namespace EntityBaseState
{
    public class EntityAirborneState : BaseState
    {
        new EntityStateMachine _ctx;
        new EntityStateFactory _factory;

        Animator _animator;
        bool subStateInitialized;
        public EntityAirborneState(BaseStateMachine currentContext, BaseStateFactory factory) : base(currentContext, factory)
        {
            this._ctx = currentContext as EntityStateMachine;
            this._factory = factory as EntityStateFactory;
            _isRootState = true;
            _animator = _ctx.Animator;
            subStateInitialized = false;
        }

        public override void CheckSwitchStates()
        {
            if (_ctx.EntityController.isGrounded && subStateInitialized)
            {
                SwitchState(_factory.Grounded());
            }
        }

        public override void Cleanup()
        {

        }

        public override void EnterState()
        {
            subStateInitialized = false; ;
            InitializeSubState();
            subStateInitialized = true;
            _animator.SetBool("isAirborne", true);
        }

        public override void ExitState()
        {
            _animator.SetBool("isAirborne", false);
        }

        public override void FixedUpdateState()
        {

        }

        public override void InitializeSubState()
        {
            if (_ctx.jumpButtonPressed == true)
            {
                SetSubState(_factory.Jump());
            }
            else
            {
                SetSubState(_factory.Fall());
            }
        }

        public override void LateUpdateState()
        {
            //throw new System.NotImplementedException();
        }
    }

}
