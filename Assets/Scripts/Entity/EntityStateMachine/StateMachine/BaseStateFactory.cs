using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using System;

namespace StateMachine
{
    /*
     * BaseStateFactory, 
     * 
     */
    public class BaseStateFactory

    {
        protected BaseStateMachine _context;

        protected Dictionary<string, BaseState> _states;
        public BaseStateFactory(BaseStateMachine context)
        {
            _context = context;
            _states = new Dictionary<string, BaseState>();
        }

        public virtual BaseState Default()
        {
            throw new NotImplementedException();
        }

        protected T GetOrCreateState<T>(params object[] constructorArgs) where T : BaseState
        {
            string typeName = typeof(T).Name;

            BaseState state;

            if (!_states.TryGetValue(typeName, out state) || state == null)
            {
                // If the state doesn't exist or is null, create a new instance with parameters
                state = (T)Activator.CreateInstance(typeof(T), constructorArgs);
                _states[typeName] = state;
            }

            return state as T;
        }


    }
}