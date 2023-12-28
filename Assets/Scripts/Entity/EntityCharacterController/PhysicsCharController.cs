using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace EntityCharController
{
    public class PhysicsCharController : MonoBehaviour
    {
        private class force
        {
            

            protected Vector3 _vector;
            public Vector3 vector { get { return _vector; } }
            bool _enabled;

            public force(Vector3 force)
            {
                this._vector = force;
                _enabled = true;
            }
            public bool enabled {  get { return _enabled; } }
            public void setForce(Vector3 newForce)
            {
                this._vector = newForce;
            }

            public void setMagnitude(float magnitude)
            {
                _vector = _vector.normalized * magnitude;
            }

            public virtual void onFixedUpdate()
            {

            }
       
            public void Enable()
            {
                _enabled = true;
            }
            public void Disable()
            {
                _enabled = false;
            }
        }
        /// <summary>
        /// force that tries to maintain a velocity of context
        /// </summary>
        private class DampingForce : force
        {
            PhysicsCharController _ctx;

            Vector3 targetVelocity;
            float maxImpulse;


            /// these variables  <summary>
            /// these variables enabled adjusting
            /// </summary>
            bool xEnabled; 
            bool yEnabled;
            bool zEnabled;

            public DampingForce(Vector3 targetVelocity, PhysicsCharController context, float maxImpulse) : base(targetVelocity)
            {
                _vector = Vector3.zero;

                this.targetVelocity = targetVelocity;
                _ctx = context;
                this.maxImpulse = maxImpulse;

                xEnabled = true;
                yEnabled = true;
                zEnabled = true;
            }
            #region xyzEnabled setters
            public void disableX()
            {
                xEnabled = false;
            }

            public void enableX()
            {
                xEnabled = true;
            }

            public void enableY()
            {
                yEnabled = true;
            }
            public void disableY()
            {
                yEnabled = false;
            }

            public void enableZ()
            {
                zEnabled = true;
            }

            public void disableZ()
            {
                zEnabled = false;
            }
            #endregion


            public void setMaxImpulse(float newMax)
            {
                maxImpulse = newMax;
            }
            public void setTargetVelocity(Vector3 velocity)
            {
                targetVelocity = velocity;
            }

            public override void onFixedUpdate()
            {
                calculateDampingForce();
            }

            
            private void calculateDampingForce()
            {
                Vector3 curVelocity = _ctx.Velocity;

                _vector = targetVelocity - curVelocity;

                if (!xEnabled)
                {
                    _vector.x = 0;
                }
                if (!yEnabled)
                {
                    _vector.y = 0;
                }
                if (!zEnabled)
                {
                    _vector.z = 0;
                }



                Vector3.ClampMagnitude(_vector, maxImpulse);


            }

            
        }

        /// <summary>
        /// uses a damping force to set a velocity to a target velocity before scaling it up
        /// </summary>
        private class MovementForce : force
        {
            DampingForce _dampingForce;
            Vector3 targetVelocity;
            PhysicsCharController _ctx;
            float maxImpulse; /// controls the maxImpulse of the damping force, not the maxImpulse.
            float dotModifier; /// the increase of the force when dampened force and target velocity is directly away

            bool xEnabled;
            bool yEnabled;
            bool zEnabled;

            public MovementForce(Vector3 targetVelocity, PhysicsCharController context, float maxImpulse, float dotModifier = 1) : base(targetVelocity)
            {
                this.targetVelocity = targetVelocity;
                _dampingForce = new DampingForce(targetVelocity, context, maxImpulse);
                this._ctx = context;
                this.dotModifier = dotModifier;

                xEnabled = true;
                yEnabled = true;
                zEnabled = true;
            }
            #region xyzEnabled setters
            public void disableX()
            {
                xEnabled = false;
                _dampingForce.disableX();
            }

            public void enableX()
            {
                xEnabled = true;
                _dampingForce.enableX();
            }

            public void enableY()
            {
                yEnabled = true;
                _dampingForce.enableY();
            }
            public void disableY()
            {
                yEnabled = false;
                _dampingForce.disableY();
            }

            public void enableZ()
            {
                zEnabled = true;
                _dampingForce.enableZ();
            }

            public void disableZ()
            {
                zEnabled = false;
                _dampingForce.disableZ();
            }
            #endregion
            public override void onFixedUpdate()
            {
                
                _dampingForce.onFixedUpdate();

                Vector3 currentVelocity = _ctx.Velocity;

                if (!xEnabled)
                {
                    currentVelocity.x = 0;
                }
                if (!yEnabled)
                {
                    currentVelocity.y = 0;
                }
                if (!zEnabled)
                {
                    currentVelocity.z = 0;
                }

                float dot = Vector3.Dot(currentVelocity.normalized, _dampingForce.vector.normalized);
                _vector = _dampingForce.vector * getModifierFromDotProduct(dot);


            }

            public void setMaxImpulse(float newMax)
            {
                maxImpulse = newMax;
                _dampingForce.setMaxImpulse(maxImpulse);
            }
            public void setTargetVelocity(Vector3 velocity)
            {
                targetVelocity = velocity;
                _dampingForce.setTargetVelocity(velocity);
            }

            private float getModifierFromDotProduct(float dot)
            {
                if (dot >= 0 )
                {
                    return 1;
                }
                else
                {
                    return (dotModifier * dot * dot) + 1;
                }
            }

            ~MovementForce()
            {
                _dampingForce = null;
                _ctx = null;
            }
        }

        /// <summary>
        /// force that applies a specific change in velocity over a certain frames
        /// </summary>
        private class ImpulseForce : force
        {
            
            int frames;
            int curFrameCount;
            public ImpulseForce(Vector3 forceDirection, float mag, int frames) :base (forceDirection)
            {
                this._vector = forceDirection.normalized * (mag/frames);
                this.frames = frames;
                this.curFrameCount = 0;
            }

            public void setImpulse(Vector3 forceDirection, float mag, int frames)
            {
                this._vector = forceDirection.normalized * (mag / frames);
                this.frames = frames;
                this.curFrameCount = 0;
            }

            public override void onFixedUpdate()
            {
                curFrameCount++;  
                
                if (curFrameCount >= frames)
                {
                    this.Disable();
                }
            }


             
        }
        private enum ControllerState
        {
            Disable,
            Grounded,
            Airborne,
            Gliding,
            Roll

        }


        CharacterController _controller;
        ControllerState _controllerState;
        [SerializeField] MovementInfo _movementInfo;
        


        Vector2 movementInput;
        Vector3 _velocity;
        Vector3 _acceleration;
        Vector3 targetFacePosition;

        bool faceMovementInput = true;

        float turnSpeed;
        float speed;
        float movementMaxImpulse; /// how quickly we want to adjust to our desired velocity
        


        /// <summary>
        /// list of forces. known are
        /// Movement
        /// Gravity
        /// </summary>
        Dictionary<string, force> currentForces;


        /// <summary>
        /// getters
        /// </summary>
        /// 
        Vector3 Velocity { get { return _velocity; } }
        Vector3 Position { get { return _controller.transform.position; } }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            currentForces = new Dictionary<string, force>();

            _velocity = Vector3.zero;
            _acceleration = Vector3.zero;

            speed = _movementInfo.speed;
            turnSpeed = _movementInfo.turnSpeed;
        }
        


        private void FixedUpdate()
        {
            applyForces();

            handleMovement();
            handleRotation();
        }

        private void applyForces()
        {
            foreach (force force in currentForces.Values)
            {


                force.onFixedUpdate();

                if (force.enabled)
                {
                    _acceleration += force.vector;
                }
                


            }
            _velocity += _acceleration * Time.fixedDeltaTime;
        }
        private void handleMovement()
        {
            _controller.Move(_velocity * Time.fixedDeltaTime);
        }

        private void handleRotation()
        {
            Vector3 positionToLookAt = new Vector3(movementInput.x, 0, movementInput.y);

            Quaternion currentRotation = _controller.transform.rotation;

            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            _controller.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, turnSpeed * Time.deltaTime);

        }
        public void jump()
        {

        }

        public void roll()
        {

        }

        private void addForce(string name, force newForce)
        {
            if (currentForces.ContainsKey(name))
            {
                return;
            }

            currentForces.Add(name, newForce);
        }

        private force getForce(string name)
        {
            if (currentForces.ContainsKey (name))
            {
                return currentForces[name];
            }
            else
            {
                return null;
            }
        }

        public void moveHorizontal(Vector2 movement)
        {
            movementInput = movement.normalized;

            Vector3 newTargetVelocity = new Vector3(movementInput.x, 0, movementInput.y) * speed;

            MovementForce movementForce = this.getForce("Movement") as MovementForce;

            if (movementForce == null) 
            {
                createMovementForce();
                movementForce = this.getForce("Movement") as MovementForce;
            }

            movementForce.setTargetVelocity(newTargetVelocity);
            targetFacePosition = newTargetVelocity + this.Position;
        }
        
        private void createMovementForce()
        {
            if (this.getForce("Movement") == null)
            {
                Vector3 targetVelocity = new Vector3(movementInput.x, 0, movementInput.y);
                MovementForce newMovementForce = new MovementForce(targetVelocity, this, movementMaxImpulse, 1);
                newMovementForce.disableY();
                this.addForce("Movement", newMovementForce);
            }
            else
            {
                return;
            }
        }


    }
}
