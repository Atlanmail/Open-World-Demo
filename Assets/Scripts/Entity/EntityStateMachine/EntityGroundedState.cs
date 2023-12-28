using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Entity;
namespace EntityBaseState
{
    public class EntityGroundedState : BaseState
    {
        new EntityStateMachine _ctx;
        new EntityStateFactory _factory;

        Animator _animator;
        public EntityGroundedState(BaseStateMachine currentContext, BaseStateFactory factory) : base(currentContext, factory)
        {
            this._ctx = currentContext as EntityStateMachine;
            this._factory = factory as EntityStateFactory;
            _isRootState = true;

            _animator = _ctx.Animator;
        }

        public override void CheckSwitchStates()
        {
            if (_ctx.jumpButtonPressed && _ctx.isGrounded)
            {
                SwitchState(_factory.Airborne());
            }
        }

        public override void Cleanup()
        {

        }

        public override void EnterState()
        {
            InitializeSubState();
            _animator.SetBool("isGrounded", true);
        }

        public override void ExitState()
        {
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isRolling", false);
            _animator.SetBool("isGrounded", false);
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
}

