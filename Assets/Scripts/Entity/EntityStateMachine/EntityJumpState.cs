using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using EntityCharController;
using Entity;

namespace EntityBaseState
{
    public class EntityJumpState : BaseState
    {
        new EntityStateMachine _ctx;
        new EntityStateFactory _factory;

        Animator _animator;
        EntityCharacterController _characterController;

        bool _isFinished;
        public EntityJumpState(BaseStateMachine currentContext, BaseStateFactory factory) : base(currentContext, factory)
        {
            _ctx = currentContext as EntityStateMachine;
            _factory = factory as EntityStateFactory;

            _animator = _ctx.Animator;
            _characterController = _ctx.EntityController;

        }

        public override void CheckSwitchStates()
        {
            if (_isFinished)
            {
                SwitchState(_factory.Fall());
            }
        }

        public override void Cleanup()
        {
            ///throw new System.NotImplementedException();
        }

        public override void EnterState()
        {
            _animator.Play("JumpStart_Normal_InPlace_SwordAndShield");
            _isFinished = false;

            _characterController.jump(Vector2.zero);

            _characterController.OnControlStateSwitch += onJumpEnd;
        }

        public override void ExitState()
        {
            _characterController.OnControlStateSwitch -= onJumpEnd;
        }

        public override void FixedUpdateState()
        {
            ///throw new System.NotImplementedException();
        }

        public override void InitializeSubState()
        {
            ///throw new System.NotImplementedException();
        }

        public override void LateUpdateState()
        {
            ///throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void onJumpEnd(string newState)
        {
            _isFinished = true;
            _ctx.jumpEnd();
        }
    }

}
