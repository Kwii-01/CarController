using System.Collections;
using System.Collections.Generic;

using Entities;

using UnityEngine;

namespace Vehicles {
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Vehicle : Entity {
        public Rigidbody rb;
        protected bool isBlackout;

        protected override void Reset() {
            base.Reset();
            this.rb = this.GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Rotate the vehicle
        ///
        /// 1 to turn right
        /// -1 to turn left
        /// </summary>
        /// <param name="range">between -1 and 1</param>
        public abstract void Steer(float range);

        /// <summary>
        /// Move the vehicle
        /// 
        /// 1 to go forward
        /// -1 to go backward
        /// </summary>
        /// <param name="range"></param>
        public abstract void Move(float range);// between -1 / 1

        /// <summary>
        /// Stop the vehicle motor
        /// </summary>
        public abstract void Stop();

        public abstract float GetNormalizedSpeed();
    }
}
