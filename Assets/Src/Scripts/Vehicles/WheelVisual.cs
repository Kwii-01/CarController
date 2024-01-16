using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Vehicles {
    public class WheelVisual : MonoBehaviour {
        [Header("Suspension")]
        [SerializeField] private Transform _model;

        [Header("Drift")]
        [SerializeField] private float _driftThreshold;
        [SerializeField] private TrailRenderer _trailMarks;
        [SerializeField] private ParticleSystem _driftVFX;

        private Vector3 _baseModelLocalPosition;

        private void Awake() {
            this._baseModelLocalPosition = this._model.transform.localPosition;
        }

        public void UpdateModelPosition(float offset) {
            this._model.localPosition = this._baseModelLocalPosition + Vector3.up * offset;
        }

        public void UpdateDriftVFXs(float value) {
            bool threshold = Mathf.Abs(value) > this._driftThreshold;
            if (this._trailMarks) {
                this._trailMarks.emitting = threshold;
            }
            if (this._driftVFX) {
                if (threshold && this._driftVFX.isEmitting == false) {
                    this._driftVFX.Play();
                } else if (threshold == false && this._driftVFX.isEmitting) {
                    this._driftVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
            }
        }
    }
}