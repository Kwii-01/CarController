using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Vehicles;

namespace CarController {
    public class Player : MonoBehaviour {
        [SerializeField] private Vehicle _vehicle;

        private void Update() {
            this.Move();
        }

        private void Move() {
            this._vehicle.Move(Input.GetAxisRaw("Vertical"));
            this._vehicle.Steer(Input.GetAxisRaw("Horizontal"));
        }
    }
}