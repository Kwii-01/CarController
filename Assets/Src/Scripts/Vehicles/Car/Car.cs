using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Vehicles {
    public class Car : Vehicle {
        private const float MAX_STEER = 30f;
        [Header("Car")]
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _braking;
        [SerializeField] private AnimationCurve _speedCurve;
        [SerializeField] private AnimationCurve _dragCurve;
        [SerializeField] private Wheel[] _motorWheels;
        [SerializeField] private Wheel[] _steeringWheels;

        [Header("Air control")]
        [SerializeField] private float _maxSpeedAir;
        [SerializeField] private float _accelerationAir;
        [SerializeField] private AnimationCurve _speedAirCurve;
        [SerializeField] private AnimationCurve _dragAirCurve;

        private float _power;
        private float _powerAC;
        private float _targetSteer;
        private float _steerSpeedFactor;
        private float _currentSpeed;
        private float _normalizedSpeed;

        protected override void Awake() {
            base.Awake();
            this.rb.maxAngularVelocity = this._maxSpeedAir;
        }

        protected void FixedUpdate() {
            this.ApplyForcesOnWheels();
            this.ApplyAirControl();
        }

        public override void Move(float direction) {
            if (this.isBlackout == false) {
                if (direction > 0) {
                    this._power = this._acceleration;
                } else if (direction < 0) {
                    this._power = this._braking;
                }
            }
        }

        public override void Stop() {
            this._power = 0f;
        }

        public override void Steer(float direction) {
            this._steerSpeedFactor = 2f;
            if (direction > 0) {
                this._targetSteer = MAX_STEER;
                this._powerAC = this._accelerationAir;
            } else if (direction < 0) {
                this._targetSteer = -MAX_STEER;
                this._powerAC = -this._accelerationAir;
            } else {
                this._powerAC = 0f;
                this._targetSteer = 0f;
                this._steerSpeedFactor = 4f;
            }
        }

        public bool IsGrounded() {
            foreach (Wheel wheel in this._motorWheels) {
                if (wheel.IsGrounded == false) {
                    return false;
                }
            }
            return true;
        }

        public override float GetNormalizedSpeed() {
            return this._normalizedSpeed;
        }

        private void ApplyForcesOnWheels() {
            float speed = this.CalculateSpeed();
            foreach (Wheel wheel in this._steeringWheels) {
                wheel.transform.localRotation = Quaternion.RotateTowards(wheel.transform.localRotation, Quaternion.AngleAxis(this._targetSteer, Vector3.up), this._steerSpeedFactor);
            }

            foreach (Wheel wheel in this._motorWheels) {
                wheel.CalculateForces(speed);
            }
        }

        private void ApplyAirControl() {
            if (this.IsGrounded() == false) {
                if (this._powerAC != 0f) {
                    this.rb.AddTorque(this.CalculateACInDirection(this.transform.up, this._powerAC));
                } else {
                    this.rb.AddTorque(this.rb.angularVelocity.normalized * this.CalculateAngularDrag());
                }
            }
        }

        private float CalculateSpeed() {
            this._currentSpeed = Vector3.Dot(this.transform.forward, this.rb.velocity);
            this._normalizedSpeed = Mathf.Clamp01(Mathf.Abs(this._currentSpeed) / this._maxSpeed);
            if (this._power != 0f) {
                return this._speedCurve.Evaluate(this._normalizedSpeed) * this._power;
            }
            return this._dragCurve.Evaluate(1 - this._normalizedSpeed) * -this._currentSpeed * this.rb.mass;
        }

        private float CalculateAngularDrag() {
            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(this.rb.angularVelocity.magnitude) / this._maxSpeedAir);
            return this._dragAirCurve.Evaluate(1 - normalizedSpeed) * -this.rb.angularVelocity.magnitude * this.rb.mass;
        }

        private Vector3 CalculateACInDirection(Vector3 direction, float acceleration) {
            float speed = Vector3.Dot(direction, this.rb.angularVelocity);
            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(speed) / this._maxSpeedAir);
            return direction * this._speedAirCurve.Evaluate(normalizedSpeed) * acceleration;
        }
    }
}