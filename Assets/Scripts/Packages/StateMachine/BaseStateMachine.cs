using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;


namespace StateMachine
{
    public class BaseStateMachine : MonoBehaviour
    {


        [SerializeField] private BaseStateMachineData _stateMachineData;
        [SerializeField] protected BaseState _currentState;
        protected BaseStateFactory _states;



        public BaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
        public BaseStateMachineData StateMachineData { get { return _stateMachineData; } }

        protected virtual void Awake()
        {
            stateMachineDataSetup();

            stateFactorySetup();

        }

        protected virtual void Start()
        {
            enterDefaultState();

        }
        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void Update()
        {


            _currentState.UpdateStates();
        }

        protected virtual void FixedUpdate()
        {
            _currentState.FixedUpdateStates();
        }

        protected virtual void LateUpdate()
        {
            _currentState.LateUpdateStates();
        }

        
        protected virtual void enterDefaultState()
        {
            _currentState = _states.Default();
            _currentState.EnterState();
        }
        /**
         * prepares stateMachineData
         */
        protected virtual void stateMachineDataInitialize()
        {
            _stateMachineData.initialize();
        }
        /**
         * Clones your stateMachineData from the template data
         */
        protected virtual void stateMachineDataSetup()
        {
            _stateMachineData = _stateMachineData.Clone();
        }
        
        protected virtual void stateFactorySetup()
        {
            _states = new BaseStateFactory(this);
        }
    }
}
