using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Vehicles;

namespace TankController {
    public class Player : MonoBehaviour {
        //[SerializeField] private Tank _tank;

        private void Update() {
            // this.MoveTank();
            // this.ControlTankTurret();
        }

        // private void MoveTank() {
        //     this._tank.Move(Input.GetAxisRaw("Vertical"));
        //     this._tank.Steer(Input.GetAxisRaw("Horizontal"));
        // }

        // private void ControlTankTurret() {
        //     //Rotate
        //     Plane plane = new Plane(Vector3.down, this._tank.transform.position.y);
        //     Ray ray = Cam.Instance.Camera.ScreenPointToRay(Input.mousePosition);

        //     if (plane.Raycast(ray, out float enter)) {
        //         Vector3 lookDirection = Vector3.ProjectOnPlane(ray.GetPoint(enter) - this._tank.transform.position, Vector3.up);
        //         this._tank.Turret.Rotate(this._tank.transform.InverseTransformDirection(lookDirection));
        //     }

        //     //Shoot
        //     if (Input.GetMouseButtonDown(0)) {
        //         this._tank.Turret.BeginShooting();
        //     } else if (Input.GetMouseButtonUp(0)) {
        //         this._tank.Turret.StopShooting();
        //     }
        // }

    }
}