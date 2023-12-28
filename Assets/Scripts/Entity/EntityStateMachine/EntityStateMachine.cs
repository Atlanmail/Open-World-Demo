using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using EntityBaseState;
using EntityCharController;
using System.Runtime.InteropServices.WindowsRuntime;
using static UnityEngine.GraphicsBuffer;
/// <summary>
/// using System.Security.Cryptography;
/// </summary>
namespace Entity
{
    public class EntityStateMachine : BaseStateMachine
    {
        protected EntityCharacterController _controller;
        protected Animator _animator;
        /// <summary>
        /// protected EntityData _data;
        /// </summary>



        private Dictionary<string, int> cooldowns = new Dictionary<string, int>();

        /// movement variables
        /// 
        protected Vector2 _movementInput;
        protected Vector2 _faceDirection; /// target face direction

        protected bool _rollButtonPressed;
        protected bool _jumpButtonPressed;


        /// getters and setters


        public EntityCharacterController EntityController { get { return _controller; } }
        public Animator Animator { get { return _animator; } }
        public Vector2 movementInput { get { return _movementInput; } }
        public Vector2 faceDirection { get { return _faceDirection; } }
        public EntityData EntityData { get { return this.StateMachineData as EntityData; } }
        public bool jumpButtonPressed { get { return _jumpButtonPressed; } }
        public bool rollButtonPressed { get { return _rollButtonPressed; } }
        public bool isGrounded { get { return _controller.isGrounded; } }

        protected override void Awake()
        {
            base.Awake();

            _controller = GetComponent<EntityCharacterController>();
            _animator = GetComponentInChildren<Animator>();


        }
        protected override void stateFactorySetup()
        {
            _states = new EntityStateFactory(this);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            tickCooldown();
            ///Debug.Log(movementInput.ToString());
        }
        public void addCooldown(string name, int cooldown)
        {
            if (cooldowns.ContainsKey(name))
            {
                cooldowns[name] = cooldown;
                return;
            }
            else
            {
                cooldowns.Add(name, cooldown);
            }

        }
        private void tickCooldown()
        {
            foreach (var kvp in cooldowns)
            {
                cooldowns[kvp.Key] = kvp.Value - 1;
            }
        }
        /// <summary>
        /// returns if a cooldown with the ability name is on cooldown
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool isOnCooldown(string name)
        {
            if (!cooldowns.ContainsKey(name))
            {
                return false;
            }
            else
            {
                return (cooldowns[name] > 0);
            }
        }


        public void move(Vector2 newMovement)
        {
            _movementInput.x = newMovement.x;
            _movementInput.y = newMovement.y;
        }

        public void face(Vector2 newFace)
        {
            _faceDirection.x = newFace.x;
            _faceDirection.y = newFace.y;
        }

        public void clearInput()
        {
            _rollButtonPressed = false;
            _jumpButtonPressed = false;
        }

        public Vector2 currentFace()
        {
            Vector3 forwards = _controller.Forward();

            Vector2 output = new Vector2(forwards.x, forwards.z);

            return (output.normalized);
        }

        public void roll(Vector2 rollDirection)
        {
            if (rollDirection == Vector2.zero)
            {
                rollDirection = Vector2.up;
            }
            _movementInput.x = rollDirection.x;
            _movementInput.y = rollDirection.y;

            _rollButtonPressed = true;

        }
        public void endRoll()
        {
            _rollButtonPressed = false;
        }

        public void jump()
        {
            _jumpButtonPressed = true;
        }

        public void jumpEnd()
        {
            _jumpButtonPressed = false;
        }


    }
}
