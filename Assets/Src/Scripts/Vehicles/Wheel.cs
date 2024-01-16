using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Vehicles {
    [RequireComponent(typeof(WheelVisual))]
    public class Wheel : MonoBehaviour {
        [SerializeField] private Rigidbody _vehicleRigidbody;
        [SerializeField] private WheelVisual _visuals;

        [Header("Suspension")]
        [SerializeField, Range(0f, 10f)] private float _restDistance;
        [SerializeField] private float _strength;
        [SerializeField] private float _damping;

        [Header("Steering force")]
        [SerializeField] private float _gripFactor;
        [SerializeField] private float _tireMass;

        public bool IsGrounded { get; private set; } = true;

        private void Reset() {
            this._vehicleRigidbody = this.GetComponentInParent<Rigidbody>();
            this._visuals = this.GetComponent<WheelVisual>();
        }


        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(this.transform.position, .1f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position - this.transform.up * this._restDistance, .1f);
        }

        public void CalculateForces(float speed) {
            Transform wheelTransform = this.transform;
            this.IsGrounded = Physics.Raycast(wheelTransform.position, -wheelTransform.up, out RaycastHit hitInfo, this._restDistance);
            if (this.IsGrounded) {
                Vector3 tireVelocity = this._vehicleRigidbody.GetPointVelocity(wheelTransform.position);
                Vector3 springForce = this.CalculateSuspension(tireVelocity, hitInfo.distance);
                Vector3 steeringForce = this.CalculateSteeringForce(tireVelocity);
                this._vehicleRigidbody.AddForceAtPosition(springForce + steeringForce + wheelTransform.forward * speed, wheelTransform.position);
            } else {
                this._visuals.UpdateModelPosition(0f);
            }

        }

        private Vector3 CalculateSuspension(Vector3 tireVelocity, float distance) {
            float upVelocity = Vector3.Dot(this.transform.up, tireVelocity);
            float offset = this._restDistance - distance;
            float force = (offset * this._strength) - (upVelocity * this._damping);
            this._visuals.UpdateModelPosition(offset);
            return this.transform.up * force;
        }

        private Vector3 CalculateSteeringForce(Vector3 tireVelocity) {
            float steeringVel = Vector3.Dot(this.transform.right, tireVelocity);
            float velChange = -(steeringVel * this._gripFactor) / Time.fixedDeltaTime * this._tireMass;
            this._visuals.UpdateDriftVFXs(steeringVel);
            return this.transform.right * velChange;
        }
    }
}
