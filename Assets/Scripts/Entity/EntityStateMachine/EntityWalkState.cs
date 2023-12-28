using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Entity;
using EntityCharController;
namespace EntityBaseState
{
    public class EntityWalkState : BaseState, ISprintState
    {

        new EntityStateMachine _ctx;
        new EntityStateFactory _factory;

        EntityCharacterController _characterController;
        EntityData _entityData;
        Animator _animator;



        bool _isSprinting;

        public EntityWalkState(BaseStateMachine currentContext, BaseStateFactory factory) : base(currentContext, factory)
        {
            this._ctx = currentContext as EntityStateMachine;
            this._factory = factory as EntityStateFactory;


            _entityData = _ctx.EntityData;
            _characterController = _ctx.EntityController as EntityCharacterController;
            _animator = _ctx.Animator;
            //speed = _entityData.Speed;
            //turnSpeed = _entityData.TurnSpeed;
        }

        public bool isSprinting { get { return _isSprinting; } }

        public override void CheckSwitchStates()
        {
            if (_ctx.rollButtonPressed)
            {
                SwitchState(_factory.Roll());
            }
            else if (_ctx.movementInput == Vector2.zero && _characterController.shouldTurn() == false)
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
            ///throw new System.NotImplementedException();
            Debug.Log("Entered walk");
            _characterController.setMovementState(MovementStates.Ground);
            _animator.SetBool("isWalking", true);
            _animator.Play("MoveFWD_SwordAndShield");
        }

        public override void ExitState()
        {
            ///throw new System.NotImplementedException();


            _animator.SetBool("isWalking", false);
        }

        public override void FixedUpdateState()
        {
            /*handleMovement();
            handleRotation();*/
        }

        public override void InitializeSubState()
        {

        }

        public override void LateUpdateState()
        {

        }

        public void startSprint()
        {
            if (_isSprinting)
            {
                return;
            }


        }

        public void stopSprint()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            handleMovementInput();
            handleAnimation();
        }

        protected virtual void handleMovementInput()
        {
            Vector2 movementInput = _ctx.movementInput;
            Vector2 faceDirection = _ctx.faceDirection;
            Vector3 movementOffset = new Vector3(movementInput.x, 0, movementInput.y).normalized;
            ///Debug.Log("Movement offset: " + movementOffset);
            _characterController.moveTo(_characterController.Position + movementOffset);
            ///_characterController.lookAt(new Vector3(faceDirection.x, 0, faceDirection.y) + _characterController.Position);
        }

        protected void handleAnimation()
        {
            ///_animator.Play("MoveFWD_SwordAndShield");
        }




    }

}