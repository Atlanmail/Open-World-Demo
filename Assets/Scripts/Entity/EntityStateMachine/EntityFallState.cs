using EntityCharController;
using StateMachine;
using System.Collections;
using UnityEngine;
using Entity;

namespace EntityBaseState
{
    public class EntityFallState : BaseState
    {
        new EntityStateMachine _ctx;
        new EntityStateFactory _factory;

        Animator _animator;
        EntityCharacterController _characterController;
        public EntityFallState(BaseStateMachine currentContext, BaseStateFactory factory) : base(currentContext, factory)
        {
            _ctx = currentContext as EntityStateMachine;
            _factory = factory as EntityStateFactory;

            _animator = _ctx.Animator;
            _characterController = _ctx.EntityController;

        }

        public override void CheckSwitchStates()
        {

        }

        public override void Cleanup()
        {
            throw new System.NotImplementedException();
        }

        public override void EnterState()
        {
            _characterController.setMovementState(MovementStates.Falling);
        }

        public override void ExitState()
        {
            ////
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

        public override void UpdateState()
        {
            base.UpdateState();

            handleMovementInput();
            ///handle animation
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


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
