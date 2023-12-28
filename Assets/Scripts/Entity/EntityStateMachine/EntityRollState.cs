using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityCharController;
using Entity;
namespace EntityBaseState
{
    public class EntityRollState : BaseState
    {
        new EntityStateMachine _ctx;
        new EntityStateFactory _factory;
        EntityCharacterController characterController;
        Animator _animator;


        bool _isFinished = false;
        public EntityRollState(BaseStateMachine currentContext, BaseStateFactory factory) : base(currentContext, factory)
        {
            _ctx = currentContext as EntityStateMachine;
            _factory = factory as EntityStateFactory;
            characterController = _ctx.EntityController;
            _animator = _ctx.Animator;

        }

        public override void CheckSwitchStates()
        {
            if (_isFinished)
            {
                SwitchState(_factory.Idle());
            }
        }

        public override void Cleanup()
        {
            ///throw new System.NotImplementedException();
        }

        public override void EnterState()
        {

            Vector2 movement = _ctx.movementInput;
            Debug.Log("Entered roll");
            if (movement == Vector2.zero)
            {
                _isFinished = true;
                return;
            }
            characterController.roll(movement);

            characterController.OnControlStateSwitch += onRollEnd;
            _isFinished = false;
            _animator.SetBool("isRolling", true);
            _animator.Play("JumpAir_Spin_InPlace_SwordAndShield");
        }

        public override void ExitState()
        {
            characterController.OnControlStateSwitch -= onRollEnd;
            _ctx.endRoll();
            _animator.SetBool("isRolling", false);
        }

        public override void FixedUpdateState()
        {
            //throw new System.NotImplementedException();
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

        void onRollEnd(string newState)
        {
            _isFinished = true;
        }
    }

}
