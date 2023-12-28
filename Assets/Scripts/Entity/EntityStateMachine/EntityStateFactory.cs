using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace EntityBaseState {
    public class EntityStateFactory : BaseStateFactory
    {


        public EntityStateFactory(BaseStateMachine context) : base(context)
        {
        }

        public override BaseState Default()
        {
            return this.Grounded();
        }

        public virtual BaseState Grounded()
        {
            return GetOrCreateState<EntityGroundedState>(_context, this);
        }

        public virtual BaseState Walk()
        {
            return GetOrCreateState<EntityWalkState>(_context, this);
        }

        public virtual BaseState Idle()
        {
            return GetOrCreateState<EntityIdleState>(_context, this);
        }

        public virtual BaseState Roll()
        {
            return GetOrCreateState<EntityRollState>(_context, this);
        }

        public virtual BaseState Airborne()
        {
            return GetOrCreateState<EntityAirborneState>(_context, this);
        }

        public virtual BaseState Jump()
        {
            return GetOrCreateState<EntityJumpState>(_context, this);
        }
        public virtual BaseState Fall()
        {
            return GetOrCreateState<EntityFallState>(_context, this);
        }
    }
}

