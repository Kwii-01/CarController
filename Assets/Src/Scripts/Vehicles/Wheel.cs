using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Vehicles {
    public class Wheel : MonoBehaviour {
        public Rigidbody rbVehicle;

        [Header("Suspension")]
        [SerializeField, Range(0f, 10f)] private float _restDistance;
        [SerializeField] private float _strength;
        [SerializeField] private float _damping;

        [Header("Steering force")]
        [SerializeField] private float _gripFactor;
        [SerializeField] private float _tireMass;

        public bool IsGrounded { get; private set; } = true;
        public float Offset;

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(this.transform.position, .25f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position - this.transform.up * this._restDistance, .25f);
        }

        public void CalculateForces(float speed) {
            Transform wheelTransform = this.transform;
            this.IsGrounded = Physics.Raycast(wheelTransform.position, -wheelTransform.up, out RaycastHit hitInfo, this._restDistance);
            if (this.IsGrounded) {
                Vector3 tireVelocity = this.rbVehicle.GetPointVelocity(wheelTransform.position);
                Vector3 springForce = this.CalculateSuspension(tireVelocity, hitInfo.distance);
                Vector3 steeringForce = this.CalculateSteeringForce(tireVelocity);
                this.rbVehicle.AddForceAtPosition(springForce + steeringForce + wheelTransform.forward * speed, wheelTransform.position);
            }
        }

        private Vector3 CalculateSuspension(Vector3 tireVelocity, float distance) {
            float upVelocity = Vector3.Dot(this.transform.up, tireVelocity);
            float offset = this._restDistance - distance;
            float force = (offset * this._strength) - (upVelocity * this._damping);
            this.Offset = offset;
            return this.transform.up * force;
        }

        private Vector3 CalculateSteeringForce(Vector3 tireVelocity) {
            float steeringVel = Vector3.Dot(this.transform.right, tireVelocity);
            float velChange = -(steeringVel * this._gripFactor) / Time.fixedDeltaTime * this._tireMass;
            return this.transform.right * velChange;
        }
    }
}
