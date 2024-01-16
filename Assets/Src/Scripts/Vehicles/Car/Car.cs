using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Vehicles {
    public class Car : Vehicle {
        [Header("Acceleration")]
        [SerializeField] private float _accelerationTime;
        [SerializeField] private AnimationCurve _accelerationCurve;
        [SerializeField] private Wheel[] _motorWheels;

        [Header("Steering")]
        [SerializeField] private float _maxSteer = 35f;
        [SerializeField] private float _baseSteerSpeed = 4f;
        [SerializeField] private float _steerSpeed = 2f;
        [SerializeField] private Wheel[] _steeringWheels;

        public bool IsMoving { get; private set; } = false;

        private float _power;
        private float _accelerationTimer;
        private float _targetSteer;
        private float _steerSpeedFactor;
        private float _currentSpeed;
        private float _normalizedSpeed;

        private void Start() {
            this._accelerationTimer = this._accelerationTime;
        }

        private void Update() {
            if (this.IsMoving) {
                if (this._accelerationTimer < this._accelerationTime) {
                    this._accelerationTimer += Time.deltaTime;
                }
            }
        }

        protected void FixedUpdate() {
            this.ApplyForcesOnWheels();
            this.ClampSpeed();
        }

        public override void Move(float direction) {
            if (this.isBlackout == false) {
                this._power = this.statBehaviour.Get(StatSystem.StatType.Acceleration) * direction;
                if (this.IsMoving == false && direction != 0) {// JUST BEGAN TO SPEED
                    this._accelerationTimer = 0f;
                }
                this.IsMoving = direction != 0f;
                if (this.IsMoving == false) {
                    this.Stop();
                }
            }
        }

        public override void Stop() {
            this._power = 0f;
            this.IsMoving = false;
            this._accelerationTimer = this._accelerationTime;
        }

        public override void Steer(float direction) {
            this._steerSpeedFactor = this._steerSpeed;
            if (direction > 0) {
                this._targetSteer = this._maxSteer;
            } else if (direction < 0) {
                this._targetSteer = -this._maxSteer;
            } else {
                this._targetSteer = 0f;
                this._steerSpeedFactor = this._baseSteerSpeed;
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

        private float CalculateSpeed() {
            this._currentSpeed = Vector3.Dot(this.transform.forward, this.rb.velocity);
            this._normalizedSpeed = Mathf.Clamp01(Mathf.Abs(this._currentSpeed) / this.statBehaviour.Get(StatSystem.StatType.MoveSpeed));
            if (this.IsMoving) {
                return this._accelerationCurve.Evaluate(this._accelerationTimer / this._accelerationTime) * this._power;
            }
            return -this._currentSpeed * this.rb.mass * 2f;
        }

        private void ClampSpeed() {
            Vector3 velocity = this.transform.InverseTransformDirection(this.rb.velocity);
            float maxSpeed = this.statBehaviour.Get(StatSystem.StatType.MoveSpeed);
            velocity.z = Mathf.Clamp(velocity.z, -maxSpeed, maxSpeed);
            this.rb.velocity = this.transform.TransformDirection(velocity);
        }

    }
}